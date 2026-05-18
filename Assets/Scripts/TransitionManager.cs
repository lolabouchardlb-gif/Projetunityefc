using System.Collections; 
using UnityEngine; 
using UnityEngine.SceneManagement; 
using UnityEngine.Video; 

public class TransitionManager : MonoBehaviour 
{
    // Le VideoPlayer qui lit la vidéo de transition.
    [SerializeField] private VideoPlayer _transitionVideo;
    // L'objet UI/Canvas qui affiche la transition.
    [SerializeField] private GameObject _transitionObject; 

    private void Start() 
    {
        // On cache la transition au lancement de la scène.
        _transitionObject.SetActive(false); 
    }

    // Méthode appelée pour jouer la vidéo de transition et changer de scène.
    public void PlayTransitionAndLoadScene(string sceneName) 
    {
        // On lance la coroutine qui joue la vidéo puis charge la scène.
        StartCoroutine(TransitionCoroutine(sceneName)); 
    }

    // Coroutine qui exécute la transition.
    private IEnumerator TransitionCoroutine(string sceneName) 
    {
        // On affiche l'objet de transition.
        _transitionObject.SetActive(true);
        // On démarre la lecture de la vidéo de transition.
        _transitionVideo.Play(); 

        // On attend que la vidéo soit finie.
        yield return new WaitForSeconds((float)_transitionVideo.clip.length); 

        // On charge la nouvelle scène.
        SceneManager.LoadScene(sceneName); 
    }
}

