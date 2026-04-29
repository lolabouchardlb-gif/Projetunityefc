using UnityEngine;
using TMPro;
public class MapInteraction : MonoBehaviour
{
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;
    public string message;
    void OnMouseDown()
    {
        popupPanel.SetActive(true);
        popupText.text = message;
    }
}