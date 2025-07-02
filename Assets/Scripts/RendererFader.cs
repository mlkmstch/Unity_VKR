using UnityEngine;
using UnityEngine.TextCore.Text;

public class RendererFader : MonoBehaviour
{

    const float VISIBLE_ALPHA = 1f;
    const float TRANSPARENT_ALPHA = 0.3f;

    private SpriteRenderer[] m_SpriteRenderers;
    private bool m_FadeOutEnabled = false;

    private Character m_Interactor;
    private int m_InitialSortOrder;

    private SpriteRenderer BackgroundObject => m_SpriteRenderers[0];
    private float BackgroundObjectAlpha => BackgroundObject.color.a;

    void Start()
    {
        m_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (m_FadeOutEnabled && BackgroundObjectAlpha > TRANSPARENT_ALPHA)
        {
            FadeOut();

            if (BackgroundObjectAlpha == TRANSPARENT_ALPHA)
            {
                m_Interactor.SpriteRenderer.sortingOrder = BackgroundObject.sortingOrder - 1;
            }
        }
        else if (!m_FadeOutEnabled && BackgroundObjectAlpha < VISIBLE_ALPHA)
        {
            FadeIn();
            m_Interactor.SpriteRenderer.sortingOrder = m_InitialSortOrder;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_FadeOutEnabled) return;

        if (collision.TryGetComponent<Character>(out var character))
        {
            m_Interactor = character;
            m_InitialSortOrder = character.SpriteRenderer.sortingOrder;
            m_FadeOutEnabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!m_FadeOutEnabled) return;

        if (collision.TryGetComponent<Character>(out var character))
        {
            m_FadeOutEnabled = false;
        }
    }

    private void FadeOut()
    {
        foreach (var renderer in m_SpriteRenderers)
        {
            ChangeOpacity(renderer, TRANSPARENT_ALPHA);
        }
    }

    private void FadeIn()
    {
        foreach (var renderer in m_SpriteRenderers)
        {
            ChangeOpacity(renderer, VISIBLE_ALPHA);
        }
    }

    private void ChangeOpacity(SpriteRenderer renderer, float targetAlpha)
    {
        Color color = renderer.color;
        Color smoothColor = new(color.r, color.g, color.b,
            Mathf.MoveTowards(color.a, targetAlpha, Time.deltaTime * 2)
        );
        renderer.color = smoothColor;
    }
}