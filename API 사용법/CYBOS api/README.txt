##국내 대신증권 CYBOS API 사용절차입니다.##
1. 대신증권 사용 준비
>>비대면 가상계좌 개설/CYBOS 5 설치
>>모의투자 회원가입/모의투자 신청(모의투자 매매할때도 여기 페이지에서 해야됨!)
https://vt.daishin.com/ds/cybos/info/info.do?m=4102&p=5492&v=4630
>>cybos5 hts에서 번호 1846을 누르고 시스템 트레이딩 신청
https://money2.daishin.com/e5/mboard/ptype_basic/HTS_Plus_Helper/DW_Basic_Read_Page.aspx?boardseq=296&seq=219&page=1&searchString=&p=&v=&m=

2. python 32bit를 가상환경과 연결
- pyvenv.txt 내 파이썬 연결 정보 수정
  home = 실행할 파이썬 위치
  version = 실행할 파이썬 버전
- py32에 있는 가상환경 세팅
>> pip install -r requirements.txt
>> python -m venv C:\Users\82102\Desktop\stock api\py32 

3. python 코드 실행
- 관리자 권한으로 cybos plus, cmd 실행
- cybos hts 로그인 화면 위에 있는 모의투자 버튼을 클릭한 후 만든 모의투자 ID/PWD/공동인증서로 로그인
- 로그인 설정에서 모의투자설정
- 가상환경 실행 후 코드 실행
(모의투자와 실제 투자는 로그인하는 방법만 다르고 api접근방식에는 차이가 X)

>> cd C:\Users\82102\Desktop\stock api\py32\Scripts
>> activate
>> cd C:\Users\82102\Desktop\stock api
//종목 정보 관련 코드
>> python test.py
//보유 주식정보 관련 코드
>> python mystock.py