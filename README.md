# HandGesture3D_Control

This project integrates MediaPipe Hands with Unity to enable real-time interaction with a 3D object using hand gestures, including movement, scaling, and rotation.

## Main Features: Gesture-to-Action Mapping

### 1. One-Hand Fist Gesture: Object Move Mode (Red)

- When a fist is detected, the selected object follows the movement of the hand.

### 2. Two-Hand Fist Gesture: Object Scale Mode (Yellow)

- When both hands are clenched, increasing the distance between the hands scales the object up.
- Decreasing the distance scales the object down.

### 3. One Fist + One Open Hand Gesture: Object Rotation Mode (Green)

- One hand remains clenched while the other is open to activate rotation mode.
- Moving the open hand up and down rotates the object around the horizontal axis (screen-based).
- Moving the open hand left and right rotates the object around the vertical axis (screen-based).

## Tech Stack

- Unity  
- C#  
- Python  
- OpenCV  
- MediaPipe  
- Socket Communication  

## Project Structure

gesture_project/
├─ gesture_unity/
├─ gesture_python/
├─ README.md
└─ .gitignore

# HandGesture3D_Control 설명
MediaPipe Hands와 Unity를 연동하여 손 제스처로 3D 오브젝트를 이동, 크기 조절, 회전할 수 있는 인터랙션 프로젝트입니다.

## 주요 기능: 손 제스처 - 동작 매칭

### 1. 한 손 주먹 제스처: 오브젝트 이동 모드 (빨간색)

- 주먹을 쥐고 움직이면 선택된 오브젝트가 손의 움직임을 따라 이동합니다.

### 2. 양손 주먹 제스처: 오브젝트 확대/축소 모드 (노란색)

- 양손 주먹을 쥔 상태에서 손 사이의 거리를 벌리면 오브젝트가 확대됩니다.
- 손 사이의 거리를 좁히면 오브젝트가 축소됩니다.

### 3. 한 손 주먹 + 한 손 펼침 제스처: 오브젝트 회전 모드 (초록색)

- 한 손은 주먹을 쥐고, 다른 한 손은 펼친 상태에서 회전 모드로 전환합니다.
- 펼친 손을 상하로 움직이면 오브젝트가 화면 기준 수평축을 중심으로 회전합니다.
- 펼친 손을 좌우로 움직이면 오브젝트가 화면 기준 수직축을 중심으로 회전합니다.

## 기술 스택

- Unity
- C#
- Python
- OpenCV
- MediaPipe
- Socket 통신

## 프로젝트 구조

```txt
gesture_project/
├─ gesture_unity/
├─ gesture_python/
├─ README.md
└─ .gitignore


## 실행 방법

1. Python 서버 실행

```bash
cd gesture_python
python main.py


2. Unity 실행
gesture_unity 폴더를 Unity Hub에서 열기
Scene 실행 (Play 버튼 클릭)

※ Python 서버와 Unity는 실행 순서에 관계없이 실행할 수 있으며, 두 프로그램이 모두 실행되어야 정상 동작합니다.