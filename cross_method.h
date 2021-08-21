#include "common_io.hpp"
#include "common_ttest.h"
#include "box_method.h"
#include <tuple>


struct c_result { string E_id;  double fc; double p; double Pfc; double Pp; };
typedef struct c_result CRESULT;

int cross_calculate(string Xfile, string Yfile, string output, int X_rows, double lowRange);
void cross_save(string filename, vector<CRESULT> results);