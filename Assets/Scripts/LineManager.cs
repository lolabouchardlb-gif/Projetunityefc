using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LineManager : MonoBehaviour
{
    public static LineManager Instance;

    public LineRenderer linePrefab;

    private LineRenderer currentLine;
    private Node startNode;

    // liste de toutes les connexions
    private List<(Node, Node, LineClick)> connections = new List<(Node, Node, LineClick)>();
    //permet de bloquer le jeu
    private bool canInteract = true;

    void Awake()
    {
        //on initialise le Singleton
        Instance = this;
    }

    void Update()
    {
        //si une ligne est en train d’être dessinée
        if (currentLine != null)
        {
            if (Mouse.current != null)
            {
                //récupère la position de la souris dans le monde
                Vector2 mousePos = Mouse.current.position.ReadValue();
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                worldPos.z = 0;

                // on met à jour la fin de la ligne
                currentLine.SetPosition(1, worldPos);
            }
        }
    }

    // fonction appelée quand on clique sur un node
    public void SelectNode(Node node)
    {
        //Bloquer les interactions
        if (!canInteract) return;
        //aucun node sélectionné
        if (startNode == null)
        {
            //on crée une nouvelle ligne
            startNode = node;
            currentLine = Instantiate(linePrefab, transform);

            // ligne avec 2 points
            currentLine.positionCount = 2;
            //démarre la ligne au node
            currentLine.SetPosition(0, node.transform.position);
            currentLine.SetPosition(1, node.transform.position);
        }
        //un node est déjà sélectionné
        else
        {
            //on fixe la fin
            currentLine.SetPosition(1, node.transform.position);

            // on rend la ligne cliquable, on ajoute le script de clic à la ligne
            LineClick lineClick = currentLine.gameObject.AddComponent<LineClick>();
            lineClick.nodeA = startNode;
            lineClick.nodeB = node;

            //on enregistre la connexion
            CheckConnection(startNode, node, lineClick);

            //on reset = prêt pour une nouvelle ligne
            startNode = null;
            currentLine = null;
        }
    }

    //on vérifie et enregistre la connexion
    void CheckConnection(Node a, Node b, LineClick line)
    {
        //empêcher une connection au même node
        if (a == b)
        {
            Debug.Log("Connexion invalide (même node)");
            return;
        }

        //on empêche les doublons en vérifiant si la connection existe déjà
        foreach (var pair in connections)
        {
            if ((pair.Item1 == a && pair.Item2 == b) ||
                (pair.Item1 == b && pair.Item2 == a))
            {
                Debug.Log("Connexion déjà faite");
                return;
            }
        }

        //on ajoute la connection
        connections.Add((a, b, line));

        Debug.Log("Connexion : " + a.nodeID + " → " + b.nodeID);
    }

    //fonction appelée quand on clique sur une ligne
    public void RemoveConnection(LineClick line)
    {
        //Bloquer les interactions
        if (!canInteract) return;

        //on compare avec la ligne cliquée
        for (int i = 0; i < connections.Count; i++)
        {
            if (connections[i].Item3 == line)
            {
                //on supprime la connection
                connections.RemoveAt(i);
                break;
            }
        }
        //on détruit le gameObject
        Destroy(line.gameObject);
    }

    public bool AllConnected(int totalConnectionsNeeded)
    {
        //on vérifie si le joueur a fait assez de connexions
        return connections.Count >= totalConnectionsNeeded;
    }

    //on calcule les bonnes / mauvaises réponses
    public void GetResults(out int correct, out int wrong)
    {
        correct = 0;
        wrong = 0;

        foreach (var pair in connections)
        {
            //on compare les nodes
            if (pair.Item1.matchID == pair.Item2.matchID)
                correct++;
            else
                wrong++;
        }
    }

    //active / désactive le jeu
    public void SetInteraction(bool state)
    {
        canInteract = state;
    }

    //fonction pour supprimer toutes les connections
    public void ResetConnections()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        connections.Clear();
    }
}