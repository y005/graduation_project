https://brunch.co.kr/@joypinkgom/49
Youtube data api (v3) 사용
1. 구글 클라우드 플랫폼 콘솔에서 새 프로젝트 생성
프로젝트 생성 url: https://console.cloud.google.com

2. Youtube data api (v3) 사용
	search result: 검색한 키워드에 대한 동영상, 재생목록들의 정보 
	조회수, 추천 갯수, 댓글 갯수 등의 정보 제공
실제 사용예제url: https://brunch.co.kr/@joypinkgom/75

3. 인증 방법 
특정 사용자에게 허용된 API를 사용 시 oAuth 2.0
그렇지 않은 경우 API키 사용, 서비스 계정 사용(API키는 테스트용으로만 사용되고 서비스 계정을 통한 사용을 권고함)
서비스 계정에서 소유자 권한 생성 완료시 계정정보가 담긴 JSON파일이 다운로드
하루에 10000크레딧 사용가능 가능하고 읽기 1, 쓰기 50, 업로드는 1600이 차감됨
.Net 환경 기준 간단한 C# 사용 예제 url:
https://developers.google.com/api-client-library/dotnet/get_started
https://developers.google.com/youtube/v3/code_samples/dotnet?hl=ko

4. 유니티에서 사용방법
string query = "https://www.googleapis.com/youtube/v3";
query += pageToken(요청하고 싶은 데이터 종류)
query += api키값
WWW w = new WWW(query);
JObject result = JObject.Parse(w.text);
Debug.Log(result);

5. 파이썬에서 사용방법
첨부된 자료 참고

주소 예시
https://developers.google.com/youtube/v3/docs/channels/list


