using System.Collections;
using UnityEngine;
public class FadeInGroup : MonoBehaviour
{
    public CanvasGroup[] elements;
    public float duration = 2f;
    public float delayBetween = 0.3f;
    void Start()
    {
        StartCoroutine(FadeSequence());
    }
    IEnumerator FadeSequence()
    {
        foreach (CanvasGroup cg in elements)
        {
            StartCoroutine(FadeIn(cg));
            yield return new WaitForSeconds(delayBetween);
        }
    }
    IEnumerator FadeIn(CanvasGroup cg)
    {
        float t = 0f;
        cg.alpha = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;
            p = Mathf.SmoothStep(0, 1, p);
            cg.alpha = p;
            yield return null;
        }
        cg.alpha = 1;
    }
}