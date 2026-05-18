using UnityEngine; 
using UnityEngine.Video; 

public class DisableAfterVideo : MonoBehaviour 
{
    // Objet à cacher à la fin de la vidéo.
    [SerializeField] private GameObject _screenVideo;
    // Référence au VideoPlayer sur le même GameObject.
    private VideoPlayer _videoPlayer; 

    private void Start() 
    {
        // On récupère le VideoPlayer.
        _videoPlayer = GetComponent<VideoPlayer>(); 

        // On reset vidéo
        _videoPlayer.Stop(); 
        _videoPlayer.frame = 0;
        // On lance la vidéo.
        _videoPlayer.Play(); 

        // Quand la vidéo finit
        _videoPlayer.loopPointReached += OnVideoFinished; 
    }

    // Fonction appelé quand la vidéo se termine.
    private void OnVideoFinished(VideoPlayer vp) 
    {
        // On cache le panneau vidéo
        _screenVideo.SetActive(false); 
    }

    // Fonction appelé quand l'objet est réactivé.
    private void OnEnable() 
    {
        // Si référence existe
        if (_screenVideo != null) 
        {
            // On réaffiche le la vidéo.
            _screenVideo.SetActive(true); 
        }

        // Si on n'a pas encore récupéré le VideoPlayer.
        if (_videoPlayer == null) 
        {
            // On le récupère.
            _videoPlayer = GetComponent<VideoPlayer>(); 
        }
            

        // On rejoue la vidéo
        _videoPlayer.Stop(); 
        _videoPlayer.frame = 0; 
        _videoPlayer.Play(); 
    }
}
