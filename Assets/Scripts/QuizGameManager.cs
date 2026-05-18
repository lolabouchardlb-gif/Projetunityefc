using TMPro; 
using UnityEngine; 
using UnityEngine.UI;
// On permet d'utiliser directement le type Question.
using static QuestionsPeupleUn; 

public class QuizGameManager : MonoBehaviour 
{
    [Header("Questions")]
    // Liste des questions du quiz.
    [SerializeField] private Question[] _questions; 

    [Header("UI")]
    // Texte de la question affichée.
    [SerializeField] private TMP_Text _questionText;
    // Texte "Question X/Y".
    [SerializeField] private TMP_Text _progressText;
    // Texte "Bonne/Mauvaise réponse".
    [SerializeField] private TMP_Text _feedbackText;
    // Boutons pour les réponses.
    [SerializeField] private Button[] _answerButtons;
    // Bouton "Valider".
    [SerializeField] private GameObject _validateButton;
    // Bouton "Suivant".
    [SerializeField] private GameObject _nextButton;
    // Bouton de félicitation.
    [SerializeField] private GameObject _returnMiniGameChoiseButton; 

    [Header("Couleurs des boutons de réponse")]
    // Couleur du bouton sélectionné.
    [SerializeField] private Color _selectedColor = Color.yellow; .
    // Couleur par défaut des boutons.
    [SerializeField] private Color _normalColor = Color.white;
    // Index de la question courante dans _questions.
    private int _currentQuestion;
    // Index de la réponse choisie (-1 = aucune).
    private int _selectedAnswer = -1; 

    private void Start() 
    {
        // On active l'UI du quiz.
        SetGameState(true);
        // On cache le bouton "Valider" au départ.
        _validateButton.SetActive(false);
        // On cache le bouton "Suivant" au départ.
        _nextButton.SetActive(false);
        // On cache le feedback au départ.
        _feedbackText.gameObject.SetActive(false);
        // On cache le bouton de félicitation au départ.
        _returnMiniGameChoiseButton.SetActive(false);
        // On affiche la première question.
        DisplayQuestion(); 
    }

    // Fonction qui active/désactive les éléments UI du quiz.
    private void SetGameState(bool state) 
    {

        // UI du quiz
        _questionText.gameObject.SetActive(state); 
        _progressText.gameObject.SetActive(state);
        // Pour chaque bouton de réponse.
        foreach (Button btn in _answerButtons) 
        {
            // On affiche/cache le bouton.
            btn.gameObject.SetActive(state); 
        }
        // Quand on ré-active, on repart sans validation.
        _validateButton.SetActive(false);
        // On cache le bouton "Suivant".
        _nextButton.SetActive(false);
        // On cache le feedback.
        _feedbackText.gameObject.SetActive(false); 
    }

    // Méthode appelée quand on commencer le quiz.
    public void StartGame() 
    {
        // On active l'UI du quiz.
        SetGameState(true); 
        //on affiche la question
        DisplayQuestion(); 
    }

    //fonction servant a afficher la question, les boutons pour répondre a la question et ou on en est
    public void DisplayQuestion() 
    {
        //on récupère la question selon l'index actuel.
        Question q = _questions[_currentQuestion]; 

        //on affiche la question
        _questionText.text = q.QuestionText; 

        //on affiche ou on en est par rapport a toutes les questions
        _progressText.text = "Question " + (_currentQuestion + 1) + "/" + _questions.Length; 
        //aucune réponse sélectionnée
        _selectedAnswer = -1;

        //on remet tout à 0
        // On cache le bouton "Valider" tant que rien n'est choisi.
        _validateButton.SetActive(false);
        // On cache le bouton "Suivant".
        _nextButton.SetActive(false);
        // On cache le feedback.
        _feedbackText.gameObject.SetActive(false); 

        //on boucle sur chaque bouton de réponse
        for (int i = 0; i < _answerButtons.Length; i++) 
        {
            //on active les boutons
            _answerButtons[i].interactable = true; 

            //on réinitialise la couleur à normal pour les bouton de choix de réponse
            _answerButtons[i].image.color = _normalColor; 

            //on met le texte de la réponse
            _answerButtons[i].GetComponentInChildren<TMP_Text>().text = q.Answers[i]; 

            // index = numéro de la réponse
            int index = i; 

            //on enlève les anciens clics
            _answerButtons[i].onClick.RemoveAllListeners();

            //on ajoute un nouveau clic et on appelle la fonction SelectAnswer(index) pour sélectionner cette réponse.
            _answerButtons[i].onClick.AddListener(() => SelectAnswer(index)); 
        }
    }

    // Fonction appeller quand on clique sur une réponse
    public void SelectAnswer(int index) 
    {
        //on enregistre la réponse choisie
        _selectedAnswer = index; 

        //on affiche le bouton Valider
        _validateButton.SetActive(true); 

        Debug.Log("Réponse sélectionnée : " + index); 

        //Reset des couleurs
        foreach (Button btn in _answerButtons) 
        {
            btn.image.color = _normalColor; 
        }

        //Couleur du bouton sélectionné
        _answerButtons[index].image.color = _selectedColor; 
    }

    //fonction appelée quand on clique sur le bouton Valider
    public void ValidateAnswer() 
    {
        // Si rien n'est choisi, alors on ne valide pas.
        if (_selectedAnswer == -1) 
        {
            return; 
        } 

        //on récupère la question
        Question q = _questions[_currentQuestion]; 

        //on affiche le feedback 
        _feedbackText.gameObject.SetActive(true); 

        //on compare la réponse sélectionné avec la bonne réponse, si la réponse sélectionner est égal à la bonne réponse
        if (_selectedAnswer == q.CorrectAnswerIndex) 
        {
            //on affiche le fait que ça soit la bonne réponse
            _feedbackText.text = "Bonne réponse !"; 

            //on affiche le bouton pour passer à la prochaine question
            _nextButton.SetActive(true); 
            _validateButton.SetActive(false); 

            //on bloque les boutons
            foreach (Button btn in _answerButtons) 
            {
                // On empêche de changer la réponse après validation.
                btn.interactable = false; 
            }
        }
        //si ce n'est pas la bonne réponse on dit que c'est la mauvaise réponse
        else
        {
            _feedbackText.text = "Mauvaise réponse, réessaie !"; 
        }
    }

    // Fonction pour aller a la question suivante
    public void NextQuestion() 
    {
        //on passe à la question suivante
        _currentQuestion++; 

        //s’il reste des questions
        if (_currentQuestion < _questions.Length) 
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

    // Fonction pour l'affichage de la fin du quizz
    public void EndQuiz() 
    {
        //on dit que le quizz est terminer
        _questionText.text = "Quiz terminé !"; 

        //on enlève le compteur de question
        _progressText.text = ""; 

        //on dit "Bravo" au joueur
        _feedbackText.gameObject.SetActive(true);
        // Message de félicitations.
        _feedbackText.text = "Bravo !";
        // On cache le bouton "Valider".
        _validateButton.SetActive(false);
        // On cache le bouton "Suivant".
        _nextButton.SetActive(false); 

        //on cache toutes les réponses
        foreach (Button btn in _answerButtons) 
        {
            // On masque les boutons de réponse.
            btn.gameObject.SetActive(false); 
        }

        //on affiche le bouton pour retouner aux vhoix des minis jeux
        _returnMiniGameChoiseButton.SetActive(true); 
    }

    // Fonction pour reset le mini jeu.
    public void ResetMiniGame() 
    {
        // On revient à la première question.
        _currentQuestion = 0;
        // Aucune réponse sélectionnée.
        _selectedAnswer = -1;
        // Cache le bouton de félicitation.
        _returnMiniGameChoiseButton.SetActive(false);
        // On réactive l'UI du quiz.
        SetGameState(true);
        // On réaffiche la première question.
        DisplayQuestion(); 
    }
}
