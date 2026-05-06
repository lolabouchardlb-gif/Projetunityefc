using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static QuestionsPeupleUn;

public class QuizGameManager : MonoBehaviour
{
    [Header("Questions")]
    public Question[] questions;

    [Header("UI")]
    public TMP_Text questionText;
    public TMP_Text progressText;
    public TMP_Text feedbackText;
    public GameObject startPanel;
    public GameObject background;

    public Button[] answerButtons;
    public GameObject validateButton;
    public GameObject nextButton;
    public GameObject returnMiniGameChoiseButton;

    [Header("Couleurs des boutons de réponse")]
    public Color selectedColor = Color.yellow;
    public Color normalColor = Color.white;

    private int currentQuestion = 0;
    private int selectedAnswer = -1;

    private void Start()
    {
        SetGameState(false);

        validateButton.SetActive(false);
        nextButton.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        returnMiniGameChoiseButton.SetActive(false);
    }

    private void SetGameState(bool state)
    {
        // UI de départ
        startPanel.SetActive(!state);
        background.SetActive(!state);

        // UI du quiz
        questionText.gameObject.SetActive(state);
        progressText.gameObject.SetActive(state);

        foreach (Button btn in answerButtons)
        {
            btn.gameObject.SetActive(state);
        }

        validateButton.SetActive(false);
        nextButton.SetActive(false);
        feedbackText.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        SetGameState(true);
        //on affiche la question
        DisplayQuestion();
    }

    //fonction servant a affichier la question, les boutons pour répondre a la question et ou on en est
    public void DisplayQuestion()
    {
        //on récupère la question
        Question q = questions[currentQuestion];

        //on affiche la question
        questionText.text = q.questionText;

        //on affiche ou on en est par rapport a toutes les questions
        progressText.text = "Question " + (currentQuestion + 1) + "/" + questions.Length;

        //aucune réponse sélectionnée
        selectedAnswer = -1;

        //on remet tout à 0
        validateButton.SetActive(false);
        nextButton.SetActive(false);
        feedbackText.gameObject.SetActive(false);

        //on boucle sur chaque bouton de réponse
        for (int i = 0; i < answerButtons.Length; i++)
        {
            //on active les boutons
            answerButtons[i].interactable = true;

            //on réinitialise la couleur à normal pour les bouton de choix de réponse
            answerButtons[i].image.color = normalColor;

            //on met le texte de la réponse
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = q.answers[i];

            // index = numéro de la réponse
            int index = i;

            //on enlève les anciens clics
            answerButtons[i].onClick.RemoveAllListeners();

            //on ajoute un nouveau clic et on appelle la fonction SelectAnswer(index)
            answerButtons[i].onClick.AddListener(() => SelectAnswer(index));
        }
    }

    //Quand on clique sur une réponse
    public void SelectAnswer(int index)
    {
        //on enregistre la réponse choisie
        selectedAnswer = index;

        //on affiche le bouton Valider
        validateButton.SetActive(true);

        Debug.Log("Réponse sélectionnée : " + index);

        //Reset couleurs
        foreach (Button btn in answerButtons)
        {
            btn.image.color = normalColor;
        }

        //Couleur du bouton sélectionné
        answerButtons[index].image.color = selectedColor;
    }

    //fonction appelée quand on clique sur le bouton Valider
    public void ValidateAnswer()
    {
        if (selectedAnswer == -1) return;

        //on récupère la question
        Question q = questions[currentQuestion];

        //on affiche le feedback (Vrai/faux)
        feedbackText.gameObject.SetActive(true);

        //on compare la réponse sélectionné avec la bonne réponse, si la réponse sélectionner est égal à la bonne réponse
        if (selectedAnswer == q.correctAnswerIndex)
        {
            //on affiche le fait que ça soit la bonne réponse
            feedbackText.text = "Bonne réponse !";

            //on affiche le bouton pour passer à la prochaine question
            nextButton.SetActive(true);
            validateButton.SetActive(false);

            //on bloque les boutons
            foreach (Button btn in answerButtons)
            {
                btn.interactable = false;
            }
        }
        //si ce n'est pas la bonne réponse on dit que c'est la mauvaise réponse
        else
        {
            feedbackText.text = "Mauvaise réponse, réessaie !";
        }
    }

    //Question suivante
    public void NextQuestion()
    {
        //on passe à la question suivante
        currentQuestion++;

        //s’il reste des questions
        if (currentQuestion < questions.Length)
        {
            //on affiche une question
            DisplayQuestion();
        }
        //s'il n'y a plus de question
        else
        {
            //on affiche la fin du quizz
            EndQuiz();
        }
    }

    //affichage de la fin du quizz
    public void EndQuiz()
    {
        //on dit que le quizz est terminer
        questionText.text = "Quiz terminé !";

        //on enlève le compteur de question
        progressText.text = "";

        //on dit "Bravo" au joueur
        feedbackText.gameObject.SetActive(true);
        feedbackText.text = "Bravo !";

        validateButton.SetActive(false);
        nextButton.SetActive(false);

        //on cache toutes les réponses
        foreach (Button btn in answerButtons)
        {
            btn.gameObject.SetActive(false);
        }

        //on affiche le bouton pour retouner aux vhoix des minis jeux
        returnMiniGameChoiseButton.SetActive(true);
    }
}