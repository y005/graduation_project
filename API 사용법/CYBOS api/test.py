import win32com.client
instCpCybos = win32com.client.Dispatch("CpUtil.CpCybos")
#연결 상태 확인
print(instCpCybos.IsConnect)
#종목 정보 클래스 호출 
instCpStockCode = win32com.client.Dispatch("CpUtil.CpStockCode")
#전체 종목 갯수 출력
print(instCpStockCode.GetCount())
#0번 인덱스의 1번 정보인 종목명 출력
print(instCpStockCode.GetData(1, 0))
#0~9번 인덱스의 1번 정보인 종목명 출력
for i in range(0, 10):
    print(instCpStockCode.GetData(1,i))
#네이버라는 인덱스의 정보(종목코드,종목명,FULL CODE) 출력
stockNum = instCpStockCode.GetCount()

for i in range(stockNum):
    if instCpStockCode.GetData(1, i) == 'NAVER':
        print(instCpStockCode.GetData(0,i))
        print(instCpStockCode.GetData(1,i))
        print(i)
        
#GetStockListByMarket를 이용해 유가증권 종목들의 코드 정보들을 튜플 형태로 입력받았음
instCpCodeMgr = win32com.client.Dispatch("CpUtil.CpCodeMgr")
codeList = instCpCodeMgr.GetStockListByMarket(1)
#종목 코드 튜플에서 각 코드에 해당하는 이름을 딕셔너리에 저장 
kospi = {}
for code in codeList:
    name = instCpCodeMgr.CodeToName(code)
    kospi[code] = name
#해당 파일을 CSV형태로 저장  
f = open('C:\\Users\\82102\\Desktop\\연구 인턴\\stock api\\kospi.csv', 'w')
for key, value in kospi.items():
    f.write("%s,%s\n" % (key, value))
f.close()
#GetStockSectionKind 메서드에 인자로 종목 코드를 전달하면 부구분 코드를 반환 
#for i, code in enumerate(codeList):
#    secondCode = instCpCodeMgr.GetStockSectionKind(code)
#    name = instCpCodeMgr.CodeToName(code)
#    print(i, code, secondCode, name)

#request/reply 형태로 진행되는 거래차트 정보 가져오기 
instStockChart = win32com.client.Dispatch("CpSysDib.StockChart")

#원하는 종목의 코드
instStockChart.SetInputValue(0, "A003540")
#기간/갯수을 기준으로 한 정보 범위 설정 
instStockChart.SetInputValue(1, ord('2'))
#갯수 10개
instStockChart.SetInputValue(4, 10)
#종가 정보를 요청
instStockChart.SetInputValue(5, 5)
#하루 기준
instStockChart.SetInputValue(6, ord('D'))
instStockChart.SetInputValue(9, ord('1'))

instStockChart.BlockRequest()

numData = instStockChart.GetHeaderValue(3)
for i in range(numData):
    print(instStockChart.GetDataValue(0, i))

#여러개의 정보(일자,시가,고가,저가,종가,거래량)를 가져오기
instStockChart.SetInputValue(0, "A003540")
instStockChart.SetInputValue(1, ord('2'))
instStockChart.SetInputValue(4, 10)
instStockChart.SetInputValue(5, (0, 2, 3, 4, 5, 8))
instStockChart.SetInputValue(6, ord('D'))
instStockChart.SetInputValue(9, ord('1'))

instStockChart.BlockRequest()

#받은 종목 갯수와 정보 필드의 갯수를 가져오기 
numData = instStockChart.GetHeaderValue(3)
numField = instStockChart.GetHeaderValue(1)

for i in range(numData):
    for j in range(numField):
        print(instStockChart.GetDataValue(j, i), end=" ")
    print("")

#종목들의 (현재가,PER,EPS,최근 분기연월) 정보 가져오기
instMarketEye = win32com.client.Dispatch("CpSysDib.MarketEye")
instMarketEye.SetInputValue(0, (4, 67, 70, 111))
instMarketEye.SetInputValue(1, 'A003540')
instMarketEye.BlockRequest()

print("현재가: ", instMarketEye.GetDataValue(0, 0))
print("PER: ", instMarketEye.GetDataValue(1, 0))
print("EPS: ", instMarketEye.GetDataValue(2, 0))
print("최근분기년월: ", instMarketEye.GetDataValue(3, 0))

#전체 유가증권 종목 중 거래량이 평균거래량보다 10배 이상인 종목 정보 출력
import time

def CheckVolumn(instStockChart, code):
    #code에 해당하는 종목의 60일간 거래량 수집
    instStockChart.SetInputValue(0, code)
    instStockChart.SetInputValue(1, ord('2'))
    instStockChart.SetInputValue(4, 60)
    instStockChart.SetInputValue(5, 8)
    instStockChart.SetInputValue(6, ord('D'))
    instStockChart.SetInputValue(9, ord('1'))
    instStockChart.BlockRequest()

    #거래량을 배열에 저장 
    volumes = []
    numData = instStockChart.GetHeaderValue(3)
    for i in range(numData):
        volume = instStockChart.GetDataValue(0, i)
        volumes.append(volume)

    #59일 동안의 평균 거래량 계산
    averageVolume = (sum(volumes) - volumes[0]) / (len(volumes) -1)

    if(volumes[0] > averageVolume * 10):
        return 1
    else:
        return 0

if __name__ == "__main__":
    instStockChart = win32com.client.Dispatch("CpSysDib.StockChart")
    instCpCodeMgr = win32com.client.Dispatch("CpUtil.CpCodeMgr")
    codeList = instCpCodeMgr.GetStockListByMarket(1)
    cnt = 0
    buyList = []
    print("한 달 동안의 평균 거래량보다 10배 이상을 기록한 종목 정보입니다.")
    for code in codeList:
        if cnt > 4 : break
        if CheckVolumn(instStockChart, code) == 1:
           buyList.append(code)
           print(instCpCodeMgr.CodeToName(code))
           cnt += 1
        #거래량의 과부화를 막기 위한 함수     
        time.sleep(1)



instCpCodeMgr = win32com.client.Dispatch("CpUtil.CpCodeMgr")
instMarketEye = win32com.client.Dispatch("CpSysDib.MarketEye")

# 음식료품 업종의 코드 리스트를 파이썬 튜플 형태로 반환
tarketCodeList = instCpCodeMgr.GetGroupCodeList(5)

# per정보 가져오기 
instMarketEye.SetInputValue(0, 67)
#가져올 종목들 리스트 전달
instMarketEye.SetInputValue(1, tarketCodeList)
instMarketEye.BlockRequest()

# 종목 갯수 가져오기 
numStock = instMarketEye.GetHeaderValue(2)

# GetData
sumPer = 0
for i in range(numStock):
    sumPer += instMarketEye.GetDataValue(0, i)

#음식료품 업종에 대한 평균 PER 출력
print("Average PER: ", sumPer / numStock)
