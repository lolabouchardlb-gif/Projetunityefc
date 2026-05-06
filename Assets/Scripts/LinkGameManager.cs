using UnityEngine;
using TMPro;

public class LinkGameManager : MonoBehaviour
{
    [Header("Références")]
    public LineManager lineManager;
    public int totalConnectionsNeeded;

    [Header("UI")]
    public GameObject startPanel;
    public GameObject background;

    public GameObject validateButton;
    public GameObject retryButton;
    public GameObject ReturnMiniGameChoiseButton;

    public TMP_Text resultText;

    //permet de savoir si le jeu a commencé
    private bool gameStarted = false;

    void Start()
    {
        //On dit que le jeu n'a pas commencer
        SetGameState(false);
        //on cache tous le UI
        validateButton.SetActive(false);
        retryButton.SetActive(false);
        ReturnMiniGameChoiseButton.SetActive(false);
        resultText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameStarted) return;

        //on verifie si toutes les connections on été faite
        bool allConnected = lineManager.AllConnected(totalConnectionsNeeded);

        //si elles ont toute été faite on affiche le bouton pour vérifier
        validateButton.SetActive(allConnected);
    }

    //on lance le jeu
    public void StartGame()
    {
        gameStarted = true;
        SetGameState(true);
    }

    //gère l’affichage du début
    void SetGameState(bool state)
    {
        //si state = true alors on cache le menu
        startPanel.SetActive(!state);
        //si state = false alors on affiche le menu
        background.SetActive(!state);
    }

    // Fonction appelée quand on clique sur le bouton “Vérifier”
    public void Validate()
    {
        int correct, wrong;
        //on récupère le nombre de bonnes connexions et le nombre de mauvaises réponse
        lineManager.GetResults(out correct, out wrong);

        //affichage du texte
        resultText.gameObject.SetActive(true);
        resultText.text = "Liaison correct : " + correct + " | Lisaisons fausse : " + wrong;

        // on cache le UI
        retryButton.SetActive(false);
        ReturnMiniGameChoiseButton.SetActive(false);

        //si tout est juste
        if (wrong == 0)
        {
            //on affiche le bouton pour retourner aux choix des mini jeu
            ReturnMiniGameChoiseButton.SetActive(true);
        }
        else
        {
            //sinon on affiche le bouton recommencer
            retryButton.SetActive(true);
        }

        validateButton.SetActive(false);
        //on bloque les interractions
        lineManager.SetInteraction(false);
    }

    // afficher le ui pour corriger
    public void Retry()
    {
        resultText.gameObject.SetActive(false);

        retryButton.SetActive(false);
        ReturnMiniGameChoiseButton.SetActive(false);

        
        lineManager.SetInteraction(true);

        validateButton.SetActive(false);
    }
}