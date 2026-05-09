using UnityEngine;
using TMPro;

public class LinkGameManager : MonoBehaviour
{
    [Header("Références")]
    public LineManager lineManager;
    public int totalConnectionsNeeded;

    [Header("UI")]
    public GameObject validateButton;
    public GameObject retryButton;
    public GameObject ReturnMiniGameChoiseButton;
    public GameObject BackgroundTxtResult;

    public TMP_Text resultText;

    //permet de savoir si le jeu a commencé
    private bool gameStarted = false;

    void Start()
    {
        //On dit que le jeu a commencé
        gameStarted = true;
        //on cache tous les UI
        validateButton.SetActive(false);
        retryButton.SetActive(false);
        ReturnMiniGameChoiseButton.SetActive(false);
        resultText.gameObject.SetActive(false);
        BackgroundTxtResult.SetActive(false);
    }

    void Update()
    {
        if (!gameStarted) 
        {
            return;
        } 

        //on verifie si toutes les connections on été faite
        bool allConnected = lineManager.AllConnected(totalConnectionsNeeded);

        //si elles ont toute été faite on affiche le bouton pour vérifier
        validateButton.SetActive(allConnected);
    }

    // Fonction appelée quand on clique sur le bouton “Vérifier”
    public void Validate()
    {
        int correct, wrong;
        //on récupère le nombre de bonnes connexions et le nombre de mauvaises réponse
        lineManager.GetResults(out correct, out wrong);

        //affichage du texte
        BackgroundTxtResult.SetActive(true);
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
        BackgroundTxtResult.SetActive(false);
        resultText.gameObject.SetActive(false);

        retryButton.SetActive(false);
        ReturnMiniGameChoiseButton.SetActive(false);

        
        lineManager.SetInteraction(true);

        validateButton.SetActive(false);
    }
}