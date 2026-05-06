using UnityEngine;
//On ajoute automatiquement un LineRenderer et un EdgeCollider2D
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class LineColliderUpdater : MonoBehaviour
{
    private LineRenderer line;
    private EdgeCollider2D edge;

    void Awake()
    {
        //on récupère les composants
        line = GetComponent<LineRenderer>();
        edge = GetComponent<EdgeCollider2D>();

        //on rend le collider plus épai
        edge.edgeRadius = 0.2f; 
    }

    void Update()
    {
        //nombre de points de la ligne
        int count = line.positionCount;

        // on créer un tableau pour stocker les points du collider
        Vector2[] points = new Vector2[count];

        //on parcourt tous les points de la ligne
        for (int i = 0; i < count; i++)
        {
            //On converti les position car le LineRenderer veux une position en World et l'EdgeCollider2D veux une position en Local
            // position dans la scène
            Vector3 worldPos = line.GetPosition(i);
            //position relative à l’objet
            Vector3 localPos = transform.InverseTransformPoint(worldPos);

            //on ajoute les points dans le tableau
            points[i] = new Vector2(localPos.x, localPos.y);
        }
        //on met à jour la forme du collider, il suivra la ligne tracé
        edge.points = points;
    }
}