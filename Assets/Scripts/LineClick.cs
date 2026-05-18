using UnityEngine; // Base Unity.
using UnityEngine.InputSystem; // Nouveau Input System (Mouse).

public class LineClick : MonoBehaviour // Permet de cliquer sur une ligne pour la supprimer.
{
    //point de départ de d'arrivé de la ligne
    public Node NodeA { get; set; } // Référence vers le node de départ de la connexion.
    public Node NodeB { get; set; } // Référence vers le node d'arrivée de la connexion.

    private void Update() // Vérifie les clics à chaque frame.
    {
        //On vérifie que le LineManager existe
        if (!LineManager.Instance) return; // Si pas de LineManager, on ne fait rien.

        //On détecte le clique gauche de la sourie
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) // Clic gauche (frame du press).
        {
            //on convertie la position de la sourie écran en position monde
            Vector2 mousePos = Mouse.current.position.ReadValue(); // Position écran.
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos); // Conversion en position monde 2D.

            //on envoie un rayon à la position du clic pour récupèrer l’objet touché
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero); // Raycast 2D "point" à la position du clic.

            //si c'est bien cette ligne touché
            if (hit.collider != null && hit.collider.gameObject == gameObject) // Si on a cliqué sur CE GameObject...
            {
                Debug.Log("Ligne cliquée !");
                //On appelle le LineManager pour retirer la connection
                LineManager.Instance.RemoveConnection(this); // Demande au manager de supprimer cette connexion.
            }
        }
    }
}
