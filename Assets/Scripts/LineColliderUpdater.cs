using UnityEngine; // Base Unity.
//On ajoute automatiquement un LineRenderer et un EdgeCollider2D
[RequireComponent(typeof(LineRenderer))] // Force la présence d'un LineRenderer sur le même GameObject.
[RequireComponent(typeof(EdgeCollider2D))] // Force la présence d'un EdgeCollider2D sur le même GameObject.
public class LineColliderUpdater : MonoBehaviour // Met à jour le collider pour qu'il suive la ligne.
{
    private LineRenderer _line; // Référence au composant LineRenderer.
    private EdgeCollider2D _edge; // Référence au composant EdgeCollider2D.

    private void Awake() // Appelé avant Start: on récupère les composants.
    {
        //on récupère les composants
        _line = GetComponent<LineRenderer>(); // Récupère le LineRenderer sur ce GameObject.
        _edge = GetComponent<EdgeCollider2D>(); // Récupère l'EdgeCollider2D sur ce GameObject.

        //on rend le collider plus épai
        _edge.edgeRadius = 0.2f; // Augmente la "largeur" du collider pour faciliter le clic.
    }

    private void Update() // À chaque frame, recalcule les points du collider.
    {
        //nombre de points de la ligne
        int count = _line.positionCount; // Nombre de points du LineRenderer.

        // on créer un tableau pour stocker les points du collider
        Vector2[] points = new Vector2[count]; // Tableau des points (en local) pour l'EdgeCollider2D.

        //on parcourt tous les points de la ligne
        for (int i = 0; i < count; i++) // Convertit chaque point du LineRenderer.
        {
            //On converti les position car le LineRenderer veux une position en World et l'EdgeCollider2D veux une position en Local
            // position dans la scène
            Vector3 worldPos = _line.GetPosition(i); // Point i en coordonnées monde.
            //position relative à l’objet
            Vector3 localPos = transform.InverseTransformPoint(worldPos); // Convertit le monde -> local du GameObject.

            //on ajoute les points dans le tableau
            points[i] = new Vector2(localPos.x, localPos.y); // Stocke le point en 2D (x,y).
        }
        //on met à jour la forme du collider, il suivra la ligne tracé
        _edge.points = points; // Met à jour les points du collider pour correspondre à la ligne.
    }
}
