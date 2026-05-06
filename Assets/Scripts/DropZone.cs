using UnityEngine;

public class DropZone : MonoBehaviour
{
    //liste des objects valide dans la drop zone
    public string[] validObjectIDs;

    //object présent dans la zone
    private DragAndDrop _currentObject;

    //fonction appeller quand un object rentre dans la drop zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //on récupère l'object
        DragAndDrop obj = collision.GetComponent<DragAndDrop>();

        //si il a ule script on enregistre l'object dans la drop zone
        if (obj != null)
        {
            _currentObject = obj;
        }
    }

    //fonction appeller quand un object sort de la drop zone
    private void OnTriggerExit2D(Collider2D collision)
    {
        //on récupère l'object
        DragAndDrop obj = collision.GetComponent<DragAndDrop>();

        //on le supprime de la zone
        if (obj != null && obj == _currentObject)
        {
            _currentObject = null;
        }
    }

    //fonction appeller pour savoir si la drop zone a les bon objects
    public bool IsCorrect()
    {
        //si la drop zone n'a pas d'object on retourne faux
        if (_currentObject == null) return false;
        //on parcourt tout les objects autoriser de la drop zone
        foreach (string id in validObjectIDs)
        {
            //si l'object a le bon ID on le valide
            if (_currentObject.objectID == id)
                return true;
        }
        //sinon on ne valide pas l'object
        return false;
    }

    //fonction pour savoir si la drop zone a un object
    public bool IsFilled()
    {
        return _currentObject != null;
    }
}