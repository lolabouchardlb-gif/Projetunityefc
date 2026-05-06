using UnityEngine;
using UnityEngine.InputSystem;


public class DragAndDrop : MonoBehaviour
{
    //creer un décalage entre la position de l’objet et la position du clic
    private Vector3 offset;
    //de base on ne déplace pas l'object
    private bool isDragging = false;
    //on lui donne son ID via l'inspecteur
    public string objectID;

    void Update()
    {
        //si le script est désactivé on bloque le dépacement
        if (!enabled) return;
        //on regarde si on est avec la sourie
        if (Mouse.current != null)
        {
            //on récupère sa position et on la convertie en position dans le monde Unity
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            //quand on clique sur le bouton gauche de la sourie on regarde si on a cliqué sur l'object
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                CheckStartDrag(worldPos);
            }
            //si on maintient le clique gauche l'object suit la sourie
            else if (Mouse.current.leftButton.isPressed && isDragging)
            {
                transform.position = worldPos + offset;
            }
            //quand on relache le clique gauche l'object arrête son déplacement
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                isDragging = false;
            }
        }
    }

    //on regarde si on a cliquer sur l'object
    void CheckStartDrag(Vector3 pos)
    {
        //on recherche un collider
        Collider2D hit = Physics2D.OverlapPoint(pos);
        //on vérifie qu’il y a un objet et que c’est celui-ci
        if (hit != null && hit.transform == transform)
        {
            //si c'est le bon object on commence le drag et on calcule le décalage
            isDragging = true;
            offset = transform.position - pos;
        }
    }
}