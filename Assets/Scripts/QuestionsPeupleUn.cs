using UnityEngine; // Donne accès à MonoBehaviour et aux attributs Unity.

public class QuestionsPeupleUn : MonoBehaviour // Contient la définition d'une question (structure de données).
{
    // On rend la classe sérialisable pour pouvoir la voir/éditer dans l'Inspector.
    [System.Serializable] // Indique à Unity que cette classe peut être sérialisée.
    public class Question // Représente une question + ses réponses.
    {
        // Texte affiché au joueur.
        public string QuestionText; // Exemple: "Quelle est la capitale...".
        // Liste des réponses possibles.
        public string[] Answers; // Exemple: ["Paris","Lyon","..."].
        // Index (position) de la bonne réponse dans le tableau Answers.
        public int CorrectAnswerIndex; // Exemple: 0 si la bonne réponse est Answers[0].
    }
}
