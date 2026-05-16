using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitcher : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        Debug.Log("Je change vers : " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}