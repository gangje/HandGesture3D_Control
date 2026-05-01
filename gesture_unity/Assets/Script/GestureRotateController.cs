using UnityEngine;

public class GestureRotateController : MonoBehaviour
{
    public GestureReceiver receiver;

    public float rotateScale = 360f;
    public float smoothSpeed = 12f;

    Quaternion targetRotation;
    float lastAngle = 0f;

    Renderer objectRenderer;
    Color originalColor;

    void Start()
    {
        targetRotation = transform.rotation;
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
            originalColor = objectRenderer.material.color;
    }

    void Update()
    {
        if (receiver == null || receiver.LatestData == null) return;

        var data = receiver.LatestData;

        if (data.gesture != "rotate")
        {
            return;
        }

        // Rotate 모드: 초록
        SetColor(Color.green);

        // 두 손의 좌표로부터 각도 계산
        Vector2 hand1 = new Vector2(data.x, data.y);
        Vector2 hand2 = new Vector2(data.x2, data.y2);

        Vector2 delta = hand2 - hand1;
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        // 각도 차이 계산
        float angleDelta = angle - lastAngle;

        // 180도 이상 차이가 나면 짧은 방향으로 계산
        if (angleDelta > 180f) angleDelta -= 360f;
        if (angleDelta < -180f) angleDelta += 360f;

        // Z축 회전 적용
        targetRotation *= Quaternion.AngleAxis(angleDelta * rotateScale, Vector3.forward);

        // 부드러운 회전
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * smoothSpeed
        );

        lastAngle = angle;
    }

    void SetColor(Color color)
    {
        if (objectRenderer != null)
            objectRenderer.material.color = color;
    }
}
