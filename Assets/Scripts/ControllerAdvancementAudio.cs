using UnityEngine; 
using UnityEngine.UI; 
using TMPro; 
using UnityEngine.Video; 

public class ControllerAdvancementAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;
    
    [Header("Video")]
    [SerializeField] private VideoPlayer _videoPlayer;
    
    [Header("UI")]
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _subtitleText;

    [SerializeField] private ScrollRect _scrollRect;

    [TextArea(5, 10)]
    [SerializeField] private string _fullText;

    // nombre total de caractères du texte
    private int _totalCharacters;

    // indique si l’audio est terminé
    private bool _hasFinished;

    void Start()
    {
        // on assigne le clip audio au lecteur
        _audioSource.clip = _audioClip;

        // slider commence à 0 et va jusqu’à la durée du son
        _slider.minValue = 0;
        _slider.maxValue = _audioClip.length;

        // on calcule la taille totale du texte
        _totalCharacters = _fullText.Length;

        // on remet tout à zéro au niveau de l'ui au départ
        ResetUI();
    }


    void Update()
    {
        // si l’audio est fini, on stop tout
        if (_hasFinished) return;

        // on récupère le temps actuel de l’audio
        float _time = _audioSource.time;

        // on met à jour le slider avec ce temps
        _slider.value = _time;

        // on met à jour les sous-titres
        UpdateSubtitles(_time);

        // on force le scroll vers le bas
        AutoScroll();

        // on vérifie si l’audio est terminé
        CheckFinish();
    }


    //bouton principal qui permet de mettre play / pause / restart quand on clique dessus
    public void TogglePlayPause()
    {
        // si au clique l'audio est terminé alors on fait un reset complet puis play
        if (_hasFinished || _audioSource.time >= _audioClip.length)
        {
            //on remet l'audio et la vidéo à zéro
            ResetAll();

            //puis on relance l'audio et la vidéo
            PlayBoth(); 
            return;
        }

        // si audio est en lecture au clique alors on met en pause l'audio et la vidéo
        if (_audioSource.isPlaying)
        {
            PauseBoth();
        }
        else
        {
            // sinon on fait jouer l'audio et la vidéo
            PlayBoth();
        }
    }


    //fonction pour faire jouer l'audio et la vidéo
    void PlayBoth()
    {
        // on sort du mode "fin"
        _hasFinished = false;

        //on lance l'audio
        _audioSource.Play();

        // si la vidéo existe alors on la lance
        if (_videoPlayer != null)
            _videoPlayer.Play(); 
    }


    //fonction pour mettre en pause l'audio et la vidéo
    void PauseBoth()
    {
        //on met en pause l'audio
        _audioSource.Pause();

        // si la vidéo existe alors on la met en pause
        if (_videoPlayer != null)
            _videoPlayer.Pause(); 
    }


    //fonction pour mise à jour les sous-titres
    void UpdateSubtitles(float _time)
    {
        // progression entre 0 et 1
        float _progress = Mathf.Clamp01(_time / _audioClip.length);

        // nombre de caractères à afficher selon progression
        int _charCount = Mathf.Clamp(
            Mathf.RoundToInt(_progress * _totalCharacters),
            0,
            _totalCharacters
        );

        // on affiche une partie du texte
        _subtitleText.text = _fullText.Substring(0, _charCount);
    }


    // fonction pour le scroll automatique du texte vers le bas
    void AutoScroll()
    {
        //on force Unity à recalculer UI
        Canvas.ForceUpdateCanvases();

        //on force a mettre le viewport en bas du scroll
        _scrollRect.verticalNormalizedPosition = 0f; 
    }


    //fonction pour vérifier si l’audio est presque fini
    void CheckFinish()
    {
        if (_audioSource.time >= _audioClip.length - 0.05f)
        {
            // on bloque les updates
            _hasFinished = true; 

            //on laisse le texte affiché
            _subtitleText.text = _fullText; 

            //si la vidéo existe on la met en pause
            if (_videoPlayer != null)
                _videoPlayer.Pause();
        }
    }


    //fonction pour déplacer le slider manuellement
    public void MovePosition()
    {
        //on fait suivre le temps audio a la valeur de nore slider
        _audioSource.time = _slider.value;

        //on reset état de fin
        _hasFinished = false; 
    }


    //fonction pour reset l'audio et la vidéo
    void ResetAll()
    {
        //on stop l'audio
        _audioSource.Stop(); 

        //on remet l'audio au début
        _audioSource.time = 0f; 

        //si la vidéo existe
        if (_videoPlayer != null)
        {
            //on la stop
            _videoPlayer.Stop(); 

            //on la remet au début
            _videoPlayer.time = 0; 
        }

        //on reset le ui
        ResetUI(); 
    }


    //fonction pour reset l'UI
    void ResetUI()
    {
        //on vide l'affichage du texte
        _subtitleText.text = "";

        //on remet slider à 0
        _slider.value = 0f;

        //on reset état de l'audio
        _hasFinished = false; 
    }
}