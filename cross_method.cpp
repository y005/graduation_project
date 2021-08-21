#include "cross_method.h"
#include <numeric>
#include <cmath>
#include <chrono>
#include <boost/math/distributions/students_t.hpp>
#include <boost/accumulators/statistics.hpp>
#include <boost/accumulators/accumulators.hpp>
#include <boost/accumulators/statistics/variance.hpp>
using namespace boost::accumulators;

int cross_calculate(string Xfile, string Yfile, string output, int X_rows, double lowRange) {
	//struct type: result.list, fold_change, p-value

	// vector of {E_id, descriptivity fc, pval & predictivity fc, pval}
	vector<CRESULT> result;

	register int k=0;

	// time measurement : step 1
	using clock = chrono::system_clock;
	using sec = chrono::duration<double>;
	const auto start = clock::now();

	//#1 Extract data from X
	vector<pid> Xdata = get_line(Xfile, X_rows);
	if (Xdata.empty()) { cout << "X data is empty\n";}
	//#2 Extract data from Y
	vector<pair<string,vector<pid>>> Ydata;
	get_all(Yfile, Ydata);

	const sec end = clock::now() - start;
	cout << "[STEP 1] " << end.count() << endl;

	//#3 Merge data
	if (!Xdata.empty() && !Ydata.empty()) {
		for (int j = 0; j < Ydata.size(); j++) {

			CRESULT r;
			vector<pair<int, pdd>> list;

			// time measurement : step 2
			using clock = chrono::system_clock;
			using sec = chrono::duration<double>;
			const auto start2 = clock::now();

			// merge X and Y
			merge(Xdata, Ydata[j].second, list);

			const sec end2 = clock::now() - start2;
			if(j==0)
				cout << "[STEP 2] " << end2.count() << endl;

			// time measurement : step 3
			const auto start3 = clock::now();

			if (!list.empty()) {

				//#4 seperate into two group(0.25/0.5)
				register int i;

				// descriptivity
				vector<double> low;
				vector<double> high;
				double x_med = 0, y_med = 0;

				// predictivity
				vector<double> p_low;
				vector<double> p_high;

				// descriptivity
				nth_element(list.begin(), list.begin() + (int)(list.size()*lowRange), list.end(), [](pair<int, pdd> a, pair<int, pdd> b)-> bool {return a.second.first < b.second.first; });

				// if list has even number of elements
				if (list.size() % 2 == 0) {
					x_med = (list[int(list.size() *lowRange)].second.first + list[int(list.size() *lowRange) - 1].second.first) / 2;
					for (i = 0; i < (int)(list.size()); i++) {
						if (list[i].second.first <= x_med)
							low.emplace_back(list[i].second.second);
					}
				}
				// if list has odd number of elements
				else {
					x_med = list[int(list.size() *lowRange)].second.first;
					for (i = 0; i < (int)(list.size()); i++) {
						if (list[i].second.first <= x_med)
							low.emplace_back(list[i].second.second);
					}
				}

				nth_element(list.begin(), list.begin() + (int)(list.size()*(1 - lowRange)), list.end(), [](pair<int, pdd> a, pair<int, pdd> b)-> bool {return a.second.first < b.second.first; });

				if (list.size() % 2 == 0) {
					x_med = (list[int(list.size() *(1 - lowRange))].second.first + list[int(list.size() *(1 - lowRange)) - 1].second.first) / 2;
					for (i = 0; i < list.size(); i++) {
						if (list[i].second.first > x_med)
							high.emplace_back(list[i].second.second);
					}
				}
				else {
					x_med = list[int(list.size() *(1 - lowRange))].second.first;
					for (i = 0; i < list.size(); i++) {
						if (list[i].second.first > x_med)
							high.emplace_back(list[i].second.second);
					}
				}

				if (low.size() < 2 || high.size() < 2) {

					r.E_id = Ydata[j].first;
					//cout << r.E_id << " empty data : descriptivity\n";
					r.fc = 0.0;
					r.p = 0.0;
				}
				//#5 calculate fold_change & p-value between low, high group
				else {
					accumulator_set<double, stats<tag::variance, tag::mean > > lowSet;
					accumulator_set<double, stats<tag::variance, tag::mean > > highSet;
					lowSet = for_each(low.begin(), low.end(), lowSet);
					highSet = for_each(high.begin(), high.end(), highSet);

					r.E_id = Ydata[j].first;
					r.fc = mean(highSet) - mean(lowSet);
					r.p = p_val(mean(lowSet), mean(highSet),low, high);
					//r.p = student_ttest(mean(lowSet), sqrt(variance(lowSet)), low.size(), mean(highSet), sqrt(variance(highSet)), high.size());
				}
				// init
				low.clear();
				high.clear();

				// predictivity
				nth_element(list.begin(), list.begin() + (int)(list.size()*lowRange), list.end(), [](pair<int, pdd> a, pair<int, pdd> b)-> bool {return a.second.second < b.second.second; });

				// if dr has even number of elements
				if (list.size() % 2 == 0) {
					y_med = (list[int(list.size() *lowRange)].second.second + list[int(list.size() *lowRange) - 1].second.second) / 2;
					for (i = 0; i < (int)(list.size()); i++) {
						if (list[i].second.second <= y_med)
							p_low.emplace_back(list[i].second.first);
					}
				}
				else {
					y_med = list[int(list.size() *lowRange)].second.second;

					for (i = 0; i < (int)(list.size()); i++) {
						if (list[i].second.second <= y_med)
							p_low.emplace_back(list[i].second.first);
					}
				}

				nth_element(list.begin(), list.begin() + (int)(list.size()*(1 - lowRange)), list.end(), [](pair<int, pdd> a, pair<int, pdd> b)-> bool {return a.second.second < b.second.second; });
				if (list.size() % 2 == 0) {
					y_med = (list[int(list.size() *(1 - lowRange))].second.second + list[int(list.size() *(1 - lowRange)) - 1].second.second) / 2;
					for (i = 0; i < list.size(); i++) {
						if (list[i].second.second > y_med)
							p_high.emplace_back(list[i].second.first);
					}
				}
				else {
					y_med = list[int(list.size() *(1 - lowRange))].second.second;

					for (i = 0; i < list.size(); i++) {
						if (list[i].second.second > y_med)
							p_high.emplace_back(list[i].second.first);
					}
				}

				if (p_low.size() < 2 || p_high.size() < 2) {
					//cout << r.E_id << " empty data : predictivity\n";
					r.Pfc = 0.0;
					r.Pp = 0.0;

				}
				else {
					//#5 calculate fold_change & p-value between low, high group
					accumulator_set<double, stats<tag::variance, tag::mean > > p_lowSet;
					accumulator_set<double, stats<tag::variance, tag::mean > > p_highSet;
					p_lowSet = for_each(p_low.begin(), p_low.end(), p_lowSet);
					p_highSet = for_each(p_high.begin(), p_high.end(), p_highSet);

					r.Pfc = mean(p_highSet) - mean(p_lowSet);
					r.Pp = p_val(mean(p_lowSet), mean(p_highSet), p_low, p_high);
					//r.Pp = student_ttest(mean(p_lowSet), sqrt(variance(p_lowSet)), p_low.size(), mean(p_highSet), sqrt(variance(p_highSet)), p_high.size());

				}
				// initilization for the next loop
				p_low.clear();
				p_high.clear();
				result.emplace_back(r);

				const sec end3 = clock::now() - start3;
				if(j==0)
					cout << "[STEP 3] " << end3.count() << endl;

				k++;
			}
		}
	}
	else { cout << "Y data is empty\n"; }

	// save result as csv file
	//cross_save(output, result);

	return 0;
}

void cross_save(string filename, vector<CRESULT> results) {

	register int i;

	ofstream output;
	output.open("./data/"+filename + ".csv");

	output << "E_id,Descriptivity (fold change),Descriptivity (p-value),Predictivity (fold change),Predictivity (p-value)\n";

	for (i = 0; i < results.size(); i++) {
		output << results[i].E_id << "," << results[i].fc << "," << results[i].p << "," << results[i].Pfc << "," << results[i].Pp << "\n";
	}

	output.close();
}
