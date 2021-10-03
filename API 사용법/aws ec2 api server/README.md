# AWS EC2를 이용한 Restful Api 서버 만들기

1. [AWS IAM 사용자 생성](https://victorydntmd.tistory.com/67?category=682759): EC2 + S3 권한 부여된 사용자 생성 
2. EC2 생성 (instance: t2.micro/AMI: Ubuntu/역할: 1에서의 사용자가 만든 EC2 + S3 역할을 부여)
3. EC2 보안그룹 설정(인바운드 규칙: 80(web),22(ssh),5000(flask)포트만 추가/아웃바운드 규칙: 모든 트래픽)
4. 탄력적 IP 생성 후 EC2와 연결 
5. SSH를 통한 EC2 서버 접속
```bash
ssh -i "testKey.pem" ubuntu@ec2-3-37-190-174.ap-northeast-2.compute.amazonaws.com
```
6. EC2 서버의 기본적인 세팅을 한 후 `Git`을 통해 프로젝트 클론
```bash
git clone "Restful api서버와 관련된 프로젝트 주소" 
```
7. flask 코드 실행    
```bash
nohup python3 app.py &
```

## 프로젝트 관련 기타 유용한 정보 모음들

### AWS의 인터넷 설정 과정
1. VPC 생성 
2. VPC 안에 프라이빗 서브넷 생성
3. 인터넷 게이트웨이 생성
4. 퍼블릭 서브넷 생성(라우팅 테이블에 2와 3을 연결)
  (ACL의 경우 여러 서브넷 접근제한 가능)

### EC2의 용량 업그레이드 하기
1. VPC 생성 
2. VPC 안에 프라이빗 서브넷 생성
3. 인터넷 게이트웨이 생성
4. 퍼블릭 서브넷 생성(라우팅 테이블에 2와 3을 연결)
  (ACL의 경우 여러 서브넷 접근제한 가능)

### 대용량 파일을 Git에 업로드 하기

0. [EBS 볼륨 확장하기](https://ithub.tistory.com/253)  

1. 파티션 크기 확인
```bash
lsblk
```
2. 파티션 크기 조절
```bash
sudo growpart /dev/xvdf 1
```
3. 파일 시스템 크기 확장
```bash
sudo resize2fs /dev/xvdf1
```
4. 디스크 용량 확인
```bash
df -h
```

### 1시간 마다 파이썬 코드 실행하는 방법 
```bash
crontab -e
* */1 * * * /usr/bin/python /home/ubuntu/test.py       
```
