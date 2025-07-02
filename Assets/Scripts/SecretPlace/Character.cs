using UnityEngine;

public class Character : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;

    public SpriteRenderer SpriteRenderer => m_SpriteRenderer;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}