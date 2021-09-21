# 유니티 환경에서 youtube data v3 api 사용방법 절차

[전체 과정 참고 링크](https://brunch.co.kr/@joypinkgom/49)

## Youtube data api v3
* 특정 키워드로 검색한 동영상들에 대한 조회수, 추천/비추천 갯수, 댓글 갯수 등의 정보 제공
* 하루에 10000크레딧 사용가능 가능하고 읽기 1, 쓰기 50, 업로드는 1600이 차감됨

[사용 예제](https://brunch.co.kr/@joypinkgom/75)

## 구글 클라우드 플랫폼 콘솔에서 새 프로젝트 생성
[프로젝트 생성하기](https://console.cloud.google.com)

## 인증 방법 
* 특정 사용자에게 허용된 API를 사용 시 oAuth 2.0
* 그렇지 않은 경우 API키 사용, 서비스 계정 사용(API키는 테스트용으로만 사용되고 서비스 계정을 통한 사용을 권고함)

[Net 환경 기준 간단한 사용 예제1](https://developers.google.com/api-client-library/dotnet/get_started)
[Net 환경 기준 간단한 사용 예제2](https://developers.google.com/youtube/v3/code_samples/dotnet?hl=ko)

## 사용방법
```bash
string query = "https://www.googleapis.com/youtube/v3";
query += pageToken(요청하고 싶은 데이터 종류)
query += api키값
WWW w = new WWW(query);
JObject result = JObject.Parse(w.text);
Debug.Log(result);
```
[다른 api 요청 예시](https://developers.google.com/youtube/v3/docs/channels/list)
