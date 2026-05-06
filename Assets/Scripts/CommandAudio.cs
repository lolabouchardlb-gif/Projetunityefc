using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CommandAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource; 
    [SerializeField] private AudioClip _audioClip; 

    [Header("Video")]
    [SerializeField] private VideoPlayer _videoPlayer; 

    [Header("UI Buttons")]
    [SerializeField] private GameObject _playButton; 
    [SerializeField] private GameObject _pauseButton; 
    [SerializeField] private GameObject _restartButton; 

    [Header("Slider")]
    [SerializeField] private Slider _audioSlider;

    // indique si on est en pause
    private bool _isPaused;
    // indique si l’audio est terminé
    private bool _hasFinished; 

    private void Awake()
    {
        // empêche Unity VideoPlayer de gérer l’audio (évite conflits et bugs)
        _videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
    }

    private void Start()
    {
        // on assigne le clip audio au lecteur
        _audioSource.clip = _audioClip;

        // état initial : pas en pause
        _isPaused = false;

        // état initial : pas terminé
        _hasFinished = false;

        // mise à jour initiale des boutons UI
        UpdateButtons();
    }

    private void Update()
    {
        // synchronise la vidéo avec l’audio à chaque frame
        SyncAudioAndVideo();

        // vérifie si l’audio est terminé
        CheckIfFinished();

        // met à jour l’UI (boutons play/pause/restart)
        UpdateButtons();
    }

    private void SyncAudioAndVideo()
    {
        // si l’audio joue
        if (_audioSource.isPlaying)
        {
            // si la vidéo ne joue pas déjà
            if (!_videoPlayer.isPlaying)
            {
                // on lance la vidéo
                _videoPlayer.Play();
            }
        }
        else
        {
            // si l’audio n’est pas fini
            if (_hasFinished)
            {
                // on stop totalement la vidéo à la fin
                if (_videoPlayer.isPlaying)
                    _videoPlayer.Stop();
            }
            else
            {
                // sinon on met juste en pause la vidéo
                if (_videoPlayer.isPlaying)
                    _videoPlayer.Pause();
            }
        }
    }

    private void CheckIfFinished()
    {
        // sécurité : si pas de clip, on sort
        if (_audioClip == null) return;

        // si on est presque à la fin ET pas déjà marqué comme fini
        if (!_hasFinished &&
            _audioSource.time >= _audioClip.length - 0.1f)
        {
            // on marque comme terminé
            _hasFinished = true;

            // on met pause audio
            _audioSource.Pause();

            // on met pause vidéo
            _videoPlayer.Pause();
        }
    }

    // bouton play
    public void Play()
    {
        // sécurité : si clip non assigné
        if (_audioSource.clip == null)
            _audioSource.clip = _audioClip;

        // on sort des états pause / finish
        _hasFinished = false;
        _isPaused = false;

        // on reprend ou lance l’audio
        _audioSource.UnPause();
        if (!_audioSource.isPlaying)
            _audioSource.Play();

        // on lance la vidéo
        _videoPlayer.Play();
    }

    // bouton pause
    public void Pause()
    {
        // pause audio
        _audioSource.Pause();

        // pause vidéo
        _videoPlayer.Pause();

        // on mémorise l’état pause
        _isPaused = true;
    }

    // bouton restart
    public void Restart()
    {
        // on reset les états
        _hasFinished = false;
        _isPaused = false;

        // reset audio complet
        // on arrête l'audio 
        _audioSource.Stop();
        // on re-assigne le clip
        _audioSource.clip = _audioClip;
        // retour au début
        _audioSource.time = 0f;

        // reset vidéo complet
        // on arrête la vidéo
        _videoPlayer.Stop();
        // retour au début
        _videoPlayer.time = 0f;
        // on reset les frame
        _videoPlayer.frame = 0; 

        // on relance l'audio et la vidéo
        _audioSource.Play();
        _videoPlayer.Play();

        // on reset le slider 
        _audioSlider.value = 0f;
    }

    // gestion des boutons
    private void UpdateButtons()
    {
        // si la lecture est terminée
        if (_hasFinished)
        {
            _playButton.SetActive(false);
            _pauseButton.SetActive(false);
            _restartButton.SetActive(true);
        }
        // si l'audio est en lecture
        else if (_audioSource.isPlaying)
        {
            _playButton.SetActive(false);
            _pauseButton.SetActive(true);
            _restartButton.SetActive(false);
        }
        // si on est en pause
        else if (_isPaused)
        {
            _playButton.SetActive(true);
            _pauseButton.SetActive(false);
            _restartButton.SetActive(false);
        }
        // état initial
        else
        {
            _playButton.SetActive(true);
            _pauseButton.SetActive(false);
            _restartButton.SetActive(false);
        }
    }

    // arrêter la vidéo
    private void StopVideo()
    {
        _videoPlayer.Stop();
        _videoPlayer.time = 0f;
    }
}