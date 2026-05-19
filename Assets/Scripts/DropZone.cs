using UnityEngine;

public class DropZone : MonoBehaviour

{

    [SerializeField] private string[] _validObjectIds;

    private DragAndDrop _currentObject;

    private void OnTriggerEnter2D(Collider2D collision)

    {

        DragAndDrop obj = collision.GetComponent<DragAndDrop>();

        if (obj == null)

        {

            return;

        }

        if (obj.CurrentDropZone != null && obj.CurrentDropZone != this)

        {

            return;

        }

        if (_currentObject != null && _currentObject != obj)

        {

            return;

        }

        _currentObject = obj;

        obj.CurrentDropZone = this;

        Debug.Log("Entré : " + obj.name + " dans " + gameObject.name);

    }

    private void OnTriggerExit2D(Collider2D collision)

    {

        DragAndDrop obj = collision.GetComponent<DragAndDrop>();

        if (obj != null && obj == _currentObject)

        {

            Debug.Log("Sorti : " + obj.name + " de " + gameObject.name);

            _currentObject = null;

            if (obj.CurrentDropZone == this)

            {

                obj.CurrentDropZone = null;

            }

        }

    }

    public bool IsCorrect()

    {

        if (_currentObject == null)

        {

            return false;

        }

        foreach (string id in _validObjectIds)

        {

            if (_currentObject.ObjectId == id)

            {

                return true;

            }

        }

        return false;

    }

    public bool IsFilled()

    {

        return _currentObject != null;

    }

    public void ResetMiniGame()

    {

        if (_currentObject != null && _currentObject.CurrentDropZone == this)

        {

            _currentObject.CurrentDropZone = null;

        }

        _currentObject = null;

    }

}