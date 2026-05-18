using UnityEngine; 

public class DropZone : MonoBehaviour 
{
    //liste des objects valide dans la drop zone
    [SerializeField] private string[] _validObjectIds; 

    //object présent dans la zone
    private DragAndDrop _currentObject; 

    //fonction appeller quand un object rentre dans la drop zone
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // On récupère le script DragAndDrop sur l'objet entrant.
        DragAndDrop obj = collision.GetComponent<DragAndDrop>(); 

        //si il a le script on enregistre l'object dans la drop zone
        if (obj != null) 
        {
            Debug.Log("Entré : " + obj.name);
            // On stocke l'objet présent dans la zone.
            _currentObject = obj; 
        }
    }

    //fonction appeller quand un object sort de la drop zone
    private void OnTriggerExit2D(Collider2D collision) 
    {
        // On récupère le script DragAndDrop de l'objet sortant.
        DragAndDrop obj = collision.GetComponent<DragAndDrop>();

        // Si l'objet sortant est celui enregistré.
        
        if (obj != null && obj == _currentObject) 
        {
            Debug.Log("Sorti : " + obj.name); 
            //on le supprime de la zone
            _currentObject = null; 
        }
    }

    //fonction appeller pour savoir si la drop zone a les bon objects
    public bool IsCorrect() 
    {
        //si la drop zone n'a pas d'object on retourne faux
        if (_currentObject == null) 
        {
            return false; 
        } 
        //on parcourt tout les objects autoriser de la drop zone
        foreach (string id in _validObjectIds) // Vérifie chaque ID autorisé...
        {
            //si l'object a le bon ID on le valide
            if (_currentObject.ObjectId == id) 
            {
                return true; 
            } 
                
        }
        //sinon on ne valide pas l'object
        return false; 
    }

    //fonction pour savoir si la drop zone a un object
    public bool IsFilled() 
    {
        // Retourne True si on a une référence d'objet.
        return _currentObject != null; 
    }

    // Fonction qui reset la drop zone.
    public void ResetMiniGame() 
    {
        // On vide la zone.
        _currentObject = null; 
    }
}
