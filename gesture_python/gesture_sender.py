import cv2
import json
import socket
import math
import mediapipe as mp

UDP_IP = "127.0.0.1"
UDP_PORT = 5005

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

mp_hands = mp.solutions.hands
mp_draw = mp.solutions.drawing_utils


def is_fist(hand_landmarks):
    # 손가락 끝 landmark
    tips = [8, 12, 16, 20]
    # 손가락 중간 마디 landmark
    pips = [6, 10, 14, 18]

    folded = 0

    for tip, pip in zip(tips, pips):
        if hand_landmarks.landmark[tip].y > hand_landmarks.landmark[pip].y:
            folded += 1

    return folded >= 3


def get_fist_center(hand_landmarks):
    """주먹의 중심점을 계산"""
    # 모든 손가락 끝의 좌표
    finger_tips = [4, 8, 12, 16, 20]  # 엄지, 검지, 중지, 약지, 소지
    
    x_sum = 0
    y_sum = 0
    
    for tip in finger_tips:
        x_sum += hand_landmarks.landmark[tip].x
        y_sum += hand_landmarks.landmark[tip].y
    
    center_x = x_sum / len(finger_tips)
    center_y = y_sum / len(finger_tips)
    
    return center_x, center_y


cap = cv2.VideoCapture(0)

with mp_hands.Hands(
    max_num_hands=2,
    min_detection_confidence=0.7,
    min_tracking_confidence=0.7
) as hands:

    while True:
        ret, frame = cap.read()
        if not ret:
            break

        frame = cv2.flip(frame, 1)
        rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

        results = hands.process(rgb)

        gesture = "none"
        x = 0.5
        y = 0.5
        x2 = 0.5
        y2 = 0.5

        if results.multi_hand_landmarks:
            # 첫 번째 손
            hand1 = results.multi_hand_landmarks[0]
            mp_draw.draw_landmarks(
                frame,
                hand1,
                mp_hands.HAND_CONNECTIONS
            )
            
            hand1_fist = is_fist(hand1)
            
            # 주먹을 쥐었으면 주먹 중심, 아니면 손목 좌표 사용
            if hand1_fist:
                x, y = get_fist_center(hand1)
            else:
                wrist1 = hand1.landmark[0]
                x = wrist1.x
                y = wrist1.y

            # 거리 기본값
            distance = 0.0

            # 두 번째 손이 있으면
            if len(results.multi_hand_landmarks) > 1:
                hand2 = results.multi_hand_landmarks[1]
                mp_draw.draw_landmarks(
                    frame,
                    hand2,
                    mp_hands.HAND_CONNECTIONS
                )
                
                hand2_fist = is_fist(hand2)
                
                # 주먹을 쥐었으면 주먹 중심, 아니면 손목 좌표 사용
                if hand2_fist:
                    x2, y2 = get_fist_center(hand2)
                else:
                    wrist2 = hand2.landmark[0]
                    x2 = wrist2.x
                    y2 = wrist2.y

                # 두 손의 중심 사이의 거리 계산
                distance = math.sqrt((x2 - x) ** 2 + (y2 - y) ** 2)

                # 두 손 모드: 둘 다 주먹이면 scale(확대축소), 둘 다 펼쳤으면 rotate(회전)
                if hand1_fist and hand2_fist:
                    gesture = "scale"
                    print(f"[GESTURE] Both fists - SCALE")
                elif not hand1_fist and not hand2_fist:
                    gesture = "rotate"
                    print(f"[GESTURE] Both open - ROTATE")
                else:
                    gesture = "none"
                    print(f"[GESTURE] Mixed - NONE (hand1_fist={hand1_fist}, hand2_fist={hand2_fist})")
            else:
                # 한 손 모드: fist/open
                if hand1_fist:
                    gesture = "fist"
                else:
                    gesture = "open"

            data = {
                "gesture": gesture,
                "x": x,
                "y": y,
                "distance": distance,
                "x2": x2,
                "y2": y2
            }

            message = json.dumps(data)
            sock.sendto(message.encode(), (UDP_IP, UDP_PORT))

        cv2.putText(
            frame,
            f"Gesture: {gesture}",
            (30, 50),
            cv2.FONT_HERSHEY_SIMPLEX,
            1,
            (0, 255, 0),
            2
        )

        cv2.imshow("Gesture Sender", frame)

        if cv2.waitKey(1) & 0xFF == 27:
            break

cap.release()
cv2.destroyAllWindows()