# Platypus
 서울과학기술대학교 컴퓨터공학과 김다은, 오성혁의 캡스톤디자인
 
# 목차
- [게임 컨셉](https://github.com/seong0929/Platypus/edit/hotfix-seong0929/README.md#%EA%B2%8C%EC%9E%84-%EC%BB%A8%EC%85%89)
  - [게임 시스템](https://github.com/seong0929/Platypus/edit/hotfix-seong0929/README.md#%EA%B2%8C%EC%9E%84-%EC%8B%9C%EC%8A%A4%ED%85%9C)
  - [플레이 방법](https://github.com/seong0929/Platypus/edit/hotfix-seong0929/README.md#%ED%94%8C%EB%A0%88%EC%9D%B4-%EB%B0%A9%EB%B2%95)
- [진행 과정](https://github.com/seong0929/Platypus/edit/hotfix-seong0929/README.md#%EC%A7%84%ED%96%89-%EA%B3%BC%EC%A0%95)
- [기타](https://github.com/seong0929/Platypus/edit/hotfix-seong0929/README.md#%EA%B8%B0%ED%83%80)
  - [역할](https://github.com/seong0929/Platypus/edit/hotfix-seong0929/README.md#%EC%97%AD%ED%95%A0)
  - [개발 편의를 위한 그림](https://github.com/seong0929/Platypus/edit/hotfix-seong0929/README.md#%EA%B0%9C%EB%B0%9C-%ED%8E%B8%EC%9D%98%EB%A5%BC-%EC%9C%84%ED%95%9C-%EA%B7%B8%EB%A6%BC)
  - [사용한 툴](https://github.com/seong0929/Platypus/edit/hotfix-seong0929/README.md#%EC%82%AC%EC%9A%A9%ED%95%9C-%ED%88%B4)

# 게임 컨셉
- 장르: 육성 전략 시뮬레이션 게임
- 배경: “소환의 별의 축복을 받고 태어난 fluffonia행성. '마법의 돌'을 바치면 소환수는 소환사의 부름에 응한다. 팀으로 소환수들을 다뤄 전투하는 스포츠 'Platypus'감독이 되어 성공적인 스포츠 팀을 만들자!”
  - 소환이 가능한 세계에 소환수를 활용한 스포츠의 감독이 되어 최고의 스포츠 팀을 만들자.
  - 소환수는 '마법의 돌'을 먹고 산다. 그들을 많이 소환할수록 요구하는 '마법의 돌'이 높아지거나 그들의 스펙이 감소하게 된다. 매 판 변화하는 그들의 스팩과 우리의 자본을 가지고 효율적인 분석을 통해 게임을 승리해보자.

## 게임 시스템
- 팀: 유능한 소환사를 모으고, 좋은 스폰서에게 후원을 받자
  - 영입: 어느 소환사가 우리 팀에 잘 맞는 지 생각해보자.
  - 자본: 스폰서, 경기 등 여러 방식으로 돈을 모아 운영하자.
  - 리그: 훌륭한 경기를 보여주며 더 높은 리그로 올라가자.
- 경기: 경기에 출전할 소환사를 고르고, 각 팀에서 밴픽을 진행한 후, 전략을 짜 경기를 진행하자. 경기는 3판 2선승!
  - 소환사: 경기 시작 전 출전할 소환사를 결정하자. 어느 소환수는 해당 소환사를 좋아할 지도 싫어할 지도 파악하자.
  - 밴픽: 상대팀의 전략을 방해하고 우리팀의 유효한 전략을 찾자.
  - 전략: 우리 소환수는 어느 타이밍에 궁극기를 쓸지, 어떻게 행동할 지 전략을 세워보자.
  - 소환수: 소환수는 각기 다른 환경에서 경기를 치르며 소환수가 좋아하는 필드가 존대한다. 또한, 소환수는 소환 시 '마법의 돌'을 요구하니 주의하자.

## 플레이 방법
- 추후 추가 예정

# 진행 과정
- 2023.3.2 ~ ...

|2023||
|--|--|
|3.2 ~ 3.23|아이디어 회의 및 기획(To do List)|
|3.7 ~ 3.23|초기 캐릭터 디자인, 기본 기능 개발(설정, 씬 이동 등)|
|3.7 ~ 4.19|Behaviour Tree 공부(자동 전투)|
|4.20 ~ 5.23|초기 캐릭터 Senor Zorro 완성|
|4.20 ~ 4.28|게임 관리 개발|
|5.23 ~ 5.25|선수 영입 및 코치 기능 개발|
|6.3 ~ 6.22|외주 후보 선정|
|6.7 ~ 8.16|밴픽 기능 개발|
|6.7 ~ 9.3|Spit Glider 개발|
|6.7 ~ 9.3|리팩토링|
|9.22 ~ 10.27|버그 수정|

# 기타
## 역할
- 김다은: 씬 제작, 캐릭터 디자인, UI/UX 디자인
- 오성혁: 게임 설정, 자동 전투, 캐릭터 코딩

## 개발 편의를 위한 그림
- [AI](https://miro.com/app/board/uXjVMYcfjd0=/)
  - ![BT](https://github.com/seong0929/Platypus/assets/50050003/5b245236-5b35-4552-9a37-4dea3c73a9a8)

## 사용한 툴
- Unity
- Visual Studio
