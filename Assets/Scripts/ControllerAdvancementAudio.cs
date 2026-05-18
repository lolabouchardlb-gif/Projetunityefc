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
    // Vidéo boule synchronisée avec l'audio.
    [SerializeField] private VideoPlayer _videoPlayer; 
    
    [Header("UI")]
    // Slider d'avancement de l'audio.
    [SerializeField] private Slider _slider;
    // Texte des sous-titres (affiché progressivement).
    [SerializeField] private TMP_Text _subtitleText;
    // ScrollRect pour auto-scroller les sous-titres vers le bas.
    [SerializeField] private ScrollRect _scrollRect; 

    [TextArea(5, 10)]
    // Texte complet à révéler au fur et à mesure.
    [SerializeField] private string _fullText; 

    [Header("Video affichage mot")]
    // VideoPlayer pour afficher des clips des mots.
    [SerializeField] private VideoPlayer _videoWordPlayer;
    // Objet UI contenant l'affichage de la vidéo des mots.
    [SerializeField] private GameObject _videoWordDisplay; 

    [Header("Video Timeline ")]
    // Début de chaque vidéo de mots (en secondes sur la timeline audio).
    [SerializeField] private float[] _videoStartTimes;
    // Fin de chaque vidéo de mots (en secondes).
    [SerializeField] private float[] _videoEndTimes;
    // Clips à jouer pendant les fenêtres de temps.
    [SerializeField] private VideoClip[] _videoClips;
    // Index de la vidéo de mot actuellement affichée (-1 = aucune).
    private int _currentVideoIndex = -1; 

    // nombre total de caractères du texte
    private int _totalCharacters; 

    // indique si l’audio est terminé
    private bool _hasFinished; 

    private void Start() 
    {
        // on assigne le clip audio au lecteur
        _audioSource.clip = _audioClip; 

        // slider commence à 0 et va jusqu’à la durée du son
        _slider.minValue = 0; 
        _slider.maxValue = _audioClip.length;

        // On stocke la taille du texte.
        _totalCharacters = _fullText.Length; 

        // on remet tout à zéro au niveau de l'ui au départ
        ResetUI();
        // Si on a un VideoPlayer de mot
        if (_videoWordPlayer != null) 
        {
            // On écoute la fin du clip.
            _videoWordPlayer.loopPointReached += OnEventVideoFinished; 
        }
        // On cache l'affichage des vidéo de mot au départ.
        _videoWordDisplay.SetActive(false); 
    }


    private void Update() 
    {
        // si l’audio est fini, on stop tout
        if (_hasFinished) 
        {
            return;
        }  

        // on récupère le temps actuel de l’audio
        float time = _audioSource.time; 

        // on met à jour le slider avec ce temps
        _slider.value = time; 

        // on met à jour les sous-titres
        UpdateSubtitles(time); 

        // on force le scroll vers le bas
        AutoScroll(); 

        // on vérifie si l’audio est terminé
        CheckFinish();

        // On joue/arrête les vidéos de mot selon la timeline.
        UpdateVideoEvents(time); 
    }

    // Fonction qui met à jour la vidéo de mot selon le temps actuel de l’audio.
    private void UpdateVideoEvents(float time) 
    {
        // On parcourt tous les clips/événements disponibles avec i qui est l'index de l’événement.
        for (int i = 0; i < _videoClips.Length; i++) 
        {
            // Si le temps est dans la fenêtre de l’événement i (start inclus, end exclus)
            if (time >= _videoStartTimes[i] && time < _videoEndTimes[i])
            {
                // Si la vidéo affichée n’est pas déjà celle de i, on la lance.
                if (_currentVideoIndex != i) 
                {
                    // On joue le clip associé à l’événement i.
                    PlayEventVideo(i); 
                }
                // On sort car on a trouvé l’événement correspondant.
                return; 
            }
        }

        // Si aucun événement ne correspond au temps actuel, on cache la vidéo de mot.
        HideEventVideo();
    }

    // Fonction qui démarre et affiche la vidéo de mot à l’index donné.
    private void PlayEventVideo(int index) 
    {
        // Si c’est déjà le bon index ET que la vidéo est déjà en lecture, ne rien faire.
        if (_currentVideoIndex == index && _videoWordPlayer.isPlaying)
        {
            return; 
        }
        // Mémorise quel vidéo de mot est actuellement actif.
        _currentVideoIndex = index;
        // On stoppe l’ancien clip (si un autre jouait déjà).
        _videoWordPlayer.Stop();
        // On assigne le nouveau clip à jouer.
        _videoWordPlayer.clip = _videoClips[index];
        // On active l’objet qui porte le VideoPlayer.
        _videoWordPlayer.gameObject.SetActive(true);
        // On active le conteneur UI d’affichage.
        _videoWordDisplay.SetActive(true);
        // On remet la lecture au début du clip.
        _videoWordPlayer.time = 0;
        // On lance la lecture du clip.
        _videoWordPlayer.Play(); 
    }

    // Fonction qui cache et arrête la vidéo de mot si elle est actuellement affichée.
    private void HideEventVideo() 
    {
        // Si aucun événement n’est affiché, on ne fait rien. -1 veux dire “aucune vidéo de mot active”.
        if (_currentVideoIndex == -1)
        {
            return; 
        }
        // On ttoppe la vidéo de mot.
        _videoWordPlayer.Stop();
        // On désactive l’objet du VideoPlayer.
        _videoWordPlayer.gameObject.SetActive(false);
        // On désactive le conteneur UI.
        _videoWordDisplay.SetActive(false);
        // OnRreset l’état : aucune vidéo de mot active.
        _currentVideoIndex = -1; 
    }

    // Fonction appelé quand la vidéo événement atteint la fin.
    private void OnEventVideoFinished(VideoPlayer vp) 
    {
        // Quand le clip est finit, on cache l’affichage de la vidéo de mot.
        HideEventVideo(); 
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
    private void PlayBoth()
    {
        // on sort du mode "fin"
        _hasFinished = false;

        //on lance l'audio
        _audioSource.Play();

        // si la vidéo existe alors on la lance
        if (_videoPlayer != null) 
        {
            _videoPlayer.Play();
        }
             
    }


    //fonction pour mettre en pause l'audio et la vidéo
    private void PauseBoth()
    {
        //on met en pause l'audio
        _audioSource.Pause();

        // si la vidéo existe alors on la met en pause
        if (_videoPlayer != null) 
        {
            _videoPlayer.Pause(); 
        }
            
    }


    //fonction pour mise à jour les sous-titres
    private void UpdateSubtitles(float time)
    {
        // progression entre 0 et 1
        float progress = Mathf.Clamp01(time / _audioClip.length);

        // nombre de caractères à afficher selon progression
        int charCount = Mathf.Clamp(Mathf.RoundToInt(progress * _totalCharacters), 0, _totalCharacters);

        // on affiche une partie du texte
        _subtitleText.text = _fullText.Substring(0, charCount);
    }


    // fonction pour le scroll automatique du texte vers le bas
    private void AutoScroll()
    {
        //on force Unity à recalculer UI
        Canvas.ForceUpdateCanvases();

        //on force a mettre le viewport en bas du scroll
        _scrollRect.verticalNormalizedPosition = 0f; 
    }

    //fonction pour vérifier si l’audio est presque fini
    private void CheckFinish()
    {
        if (_audioSource.time >= _audioClip.length - 0.05f)
        {
            // on bloque les updates
            _hasFinished = true; 

            //on laisse le texte affiché
            _subtitleText.text = _fullText;

            //si la vidéo existe on la met en pause
            if (_videoPlayer != null) 
            {
                _videoPlayer.Pause();
            }
                
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
    private void ResetAll()
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
    private void ResetUI()
    {
        //on vide l'affichage du texte
        _subtitleText.text = "";

        //on remet slider à 0
        _slider.value = 0f;

        //on reset état de l'audio
        _hasFinished = false; 
    }
}
