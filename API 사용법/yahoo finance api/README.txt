##유니티 Window PC환경에서 yahoo finance api사용절차##
1. yahoo finance api에 로그인 후 Basic Subscribe
- 국내증권사와 다르게 로그인만 하면 사용가능해 편리함
- 자신이 발급받은 api key를 코드에 자동으로 기입해줌
- 다양한 예제코드들과 제공되는 언어환경이 폭넓음

관련 url:
https://rapidapi.com/apidojo/api/yahoo-finance1

2. request 받은 파일을 json형태로 파싱해야 값들을 읽어야 합니다.
- unity에 json관련 라이브러리인 newtonsoft.json이 설치되어야 함
- 홈페이지에서 패키지 파일다운 한 뒤 확장자를 zip으로 바꾼 뒤 압축해제
- lib/net20에 있는 newtonsoft.json.dll 파일을 Asset/Plugins에 넣기

참고url:
https://docs.microsoft.com/ko-kr/visualstudio/gamedev/unity/unity-scripting-upgrade
https://stackoverflow.com/questions/18784697/how-to-import-jsonconvert-in-c-sharp-application

3. 사용 코드
- http로 받은 string 결과물을 json 형태로 파싱해야 함
>>using Newtonsoft.Json; 
>>var body = await response.Content.ReadAsStringAsync();
>>JObject obj = JObject.Parse(body);
>>Debug.Log(obj["index"]["index"])

받은 정보들에 대한 JSON 형태의 스키마는 Debug.Log(obj)를 통해
확인한 다음에 해당하는 index를 넣어서 접근하세요.

참고url:
https://stackoverflow.com/questions/39468096/how-can-i-parse-json-string-from-httpclient
            