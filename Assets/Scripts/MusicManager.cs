using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource audioSource;
    public string[] scenesAvecMusique;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += SceneChargee;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void SceneChargee(Scene scene, LoadSceneMode mode)
    {
        bool jouerMusique = false;
        foreach (string nomScene in scenesAvecMusique)
        {
            if (scene.name == nomScene)
            {
                jouerMusique = true;
                break;
            }
        }
        if (jouerMusique)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}