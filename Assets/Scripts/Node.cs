using UnityEngine; // Base Unity (MonoBehaviour, SerializeField, Debug, etc.).
using UnityEngine.InputSystem; // Nouveau Input System (Mouse, etc.).

public class Node : MonoBehaviour // Représente un "node" cliquable pour le mini-jeu de liaisons.
{
    [SerializeField] private int _nodeId; // Identifiant du node (utile pour debug/affichage).
    [SerializeField] private string _matchId; // Identifiant de "paire" (2 nodes avec même MatchId = bonne connexion).

    public int NodeId => _nodeId; // Propriété en lecture seule vers _nodeId.
    public string MatchId => _matchId; // Propriété en lecture seule vers _matchId.

    private void Update() // Vérifie les clics à chaque frame.
    {
        if (Mouse.current == null) return; // Si pas de souris détectée, on ne fait rien.

        //On détecte le clique gauche de la sourie
        if (Mouse.current.leftButton.wasPressedThisFrame) // True seulement la frame où on appuie.
        {
            //on prend la position écran de la sourie
            Vector2 mousePos = Mouse.current.position.ReadValue(); // Position écran (pixels).
            //puis on la converit en position dans la scène Unity
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos); // Conversion en coordonnées "monde".

            //On ignore les lignes et UI
            RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero); // Raycast 2D à la position du clic.
            
            //on boucle sur les objects touché par le raycast
            foreach (var hit in hits) // Vérifie chaque collider touché.
            {
                //on regarde si l'object touché est un node
                if (hit.collider != null && hit.collider.gameObject == gameObject) // Si c'est bien ce node...
                {
                    Debug.Log("Node cliqué : " + NodeId); // Log de debug.
                    //on prévient le lineManager que l'on a cliqué sur ce node
                    LineManager.Instance.SelectNode(this); // Délègue au LineManager la sélection/connexion.
                    //on termine la boucle
                    return; // Stoppe Update (on a déjà traité le clic).
                }
            }
        }
    }
}
