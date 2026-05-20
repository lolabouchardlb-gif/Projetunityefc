using UnityEngine;
using UnityEngine.UI;

public class ClickSoundManager : MonoBehaviour
{
    public static ClickSoundManager Instance;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clickSound;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AddSoundsToButtons();
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene,
                               UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        AddSoundsToButtons();
    }

    private void AddSoundsToButtons()
    {
        Button[] allButtons;


        allButtons = Object.FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Button btn in allButtons)
        {
            btn.onClick.RemoveListener(PlayClick);
            btn.onClick.AddListener(PlayClick);
        }
    }

    private void PlayClick()
    {
        _audioSource.PlayOneShot(_clickSound);
    }
}
