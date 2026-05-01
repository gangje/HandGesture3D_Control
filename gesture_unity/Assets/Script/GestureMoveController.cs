using UnityEngine;

public class GestureMoveController : MonoBehaviour
{
    public GestureReceiver receiver;

    public float moveScaleX = 10f;
    public float moveScaleY = 10f;

    public float smoothSpeed = 12f;
    public float deadZone = 0.02f;

    bool isHolding = false;

    Vector2 handStartPos;
    Vector3 objectStartPos;

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

        if (data.gesture != "fist")
        {
            isHolding = false;
            return;
        }

        // Move 모드: 빨강
        SetColor(Color.red);

        Vector2 currentHandPos = new Vector2(data.x, data.y);

        if (!isHolding)
        {
            isHolding = true;
            handStartPos = currentHandPos;
            objectStartPos = transform.position;
        }

        Vector2 delta = currentHandPos - handStartPos;

        if (Mathf.Abs(delta.x) < deadZone) delta.x = 0;
        if (Mathf.Abs(delta.y) < deadZone) delta.y = 0;

        Vector3 target = objectStartPos + new Vector3(
            delta.x * moveScaleX,
            -delta.y * moveScaleY,
            0f
        );

        transform.position = Vector3.Lerp(
            transform.position,
            target,
            Time.deltaTime * smoothSpeed
        );
    }

    void SetColor(Color color)
    {
        if (objectRenderer != null)
            objectRenderer.material.color = color;
    }
}