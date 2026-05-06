using UnityEngine;
using UnityEngine.InputSystem;

public class Node : MonoBehaviour
{
    public int nodeID;
    public string matchID;

    void Update()
    {
        if (Mouse.current == null) return;

        //On détecte le clique gauche de la sourie
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //on prend la position écran de la sourie
            Vector2 mousePos = Mouse.current.position.ReadValue();
            //puis on la converit en position dans la scène Unity
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            //On ignore les lignes et UI
            RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);
            
            //on boucle sur les objects touché par le raycast
            foreach (var hit in hits)
            {
                //on regarde si l'object touché est un node
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Node cliqué : " + nodeID);
                    //on prévient le lineManager que l'on a cliqué sur ce node
                    LineManager.Instance.SelectNode(this);
                    //on termine la boucle
                    return;
                }
            }
        }
    }
}