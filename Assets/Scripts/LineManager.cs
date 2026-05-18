using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.InputSystem; 

public class LineManager : MonoBehaviour 
{
    // On fait un singleton pour accéder facilement au manager.
    public static LineManager Instance { get; private set; } 

    // Prefab de ligne(LineRenderer) à instancier.
    [SerializeField] private LineRenderer _linePrefab;

    // Ligne en cours de dessin.
    private LineRenderer _currentLine;
    // Node de départ sélectionné.
    private Node _startNode; 

    // liste de toutes les connexions
    private readonly List<(Node, Node, LineClick)> _connections = new List<(Node, Node, LineClick)>(); 
    //permet de bloquer le jeu
    private bool _canInteract = true; 

    private void Awake() 
    {
        //on initialise le Singleton
        Instance = this; 
    }

    // Méthode appelé quand l'objet est détruit.
    private void OnDestroy() 
    {
        // Si c'est bien l'instance enregistrée
        if (Instance == this) 
        {
            // On libère le singleton.
            Instance = null; 
        }
    }

    // Méthode qui met à jour la ligne "en cours" pour suivre la souris.
    private void Update() 
    {
        //si une ligne est en train d’être dessinée
        if (_currentLine != null) 
        {
            // Si la souris existe.
            if (Mouse.current != null) 
            {
                // On récupère la position de la souris dans le monde
                // Position écran de la sourie.
                Vector2 mousePos = Mouse.current.position.ReadValue();
                // On convertie la position écran en position monde.
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                // On force z=0 pour être sur le plan 2D.
                worldPos.z = 0;

                // on met à jour la fin de la ligne 
                // Point 1 = extrémité qui suit la souris.
                _currentLine.SetPosition(1, worldPos); 
            }
        }
    }

    // fonction appelée quand on clique sur un node
    public void SelectNode(Node node) 
    {
        //Bloquer les interactions
        if (!_canInteract) 
        {
            return; 
        }
        // Au premier clic, on choisit le point de départ.
        if (_startNode == null) 
        {
            // On crée une nouvelle ligne en mémorisant le node de départ.
            _startNode = node;
            // On instancie une ligne enfant du LineManager.
            _currentLine = Instantiate(_linePrefab, transform); 

            // ligne avec 2 points
            _currentLine.positionCount = 2;
            // On démarre la ligne au node 0 qui est le node de départ.
            _currentLine.SetPosition(0, node.transform.position);
            // Le node 1 est créé au même endroit de façon temporaire.
            _currentLine.SetPosition(1, node.transform.position); 
        }

        //un node est déjà sélectionné
        else
        {
            //on fixe la fin
            _currentLine.SetPosition(1, node.transform.position); 

            // on rend la ligne cliquable en ajoutant le script de clic à la ligne
            LineClick lineClick = _currentLine.gameObject.AddComponent<LineClick>();
            // On stocke le node de départ.
            lineClick.NodeA = _startNode;
            // On stocke le node d'arrivée.
            lineClick.NodeB = node; 

            //on enregistre la connexion
            CheckConnection(_startNode, node, lineClick); 

            //on reset pour être prêt pour une nouvelle ligne
            _startNode = null;
            // Plus de ligne "en cours".
            _currentLine = null; 
        }
    }

    //Méthode qui vérifie et enregistre la connexion
    private void CheckConnection(Node a, Node b, LineClick line) 
    {
        // On empêche une connection au même node
        if (a == b) 
        {
            Debug.Log("Connexion invalide (même node)");
            return;
        }

        //on empêche les doublons en vérifiant si la connection existe déjà en parcourant toutes les connexions existantes.
        foreach (var pair in _connections) 
        {
            if ((pair.Item1 == a && pair.Item2 == b) || (pair.Item1 == b && pair.Item2 == a))
            {
                Debug.Log("Connexion déjà faite");
                return;
            }
        }

        //on ajoute la connection
        _connections.Add((a, b, line)); 

        Debug.Log("Connexion : " + a.NodeId + " → " + b.NodeId); 
    }

    //fonction appelée quand on clique sur une ligne pour retirer une connexion.
    public void RemoveConnection(LineClick line) 
    {
        // On bloque les interactions en sortant de la fonction
        if (!_canInteract) 
        {
            return; 
        } 

        //on compare avec la ligne cliquée
        for (int i = 0; i < _connections.Count; i++) 
        {
            if (_connections[i].Item3 == line)
            {
                //on supprime la connection
                _connections.RemoveAt(i);
                break;
            }
        }
        //on détruit le gameObject de la ligne
        Destroy(line.gameObject); 
    }

    // Méthode qui indique si on a atteint le nombre de connexions requis.
    public bool AllConnected(int totalConnectionsNeeded) 
    {
        //on vérifie si le joueur a fait assez de connexions
        return _connections.Count >= totalConnectionsNeeded; 
    }

    //on calcule les bonnes / mauvaises réponses
    public void GetResults(out int correct, out int wrong) 
    {
        // Compteur de bonnes réponses.
        correct = 0;
        // Compteur de mauvaises réponses.
        wrong = 0;
        // Pour chaque connexion
        foreach (var pair in _connections) 
        {
            //on compare les nodes
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

    //Fonction pour activer / désactiver le jeu
    public void SetInteraction(bool state) 
    {
        // On met à jour le state.
        _canInteract = state; 
    }

    //fonction pour supprimer toutes les connections
    public void ResetConnections() 
    {
        // Si une ligne temporaire existe
        if (_currentLine != null) 
        {
            // On la détruit.
            Destroy(_currentLine.gameObject);
            // Et on clear la référence.
            _currentLine = null; 
        }

        // Aucun node de départ sélectionné.
        _startNode = null;

        // Chaque enfant du LineManager est une ligne instanciée.
        foreach (Transform child in transform) 
        {
            // On détruit chaque ligne.
            Destroy(child.gameObject); 
        }

        // On vide la liste de connexions.
        _connections.Clear(); 
    }

    // Fonction pour reset le mini-jeu.
    public void ResetMiniGame() 
    {
        // On ré-autorise les interactions.
        _canInteract = true;
        // On supprime toutes les connexions.
        ResetConnections(); 
    }
}
