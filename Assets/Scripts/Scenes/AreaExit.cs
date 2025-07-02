using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExit : MonoBehaviour
{
    public GameObject fadeTransition;

    [SerializeField] private Transform targetPosition;

    private void Start()
    {
        fadeTransition.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            fadeTransition.SetActive(true);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(TeleportPlayerRoutine(player));
        }
    }

    private IEnumerator TeleportPlayerRoutine(Player player)
    {
        fadeTransition.SetActive(true);

        yield return UIFade.Instance.FadeToBlack();

        player.transform.position = targetPosition.position;

        yield return new WaitForSeconds(0.1f);

        yield return UIFade.Instance.FadeToClear();

        fadeTransition.SetActive(false);
    }
}
