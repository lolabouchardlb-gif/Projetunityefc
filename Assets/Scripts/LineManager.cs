using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
public class LineManager : MonoBehaviour
{
    public static LineManager Instance { get; private set; }
    [SerializeField] private LineRenderer _linePrefab;
    private LineRenderer _currentLine;
    private Node _startNode;
    private readonly List<(Node, Node, LineClick)> _connections = new List<(Node, Node, LineClick)>();
    private bool _canInteract = true;
    private void Awake()
    {
        Instance = this;
        EnhancedTouchSupport.Enable();
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    private void Update()
    {
        if (_currentLine != null)
        {
            Vector2 screenPos;
            bool hasInput = false;
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
                hasInput = true;
            }
            else if (Mouse.current != null)
            {
                screenPos = Mouse.current.position.ReadValue();
                hasInput = true;
            }
            else
            {
                screenPos = Vector2.zero;
            }
            if (hasInput)
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                worldPos.z = 0;
                _currentLine.SetPosition(1, worldPos);
            }
        }
    }
    public void SelectNode(Node node)
    {
        if (!_canInteract)
        {
            return;
        }
        if (_startNode == null)
        {
            _startNode = node;
            _currentLine = Instantiate(_linePrefab, transform);
            _currentLine.positionCount = 2;
            _currentLine.SetPosition(0, node.transform.position);
            _currentLine.SetPosition(1, node.transform.position);
        }
        else
        {
            _currentLine.SetPosition(1, node.transform.position);
            LineClick lineClick = _currentLine.gameObject.AddComponent<LineClick>();
            lineClick.NodeA = _startNode;
            lineClick.NodeB = node;
            CheckConnection(_startNode, node, lineClick);
            _startNode = null;
            _currentLine = null;
        }
    }
    private void CheckConnection(Node a, Node b, LineClick line)
    {
        if (a == b)
        {
            Debug.Log("Connexion invalide");
            return;
        }
        foreach (var pair in _connections)
        {
            if ((pair.Item1 == a && pair.Item2 == b) ||
                (pair.Item1 == b && pair.Item2 == a))
            {
                Debug.Log("Connexion déjà faite");
                return;
            }
        }
        _connections.Add((a, b, line));
        Debug.Log("Connexion : " + a.NodeId + " → " + b.NodeId);
    }
    public void RemoveConnection(LineClick line)
    {
        if (!_canInteract)
        {
            return;
        }
        for (int i = 0; i < _connections.Count; i++)
        {
            if (_connections[i].Item3 == line)
            {
                _connections.RemoveAt(i);
                break;
            }
        }
        Destroy(line.gameObject);
    }
    public bool AllConnected(int totalConnectionsNeeded)
    {
        return _connections.Count >= totalConnectionsNeeded;
    }
    public void GetResults(out int correct, out int wrong)
    {
        correct = 0;
        wrong = 0;
        foreach (var pair in _connections)
        {
            if (pair.Item1.MatchId == pair.Item2.MatchId)
            {
                correct++;
            }
            else
            {
                wrong++;
            }
        }
    }
    public void SetInteraction(bool state)
    {
        _canInteract = state;
    }
    public void ResetConnections()
    {
        if (_currentLine != null)
        {
            Destroy(_currentLine.gameObject);
            _currentLine = null;
        }
        _startNode = null;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        _connections.Clear();
    }
    public void ResetMiniGame()
    {
        _canInteract = true;
        ResetConnections();
    }
}