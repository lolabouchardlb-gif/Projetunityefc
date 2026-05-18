using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MapPanelController : MonoBehaviour
{
    public RectTransform panel;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Vector2 hiddenPos = new Vector2(420, 0);
    public Vector2 visiblePos = new Vector2(-40, 0);
    public float duration = 8.0f;
    private string currentSceneName;
    Coroutine current;
    void Start()
    {
        panel.anchoredPosition = hiddenPos;
    }
    public void Show(string title, string desc, string sceneName)
    {
        if (panel == null || titleText == null || descriptionText == null)
        {
            Debug.LogError($"{nameof(MapPanelController)}: Références UI manquantes sur {name}.", this);
            return;
        }

        Debug.Log($"{nameof(MapPanelController)}: Show('{title}', scene '{sceneName}')", this);
        currentSceneName = sceneName;
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(SwitchRoutine(title, desc));
    }
    public void LoadSelectedScene()
    {
        if (!string.IsNullOrEmpty(currentSceneName))
        {
            SceneManager.LoadScene(currentSceneName);
        }
    }
    public void Hide()
    {
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(Slide(hiddenPos));
    }
    public void ShowTest()

    {

        Show("Titre test", "Description test", "NomScene");

    }

    IEnumerator SwitchRoutine(string title, string desc)
    {
        yield return Slide(hiddenPos);
        titleText.text = title;
        descriptionText.text = desc;
        yield return new WaitForSeconds(0.05f);
        yield return Slide(visiblePos);
    }
    IEnumerator Slide(Vector2 target)
    {
        Vector2 start = panel.anchoredPosition;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;
            p = Mathf.Pow(p, 3f); // easing lent
            panel.anchoredPosition = Vector2.Lerp(start, target, p);
            yield return null;
        }
        panel.anchoredPosition = target;
    }
}
