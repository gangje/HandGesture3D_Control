using UnityEngine;

public class GestureScaleController : MonoBehaviour
{
    public GestureReceiver receiver;

    public float scaleSensitivity = 5f;
    public float smoothSpeed = 12f;

    public float minScale = 0.3f;
    public float maxScale = 3f;

    bool isScaling = false;

    float startDistance;
    Vector3 startScale;

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

        if (data.gesture != "scale")
        {
            isScaling = false;
            if (data.gesture == "none" || data.gesture == "open")
                SetColor(originalColor);
            return;
        }

        // Scale 모드: 노랑
        SetColor(Color.yellow);

        if (!isScaling)
        {
            isScaling = true;
            startDistance = data.distance;
            startScale = transform.localScale;
        }

        float delta = data.distance - startDistance;

        float scaleFactor = 1f + delta * scaleSensitivity;
        scaleFactor = Mathf.Clamp(scaleFactor, minScale, maxScale);

        Vector3 target = startScale * scaleFactor;

        transform.localScale = Vector3.Lerp(
            transform.localScale,
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