import sys
import ctypes
import win32com.client


g_objCodeMgr = win32com.client.Dispatch('CpUtil.CpCodeMgr')
g_objCpStatus = win32com.client.Dispatch('CpUtil.CpCybos')
g_objCpTrade = win32com.client.Dispatch('CpTrade.CpTdUtil')

if ctypes.windll.shell32.IsUserAnAdmin():
    print('정상: 관리자권한으로 실행된 프로세스입니다.')
else:
    print('오류: 일반권한으로 실행됨. 관리자 권한으로 실행해 주세요')
    exit()    
if (g_objCpStatus.IsConnect == 0):
    print("PLUS가 정상적으로 연결되지 않음. ")
    exit()       
    
if (g_objCpTrade.TradeInit(0) != 0):
    print("주문 초기화 실패")
    exit()    
    
acc = g_objCpTrade.AccountNumber[0]  # 계좌번호
accFlag = g_objCpTrade.GoodsList(acc, 1)  # 주식상품 구분
print("계좌번호: ",acc)

objRq = win32com.client.Dispatch("CpTrade.CpTd6033")
objRq.SetInputValue(0, acc)  # 계좌번호
objRq.SetInputValue(1, accFlag[0])  # 상품구분 - 주식 상품 중 첫번째
objRq.SetInputValue(2, 50)  # 요청 건수(최대 50)

objRq.BlockRequest()

rqStatus = objRq.GetDibStatus()
rqRet = objRq.GetDibMsg1()
print("통신상태", rqStatus, rqRet)
if rqStatus != 0:
    print("통신이 안됨")
    exit() 
cnt = objRq.GetHeaderValue(7)
print("보유 종목 갯수: ",cnt)
print("")

jangoData = {}
for i in range(cnt):
    item = {}
    code = objRq.GetDataValue(12, i)  # 종목코드
    item['종목코드'] = code
    item['종목명'] = objRq.GetDataValue(0, i)  # 종목명
    #item['현금신용'] = dicflag1[objRq.GetDataValue(1,i)] # 신용구분
    #print(code, '현금신용', item['현금신용'])
    #item['대출일'] = objRq.GetDataValue(2, i)  # 대출일
    item['잔고수량'] = objRq.GetDataValue(7, i)  # 체결잔고수량
    #item['매도가능'] = objRq.GetDataValue(15, i)
    item['장부가'] = objRq.GetDataValue(17, i)  # 체결장부단가
    item['평가금액'] = objRq.GetDataValue(9, i)  # 평가금액(천원미만은 절사 됨)
    item['평가손익'] = objRq.GetDataValue(11, i)  # 평가손익(천원미만은 절사 됨)
    # 매입금액 = 장부가 * 잔고수량
    item['매입금액'] = item['장부가'] * item['잔고수량']
    #item['현재가'] = 0
    #item['대비'] = 0
    #item['거래량'] = 0
    print("종목코드: ",item['종목코드'])
    print("종목명: ",item['종목명'])
    print("잔고수량: ",item['잔고수량'])
    print("장부가: ",item['장부가'])
    print("총매입금액: ",item['매입금액'])
    print("평가금액: ",item['평가금액'])
    print("평가손익: ",item['평가손익'])  
    print("")
    if i >= 10:  # 최대 10 종목만
        break
