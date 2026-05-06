using UnityEngine;

public class QuestionsPeupleUn : MonoBehaviour
{
    // on affiche la class dans l'inspecteur
    [System.Serializable]
    public class Question
    {
        // texte de la question
        public string questionText;
        // Tableau des réponses possibles
        public string[] answers;
        //Index de la bonne réponse dans le tableau
        public int correctAnswerIndex;
    }
}
