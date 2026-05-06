using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TransitionManager : MonoBehaviour
{

    public VideoPlayer transitionVideo;

    public GameObject transitionObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transitionObject.SetActive(false);
    }


    public void PlayTransitionAndLoadScene(string sceneName)
    {
        StartCoroutine(TransitionCoroutine(sceneName));
    }

    IEnumerator TransitionCoroutine(string sceneName)
    {
        transitionObject.SetActive(true); // Affiche la vidéo
        transitionVideo.Play();

        // Attend que la vidéo soit finie
        yield return new WaitForSeconds((float)transitionVideo.clip.length);

        // Charge la nouvelle scène
        SceneManager.LoadScene(sceneName);
    }
}
