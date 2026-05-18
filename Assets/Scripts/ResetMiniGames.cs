using UnityEngine; 
using UnityEngine.SceneManagement; 
using System.Collections;

public class ResetMiniGames : MonoBehaviour 
{
    [Header("Scènes mini-jeux à reset")]
    // Liste des noms de scènes à reset.
    [SerializeField] private string[] _miniGameSceneNames;

    // Méthode pour reset les mini jeu.
    public void ResetAllMiniGames() 
    {
        // On démarre la coroutine qui va reset chaque scène de mini jeu.
        StartCoroutine(ResetAllMiniGamesRoutine()); 
    }

    // On fait une Coroutine pour exécuter des actions étalées sur plusieurs frames.
    private IEnumerator ResetAllMiniGamesRoutine() 
    {
        // On reset les mini-jeux même si leurs scènes ne sont pas actuellement chargées.
        // Et on évite un NullReference si la liste n'est pas assignée.
        if (_miniGameSceneNames != null) 
        {
            // On parcourt tous les noms de scènes configurés.
            for (int i = 0; i < _miniGameSceneNames.Length; i++) 
            {
                // On récupère le nom de la scène à l'index i.
                string sceneName = _miniGameSceneNames[i];
                // On ignore les entrées vides/espaces.
                if (string.IsNullOrWhiteSpace(sceneName)) 
                {
                    continue;
                }

                // On ignore si c'est déjà la scène active.
                if (SceneManager.GetActiveScene().name == sceneName) 
                {
                    continue;
                }

                // On charge la scène en "additive" (sans remplacer la scène courante).
                AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                // Si Unity n'a pas pu lancer le chargement, on passe à la suivante.
                if (loadOp == null) 
                {
                    continue;
                }

                // On attend la fin du chargement.
                while (!loadOp.isDone) 
                {
                    yield return null; 
                }

                // On récupère l'objet Scene correspondant au nom.
                Scene loadedScene = SceneManager.GetSceneByName(sceneName);
                // On vérifie que la scène existe et est bien chargée.
                if (loadedScene.IsValid() && loadedScene.isLoaded) 
                {
                    // Liste des GameObjects "racine" de cette scène.
                    GameObject[] roots = loadedScene.GetRootGameObjects();
                    // Parcourt chaque racine pour chercher des GameManagers.
                    for (int r = 0; r < roots.Length; r++) 
                    {
                        // On cherche un GameManager pour le mini jeu de drag&drop même s'il est inactif.
                        GameManager dragDropGameManager = roots[r].GetComponentInChildren<GameManager>(true);
                        // Si il est trouvé, alors on reset le mini-jeu.
                        if (dragDropGameManager != null) 
                        {
                            dragDropGameManager.ResetMiniGame();
                        }

                        // On cherche le GameManager du mini-jeu de liaisons.
                        LinkGameManager linkGameManager = roots[r].GetComponentInChildren<LinkGameManager>(true);
                        // Si il est trouvé, alors on reset le mini-jeu.
                        if (linkGameManager != null) 
                        {
                            linkGameManager.ResetMiniGame();
                        }

                        // On cherche le GameManager du mini-jeu de quiz.
                        QuizGameManager quizGameManager = roots[r].GetComponentInChildren<QuizGameManager>(true);
                        // Si il est trouvé, alors on reset le mini-jeu.
                        if (quizGameManager != null) 
                        {
                            quizGameManager.ResetMiniGame();
                        }

                        // On cherche le LineManager .
                        LineManager lineManager = roots[r].GetComponentInChildren<LineManager>(true);
                        // Si il est trouvé, alors on reset le mini-jeu.
                        if (lineManager != null) 
                        {
                            lineManager.ResetMiniGame();
                        }
                    }

                    // On décharge la scène.
                    AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(loadedScene);
                    // On vérifie que l'opération existe.
                    if (unloadOp != null) 
                    {
                        while (!unloadOp.isDone) 
                        {
                            // On attend la fin du déchargement.
                            yield return null; 
                        } 
                    }
                }
            }
        }
    }
}
