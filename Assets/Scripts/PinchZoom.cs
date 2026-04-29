using UnityEngine;
public class PinchZoom : MonoBehaviour
{
    public float zoomSpeed = 0.1f;
    public float minZoom = 2f;
    public float maxZoom = 10f;
    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            float prevDistance = (touch0PrevPos - touch1PrevPos).magnitude;
            float currentDistance = (touch0.position - touch1.position).magnitude;
            float delta = currentDistance - prevDistance;
            Camera.main.orthographicSize -= delta * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(
                Camera.main.orthographicSize,
                minZoom,
                maxZoom
            );
        }
    }
}