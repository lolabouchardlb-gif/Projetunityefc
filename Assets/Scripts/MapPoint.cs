using UnityEngine; 
using UnityEngine.EventSystems; 

public class MapPoint : MonoBehaviour, IPointerClickHandler 
{
    [Header("UI")]
    // Titre affiché dans le panel.
    [SerializeField] private string _title; 

    [TextArea]
    // Description affichée dans le panel.
    [SerializeField] private string _description; 

    [Header("Scene")]
    // Nom de la scène à charger.
    [SerializeField] private string _sceneName; 

    [Header("References")]
    // Référence vers le contrôleur du panel.
    [SerializeField] private MapPanelController _panelController;

    // Méthode publique appelée lors d'un clic sur le point.
    public void ClickPoint() 
    {
        // On demande au panel de s'afficher avec les infos.
        _panelController.Show(_title, _description, _sceneName); 
    }

    // Méthode appelé automatiquement par l'EventSystem lors d'un clic.
    public void OnPointerClick(PointerEventData eventData) 
    {
        // On réutilise la même logique que pour un clic.
        ClickPoint(); 
    }

    // Méthode appelé par Unity si l'objet reçoit un clic via collider.
    private void OnMouseDown() 
    {
        // On déclenche l'affichage du panel.
        ClickPoint(); 
    }
}
