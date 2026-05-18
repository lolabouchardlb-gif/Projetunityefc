using UnityEngine; // Base Unity.
using UnityEngine.UI; // UI (Slider).
using UnityEngine.Video; // VideoPlayer.

public class CommandAudio : MonoBehaviour // Contrôle la lecture audio et synchronise une vidéo avec cet audio.
{
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource; // Composant qui joue le son.
    [SerializeField] private AudioClip _audioClip; // Clip audio à jouer.

    [Header("Video")]
    [SerializeField] private VideoPlayer _videoPlayer; // Vidéo à synchroniser avec l'audio.

    [Header("UI Buttons")]
    [SerializeField] private GameObject _playButton; // Bouton Play.
    [SerializeField] private GameObject _pauseButton; // Bouton Pause.
    [SerializeField] private GameObject _restartButton; // Bouton Restart.

    [Header("Slider")]
    [SerializeField] private Slider _audioSlider; // Slider d'avancement (ici remis à 0 au restart).

    // indique si on est en pause
    private bool _isPaused;
    // indique si l’audio est terminé
    private bool _hasFinished; 

    private void Awake() // Appelé avant Start.
    {
        // empêche Unity VideoPlayer de gérer l’audio (évite conflits et bugs)
        _videoPlayer.audioOutputMode = VideoAudioOutputMode.None; // La vidéo ne sort pas d'audio.
    }

    private void Start() // Initialisation.
    {
        // on assigne le clip audio au lecteur
        _audioSource.clip = _audioClip; // Associe le clip au AudioSource.

        // état initial : pas en pause
        _isPaused = false;

        // état initial : pas terminé
        _hasFinished = false;

        // mise à jour initiale des boutons UI
        UpdateButtons(); // Met l'UI dans le bon état au départ.
    }

    private void Update() // Boucle par frame.
    {
        // synchronise la vidéo avec l’audio à chaque frame
        SyncAudioAndVideo(); // Synchronise l'état de la vidéo avec l'audio.

        // vérifie si l’audio est terminé
        CheckIfFinished(); // Détecte si l'audio est terminé.

        // met à jour l’UI (boutons play/pause/restart)
        UpdateButtons(); // Met à jour les boutons selon l'état.
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
    public void Play() // Handler bouton Play.
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
    public void Pause() // Handler bouton Pause.
    {
        // pause audio
        _audioSource.Pause();

        // pause vidéo
        _videoPlayer.Pause();

        // on mémorise l’état pause
        _isPaused = true;
    }

    // bouton restart
    public void Restart() // Handler bouton Restart.
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
    private void UpdateButtons() // Affiche/cache les boutons selon l'état (playing/paused/finished).
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
    private void StopVideo() // Stoppe et remet la vidéo au début (fonction utilitaire).
    {
        _videoPlayer.Stop();
        _videoPlayer.time = 0f;
    }
}
