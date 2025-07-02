using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    public static Journal Instance { get; private set; }
    [SerializeField] List<Pages> pages = new List<Pages>();
    public Sprite[] images = new Sprite[4];
    private int pageIndex = 1;
    TMP_Text pagetext;
    public Image pageImage;
    public Button previousButton;
    public Button nextButton;
    private bool openJournal = false;
    public GameObject journalPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (openJournal)
            {
                QuitWindow();
            }
            else
            {
                OpenWindow();
            }
        }
    }

    public void QuitWindow()
    {
        journalPanel.SetActive(false);
        openJournal = false;
    }

    public void OpenWindow()
    {
        journalPanel.SetActive(true);
        openJournal = true;
        
        if (pagetext == null)
        {
            previousButton.gameObject.SetActive(false);
            pagetext = GameObject.Find("Page Text").GetComponent<TMP_Text>();
            pageImage.sprite = images[0];
            pagetext.text = pages[0].pageText;
        }
    }

    public void PageTurn()
    {
        previousButton.gameObject.SetActive(true);
        if (pageIndex <= pages.Count)
        {
            pageImage.sprite = images[pageIndex];
            pagetext.text = pages[pageIndex].pageText;
            pageIndex++;
            if (pageIndex == pages.Count)
            {
                nextButton.gameObject.SetActive(false);
            }
        }
    }

    public void PageTurnBack()
    {
        nextButton.gameObject.SetActive(true);
        pageIndex--;
        if (pageIndex > 0)
        {
            pageImage.sprite = images[pageIndex-1];
            pagetext.text = pages[pageIndex-1].pageText;
            
        }
        if (pageIndex == 1)
        {
            pageImage.sprite = images[0];
            pagetext.text = pages[0].pageText;
            previousButton.gameObject.SetActive(false);
        }
    }
}
