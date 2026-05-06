using UnityEngine;
using UnityEngine.InputSystem;

public class LineClick : MonoBehaviour
{
    //point de départ de d'arrivé de la ligne
    public Node nodeA;
    public Node nodeB;

    void Update()
    {
        //On vérifie que le LineManager existe
        if (!LineManager.Instance) return;

        //On détecte le clique gauche de la sourie
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            //on convertie la position de la sourie écran en position monde
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            //on envoie un rayon à la position du clic pour récupèrer l’objet touché
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            //si c'est bien cette ligne touché
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Ligne cliquée !");
                //On appelle le LineManager pour retirer la connection
                LineManager.Instance.RemoveConnection(this);
            }
        }
    }
}