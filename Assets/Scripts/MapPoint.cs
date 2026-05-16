using UnityEngine;

public class MapPoint : MonoBehaviour

{

    public string title;

    [TextArea] public string description;

    public string sceneName;

    public MapPanelController panelController;

    public void ClickPoint()

    {

        Debug.Log("Point cliqué");

        if (panelController == null)

        {

            Debug.LogError("PanelController pas assigné");

            return;

        }

        panelController.Show(title, description, sceneName);

    }

}
