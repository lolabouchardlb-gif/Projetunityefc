using UnityEngine; 
using TMPro; 

public class LinkGameManager : MonoBehaviour 
{
    [Header("Références")]
    // Référence au LineManager (lignes et résultats).
    [SerializeField] private LineManager _lineManager;
    // Nombre de connexions nécessaires pour activer la validation.
    [SerializeField] private int _totalConnectionsNeeded; 

    [Header("UI")]
    // Bouton "Vérifier/Valider".
    [SerializeField] private GameObject _validateButton;
    // Bouton pour se corriger.
    [SerializeField] private GameObject _retryButton;
    // Bouton de félicitation.
    [SerializeField] private GameObject _returnMiniGameChoiseButton;
    // Fond UI derrière le texte des résultats.
    [SerializeField] private GameObject _backgroundTxtResult;
    // Texte de résultat.
    [SerializeField] private TMP_Text _resultText; 

    //permet de savoir si le jeu a commencé
    private bool _gameStarted;

    // On initialise l'UI.
    private void Start() 
    {
        //On dit que le jeu a commencé
        _gameStarted = true; 
        //on cache tous les UI
        _validateButton.SetActive(false); 
        _retryButton.SetActive(false); 
        _returnMiniGameChoiseButton.SetActive(false); 
        _resultText.gameObject.SetActive(false); 
        _backgroundTxtResult.SetActive(false); 
    }

    // Fonction qui met à jour l'état du bouton "Valider" selon les connexions.
    private void Update() 
    {
        // Si le jeu n'a pas démarré, on sort de la fonction.
        if (!_gameStarted) 
        {
            return;
        } 

        //on verifie si toutes les connections on été faite
        bool allConnected = _lineManager.AllConnected(_totalConnectionsNeeded); 

        //si elles ont toute été faite on affiche le bouton pour vérifier
        _validateButton.SetActive(allConnected); 
    }

    // Fonction appelée quand on clique sur le bouton “Vérifier”. La fonction va Calculer les résultats et affiche l'UI de fin.
    public void Validate() 
    {
        // Variables qui recevront les résultats.
        int correct; 
        int wrong;
        //on récupère le nombre de bonnes connexions et le nombre de mauvaises réponse à partir des connexions.
        _lineManager.GetResults(out correct, out wrong);  

        //affichage du texte
        _backgroundTxtResult.SetActive(true); 
        _resultText.gameObject.SetActive(true);
        // Message de résultat.
        _resultText.text = "Liaison correct : " + correct + " | Lisaisons fausse : " + wrong; 

        // on cache l'UI
        _retryButton.SetActive(false); 
        _returnMiniGameChoiseButton.SetActive(false); 

        //si tout est juste
        if (wrong == 0) 
        {
            //on affiche le bouton de félicitation
            _returnMiniGameChoiseButton.SetActive(true); 
        }
        else
        {
            //sinon on affiche le bouton de correction
            _retryButton.SetActive(true); 
        }
        // Cache le bouton valider après validation.
        _validateButton.SetActive(false); 
        //on bloque les interractions
        _lineManager.SetInteraction(false); 
    }

    // Fonction qui va afficher l'ui pour corriger
    public void Retry() 
    {
        // On cache le fond résultat.
        _backgroundTxtResult.SetActive(false);
        // On cache le texte résultat.
        _resultText.gameObject.SetActive(false);
        // On cache le bouton retry.
        _retryButton.SetActive(false);
        // On cache l'affichage des félicitations.
        _returnMiniGameChoiseButton.SetActive(false);

        // On ré-autorise les clics.
        _lineManager.SetInteraction(true);
        // On cache le bouton pour valider valider.
        _validateButton.SetActive(false); 
    }

    // Fonction pour reset le mini jeu.
    public void ResetMiniGame() 
    {
        // On cache le fond du texte de résultat.
        _backgroundTxtResult.SetActive(false);
        // On cache le texte des résultats.
        _resultText.gameObject.SetActive(false);
        // On cache le bouton de retry.
        _retryButton.SetActive(false);
        // On cache le bouton de félicitation.
        _returnMiniGameChoiseButton.SetActive(false);
        // On cache le bouton pour valider.
        _validateButton.SetActive(false);
        // On marque le jeu comme actif.
        _gameStarted = true;
        // On reset toutes les lignes/connexions.
        _lineManager.ResetMiniGame(); 
    }
}
