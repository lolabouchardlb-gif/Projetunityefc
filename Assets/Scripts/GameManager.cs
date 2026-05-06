using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public DropZone[] dropZones;
    public GameObject validateButton;
    public TMP_Text resultText;
    public GameObject ReturnMiniGameChoise;
    public GameObject retryButton;

    public GameObject startPanel;
    public GameObject background;
    public DragAndDrop[] draggableObjects;

    // Indique si le jeu a commencé
    private bool gameStarted = false;

    // Indique si on peut afficher le bouton valider
    private bool canValidate = false;

    void Start()
    {
        // On désactive la phase de jeu au départ
        SetGameState(false);

        // On cache tous les éléments UI du jeu
        validateButton.SetActive(false);
        resultText.gameObject.SetActive(false);
        ReturnMiniGameChoise.SetActive(false);
        retryButton.SetActive(false);
    }

    void Update()
    {
        // Si le jeu n'a pas commencé alors on ne fait rien
        if (!gameStarted) return;

        //on suppose que tout est rempli
        bool allFilled = true;

        //On vérifie toutes les zones de drop
        foreach (DropZone zone in dropZones)
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
            canValidate = true;
            // on affiche le bouton
            validateButton.SetActive(true);
        }
    }

    public void Validate()
    {
        // nombre de bonnes réponses
        int correct = 0;
        // nombre de mauvaises réponses
        int wrong = 0;

        // On vérifie chaque zone
        foreach (DropZone zone in dropZones)
        {
            // si bonne réponse
            if (zone.IsCorrect())
                correct++;
            //si mauvaise réponse
            else
                wrong++;
        }

        // On affiche le résultat
        resultText.gameObject.SetActive(true);
        resultText.text = "Correct : " + correct + " | Faux : " + wrong;

        // Si tout est bon
        if (wrong == 0)
        {
            // on affiche le bouton retour menu
            ReturnMiniGameChoise.SetActive(true);
        }
        else
        {
            // on affiche le bouton pour corriger
            retryButton.SetActive(true);
        }

        //on bloque les objets
        foreach (DragAndDrop obj in draggableObjects)
        {
            obj.enabled = false;
        }

        // On cache le bouton valider
        validateButton.SetActive(false);
    }

    public void StartGame()
    {
        // Le jeu commence
        gameStarted = true;
        // On active la phase de jeu
        SetGameState(true);
    }

    void SetGameState(bool state)
    {
        // UI de départ cachée et fond caché selon l'état
        startPanel.SetActive(!state);
        background.SetActive(!state);

        // Activation / désactivation du drag & drop selon l'état du jeu
        foreach (DragAndDrop obj in draggableObjects)
        {
            obj.enabled = state;
        }
    }

    public void Retry()
    {
        // On cache les UI de résultat
        resultText.gameObject.SetActive(false);
        ReturnMiniGameChoise.SetActive(false);
        retryButton.SetActive(false);

        // On réactive les objets drag & drop
        foreach (DragAndDrop obj in draggableObjects)
        {
            obj.enabled = true;
        }

        // On reset la validation
        canValidate = false;
        validateButton.SetActive(false);
    }
}