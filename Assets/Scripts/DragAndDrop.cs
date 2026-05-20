using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
public class DragAndDrop : MonoBehaviour
{
    private Vector3 _offset;
    private bool _isDragging;
    [SerializeField] private string _objectId;
    public string ObjectId => _objectId;
    public DropZone CurrentDropZone { get; set; }
    private Vector3 _startPosition;
    private void Awake()
    {
        _startPosition = transform.position;
        EnhancedTouchSupport.Enable();
    }
    private void Update()
    {
        if (!enabled)
        {
            return;
        }
        if (Touchscreen.current != null )
        {
            var touch = Touchscreen.current.primaryTouch;
            Vector2 touchPos = touch.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);
            worldPos.z = 0;
            if (touch.press.wasPressedThisFrame)
            {
                CheckStartDrag(worldPos);
            }
            else if (touch.press.isPressed && _isDragging)
            {
                transform.position = worldPos + _offset;
            }
            else if (touch.press.wasReleasedThisFrame)
            {
                _isDragging = false;
            }
            return;
        }
        /*
        if (Mouse.current != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                CheckStartDrag(worldPos);
            }
            else if (Mouse.current.leftButton.isPressed && _isDragging)
            {
                transform.position = worldPos + _offset;
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                _isDragging = false;
            }
        }
        */
    }
    private void CheckStartDrag(Vector3 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos);
        if (hit != null && hit.transform == transform)
        {
            _isDragging = true;
            _offset = transform.position - pos;
        }
    }
}
