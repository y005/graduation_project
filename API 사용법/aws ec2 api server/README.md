# AWS EC2를 이용한 Restful Api 서버 만들기

1. [AWS IAM 사용자 생성](https://victorydntmd.tistory.com/67?category=682759): EC2 + S3 권한 부여된 사용자 생성 
2. EC2 생성 (instance: t2.micro/AMI: Ubuntu/Role: EC2 + S3)
3. EC2 보안그룹 설정(인바운드 규칙: 80(web),22(ssh),5000(flask)포트만 추가/아웃바운드 규칙: 모든 트래픽)
4. 탄력적 IP 생성 후 EC2와 연결 
5. SSH를 통한 EC2 서버 접속
```bash
ssh -i "testKey.pem" ubuntu@ec2-3-37-190-174.ap-northeast-2.compute.amazonaws.com
```
6. git clone 
7.    

### AWS의 인터넷 설정 과정
1. VPC 생성 
2. VPC 안에 프라이빗 서브넷 생성
3. 인터넷 게이트웨이 생성
4. 퍼블릭 서브넷 생성(라우팅 테이블에 2와 3을 연결)
  (ACL의 경우 여러 서브넷 접근제한 가능)
