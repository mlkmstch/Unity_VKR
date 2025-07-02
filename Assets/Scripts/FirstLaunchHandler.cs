using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLaunchHandler : MonoBehaviour
{
    public Animator animator;
    public GameObject cutscenePanel;

    private void Awake()
    {
        cutscenePanel.SetActive(false);

    }
    void Start()
    {
        PlayerPrefs.DeleteKey("HasLaunched");

        if (!PlayerPrefs.HasKey("HasLaunched"))
        {
            RunFirstLaunchLogic();
            PlayerPrefs.SetInt("HasLaunched", 1);
            PlayerPrefs.Save();
        }
    }

    void RunFirstLaunchLogic()
    {
        if (animator != null)
        {
            cutscenePanel.SetActive(true);
            animator.SetTrigger("CutsceneTrigger");
            Invoke("StopCutscene", 10);
        }
        else
        {
            Debug.LogWarning("Animator не назначен!");
        }
    }

    private void StopCutscene()
    {
        cutscenePanel.SetActive(false);
    }
}