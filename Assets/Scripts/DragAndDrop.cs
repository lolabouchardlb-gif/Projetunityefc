using UnityEngine; 
using UnityEngine.InputSystem; 


public class DragAndDrop : MonoBehaviour 
{
    //créer un décalage entre la position de l’objet et la position du clic pour ne pas "téléporter" l'objet.
    private Vector3 _offset; 
    //de base on ne déplace pas l'object
    private bool _isDragging; 
    //on lui donne son ID via l'inspecteur
    [SerializeField] private string _objectId;
    // Propriété en lecture seule vers l'ID.
    public string ObjectId => _objectId;
    // Position initiale pour pouvoir reset l'objet.
    private Vector3 _startPosition; 

    private void Awake() 
    {
        // On sauvegarde la position initiale.
        _startPosition = transform.position; 
    }
    // Fonction qui gère le drag.
    private void Update() 
    {
        //si le script est désactivé on bloque le dépacement donc on sort de la fonction
        if (!enabled) 
        {
            return; 
        } 
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
                // On tente de démarrer le drag.
                CheckStartDrag(worldPos); 
            }
            //si on maintient le clique gauche l'object suit la sourie
            else if (Mouse.current.leftButton.isPressed && _isDragging) 
            {
                // On suit la souris en conservant l'offset.
                transform.position = worldPos + _offset; 
            }
            //quand on relache le clique gauche l'object arrête son déplacement
            else if (Mouse.current.leftButton.wasReleasedThisFrame) 
            {
                // On arrête le drag.
                _isDragging = false; 
            }
        }
    }

    // Donction qui regarde si on a cliquer sur l'object
    private void CheckStartDrag(Vector3 pos) 
    {
        //on recherche un collider à ce point.
        Collider2D hit = Physics2D.OverlapPoint(pos);  
        //on vérifie qu’il y a un objet et que c’est celui-ci
        if (hit != null && hit.transform == transform) 
        {
            //si c'est le bon object on commence le drag et on calcule le décalage
            _isDragging = true;
            // On calcule l'offset pour garder la même position relative.
            _offset = transform.position - pos; 
        }
    }

    // Fonction qui reset le mini jeu.
    public void ResetMiniGame() 
    {
        // On arrête tout drag.
        _isDragging = false;
        // On reset l'offset.
        _offset = Vector3.zero;
        // On replace l'objet à son point de départ.
        transform.position = _startPosition;
        // On réactive le script.
        enabled = true; 
    }
}
