# 유니티 환경에서 yahoo finance api사용절차

## yahoo finance api
* 국내증권사와 다르게 로그인만 하면 사용가능해 편리함
* 자신이 발급받은 api key를 코드에 자동으로 기입해줌
* 다양한 예제코드들과 제공되는 언어환경이 폭넓음

## 야후 파이낸스 api 키 발급하기
yahoo finance api에 로그인 후 Basic Subscribe: [여기](https://rapidapi.com/apidojo/api/yahoo-finance1)로 가서 basic subscribe 구독(한 달에 500건 호출 무료 제공)
  
## 유니티 환경(net 2.0)에서의 통신 환경 세팅
1. unity에 json관련 라이브러리인 newtonsoft.json을 [홈페이지](https://www.nuget.org/packages/Newtonsoft.Json/)에서 Download package 클릭
2. 다운로드한 파일을 찾아서 확장자를 `.nupkg` 에서 `.zip` 으로 변경합니다
3. 압축 해제된 폴더에 `lib/net20`에 있는 `newtonsoft.json.dll`를 `Asset/Plugins`에 세팅

[참고 링크](https://docs.microsoft.com/ko-kr/visualstudio/gamedev/unity/unity-scripting-upgrade)

## 사용 코드 예시
```bash
using Newtonsoft.Json; 
var body = await response.Content.ReadAsStringAsync();
JObject obj = JObject.Parse(body);
Debug.Log(obj["index"]["index"])
```
[참고 링크](https://stackoverflow.com/questions/39468096/how-can-i-parse-json-string-from-httpclient)
