using UnityEngine;

public class GestureRotateController : MonoBehaviour
{
    public GestureReceiver receiver;

    public float rotationSensitivity = 200f;
    public float smoothSpeed = 12f;
    public float rotateDeadZone = 0.01f;

    bool isRotating = false;

    Vector2 openHandStartPos;
    Quaternion objectStartRotation;

    Renderer objectRenderer;
    Color originalColor;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
            originalColor = objectRenderer.material.color;
    }

    void Update()
    {
        if (receiver == null || receiver.LatestData == null) return;

        var data = receiver.LatestData;

        // rotate 제스처가 아니면 회전 종료
        if (data.gesture != "rotate")
        {
            isRotating = false;
            if (data.gesture == "none" || data.gesture == "open")
                SetColor(originalColor);
            return;
        }

        // Rotate 모드: 초록색
        SetColor(Color.green);

        // 펼친 손의 현재 좌표
        Vector2 currentOpenPos = new Vector2(data.x, data.y);

        // 회전 시작 시점 저장
        if (!isRotating)
        {
            isRotating = true;
            openHandStartPos = currentOpenPos;
            objectStartRotation = transform.rotation;
        }

        // 시작 위치로부터 손이 얼마나 움직였는지
        Vector2 delta = currentOpenPos - openHandStartPos;

        // 너무 작은 움직임은 무시
        if (Mathf.Abs(delta.x) < rotateDeadZone && Mathf.Abs(delta.y) < rotateDeadZone)
            return;

        Quaternion targetRotation;

        // 좌우 움직임이 더 크면: 전역 Y축 기준 회전
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            float yaw = -delta.x * rotationSensitivity;

            targetRotation =
                Quaternion.AngleAxis(yaw, Vector3.up) *
                objectStartRotation;
        }
        // 위아래 움직임이 더 크면: 전역 X축 기준 회전
        else
        {
            float pitch = -delta.y * rotationSensitivity;

            targetRotation =
                Quaternion.AngleAxis(pitch, Vector3.right) *
                objectStartRotation;
        }

        // 부드럽게 회전
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * smoothSpeed
        );
    }

    void SetColor(Color color)
    {
        if (objectRenderer != null)
            objectRenderer.material.color = color;
    }
}