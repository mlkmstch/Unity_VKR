using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    public static UIFade Instance { get; private set; }

    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed = 1f;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Публичные корутины для использования с yield return
    public IEnumerator FadeToBlack()
    {
        yield return StartFade(1f);
    }

    public IEnumerator FadeToClear()
    {
        yield return StartFade(0f);
    }

    // Общая логика анимации затемнения/осветления
    private IEnumerator StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha));
        yield return fadeCoroutine;
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        Color color = fadeScreen.color;

        while (!Mathf.Approximately(color.a, targetAlpha))
        {
            color.a = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            fadeScreen.color = color;
            yield return null;
        }

        fadeScreen.color = new Color(color.r, color.g, color.b, targetAlpha);
    }
}
