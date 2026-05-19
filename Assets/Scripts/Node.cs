using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
public class Node : MonoBehaviour
{
    [SerializeField] private int _nodeId;
    [SerializeField] private string _matchId;
    public int NodeId => _nodeId;
    public string MatchId => _matchId;
    private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }
    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            CheckClick(mousePos);
        }
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch.press.wasPressedThisFrame)
            {
                Vector2 touchPos = touch.position.ReadValue();
                CheckClick(touchPos);
            }
        }
    }
    private void CheckClick(Vector2 screenPosition)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);
        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Node cliqué : " + NodeId);
                LineManager.Instance.SelectNode(this);
                return;
            }
        }
    }
}