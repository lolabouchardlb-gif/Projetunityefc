using UnityEngine; 
using TMPro; 

public class GameManager : MonoBehaviour 
{
    // Zones où l'on dépose les objets.
    [SerializeField] private DropZone[] _dropZones; 
    [SerializeField] private GameObject _validateButton;
    // Texte qui affiche le résultat.
    [SerializeField] private TMP_Text _resultText;
    // Bouton de félicitation.
    [SerializeField] private GameObject _returnMiniGameChoise; 
    [SerializeField] private GameObject _retryButton;
    // Fond UI derrière le résultat.
    [SerializeField] private GameObject _backgroundText;
    // Objets que le joueur peut déplacer.
    [SerializeField] private DragAndDrop[] _draggableObjects; 

    // Indique si on peut afficher le bouton valider
    private bool _canValidate; 

    private void Start() 
    {
        // On active la phase de jeu au départ
        SetGameState(true); 

        // On cache tous les éléments UI du jeu
        _validateButton.SetActive(false); 
        _resultText.gameObject.SetActive(false); 
        _returnMiniGameChoise.SetActive(false); 
        _retryButton.SetActive(false); 
        _backgroundText.SetActive(false); 
    }

    // Fonction qui vérifie en continu si toutes les zones sont remplies.
    private void Update() 
    {
        //on suppose que tout est rempli
        bool allFilled = true; 

        //On vérifie toutes les zones de drop
        foreach (DropZone zone in _dropZones) 
        {
            //Si une zone est vide alors on est pas prêt à valider
            if (!zone.IsFilled()) 
            {
                allFilled = false; 
                break; 
            }
        }

        //Si toutes les zones sont remplies
        if (allFilled) 
        {
            //on autorise la validation
            _canValidate = true; 
            // on affiche le bouton
            _validateButton.SetActive(true); 
        }
    }

    // Fonction qui vérifie les réponses et affiche le résultat.
    public void Validate()
    {
        // nombre de bonnes réponses
        int correct = 0; 
        // nombre de mauvaises réponses
        int wrong = 0;

        // On vérifie chaque zone
        foreach (DropZone zone in _dropZones) 
        {
            // si bonne réponse
            if (zone.IsCorrect()) 
            {
                correct++; 
            }  
            //si mauvaise réponse
            else 
            {
                wrong++; 
            }
                
        }

        // On affiche le résultat
        _backgroundText.SetActive(true); 
        _resultText.gameObject.SetActive(true); 
        _resultText.text = "Correct : " + correct + " | Faux : " + wrong; 

        // Si tout est bon
        if (wrong == 0) 
        {
            // on affiche le bouton retour menu
            _returnMiniGameChoise.SetActive(true); 
        }
        else
        {
            // on affiche le bouton pour corriger
            _retryButton.SetActive(true); 
        }

        //on bloque les objets
        foreach (DragAndDrop obj in _draggableObjects) 
        {
            obj.enabled = false; 
        }

        // On cache le bouton valider
        _validateButton.SetActive(false); 
    }

    // FOnction qui démarre le jeu.
    public void StartGame() 
    {
        // On active la phase de jeu
        SetGameState(true); 
    }

    // Fonction qui active/désactive tous les objets déplaçables.
    private void SetGameState(bool state) 
    {
        // Activation / désactivation du drag & drop selon l'état du jeu
        foreach (DragAndDrop obj in _draggableObjects) 
        {
            // On active/désactive le script.
            obj.enabled = state; 
        }
    }

    // Fonction qui cache le résultat et redonne la main au joueur.
    public void Retry() 
    {
        // On cache les UI de résultat
        _resultText.gameObject.SetActive(false); 
        _returnMiniGameChoise.SetActive(false); 
        _retryButton.SetActive(false); 
        _backgroundText.SetActive(false); 

        // On réactive les objets drag & drop
        foreach (DragAndDrop obj in _draggableObjects) 
        {
            obj.enabled = true; 
        }

        // On reset la validation
        _canValidate = false; 
        _validateButton.SetActive(false); 
    }

    // Fonction qui reset le mini-jeu .
    public void ResetMiniGame() 
    {
        // Pour chaque dropZone
        foreach (DropZone zone in _dropZones) 
        {
            // On efface l'objet courant.
            zone.ResetMiniGame(); 
        }

        // Pour chaque objets déplaçables.
        foreach (DragAndDrop obj in _draggableObjects) 
        {
            // On les remet a leurs position initial et on les réactive.
            obj.ResetMiniGame(); 
        }

        _resultText.gameObject.SetActive(false); 
        _returnMiniGameChoise.SetActive(false); 
        _retryButton.SetActive(false); 
        _backgroundText.SetActive(false);

        _canValidate = false; 
        _validateButton.SetActive(false); 

        SetGameState(true); 
    }
}
