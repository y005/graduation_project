# 주피터 노트북 환경에서 youtube data v3 api 사용방법 절차

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

## 프로젝트 파일 설명
`youtubeStat(19_08-20_07).ipynb` : 19년 8월부터 20년 7월까지의 주식 관련 동영상 정보를 수집하는 코드
`youtube title extract.ipynb` : 비디오 아이디를 이용해 수집한 영상들의 제목을 추가하는 코드
`youtubeComment.ipynb` : 비디오의 특정 댓글들만 수집하는 코드
`Live chat from youtube.ipynb` : 라이브 비디오의 댓글을 수집하는 코드
`sentiment analysis video comment.ipynb` : 수집한 영상 댓글들을 감성분석하는 코드
`sentiment analysis(make model).ipynb` : 영화리뷰 데이터를 이용해 감성분석기를 학습하는 코드
`sentiment analysis(using model).ipynb` : 학습된 감성분석기를 사용하는 코드

