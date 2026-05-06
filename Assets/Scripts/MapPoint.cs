using UnityEngine;
public class MapPoint : MonoBehaviour
{
    public string title;
    [TextArea] public string description;
    public Object scene; // glissé dans l'inspector
    public MapPanelController panelController;
    public void ClickPoint()
    {
        panelController.Show(title, description, scene.name);
    }
}