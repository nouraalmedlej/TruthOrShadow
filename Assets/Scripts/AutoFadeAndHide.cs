using UnityEngine;

public class AutoFadeAndHide : MonoBehaviour
{
    public CanvasGroup group;      // اسحبي CanvasGroup هنا
    public float showTime = 1.5f;  // وقت الظهور الكامل قبل التلاشي
    public float fadeDuration = 1f;// مدة التلاشي

    void Awake()
    {
        if (!group) group = GetComponent<CanvasGroup>();
        if (group) group.alpha = 1f;
    }

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(FadeAndHide());
    }

    System.Collections.IEnumerator FadeAndHide()
    {
        // انتظار قبل التلاشي
        yield return new WaitForSeconds(showTime);

        float t = 0f;
        float start = group ? group.alpha : 1f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (group) group.alpha = Mathf.Lerp(start, 0f, t / fadeDuration);
            yield return null;
        }

        if (group) group.alpha = 0f;
        gameObject.SetActive(false); // يخفي الأب وكل الأبناء
    }
}
