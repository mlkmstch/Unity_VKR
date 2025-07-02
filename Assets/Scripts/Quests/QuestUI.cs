using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public Transform questListContent;
    public GameObject questEntryPrefab;
    public GameObject objectiveTextPrefab;
    public GameObject questPanel;
    public Quest testQuest;
    public int testQuestAmount;

    private List<QuestProgress> testQuests = new();
    public static QuestUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < testQuestAmount; i++)
        {
            testQuests.Add(new QuestProgress(testQuest));
        }

        // Вызываем UI обновление с задержкой на 1 кадр, чтобы избежать ошибок при старте
        StartCoroutine(DelayedUIUpdate());
    }

    private System.Collections.IEnumerator DelayedUIUpdate()
    {
        yield return null;
        yield return null;

        UpdateQuestUI();
    }

    public void OpenQuestPanel()
    {
        if (questPanel != null)
            questPanel.SetActive(true);
    }

    public void CloseQuestPanel()
    {
        if (questPanel != null)
            questPanel.SetActive(false);
    }

    public void UpdateQuestUI()
    {
        if (questListContent == null || questEntryPrefab == null || objectiveTextPrefab == null)
        {
            Debug.LogWarning("QuestUI: Одно из обязательных полей не назначено.");
            return;
        }

        // Удаляем старые записи квестов безопасно
        for (int i = questListContent.childCount - 1; i >= 0; i--)
        {
            Transform child = questListContent.GetChild(i);
            if (child != null)
                Destroy(child.gameObject);
        }

        if (QuestController.Instance == null || QuestController.Instance.activateQuests == null)
        {
            Debug.LogWarning("QuestUI: Нет активных квестов или контроллера.");
            return;
        }

        // Добавляем новые записи
        foreach (var quest in QuestController.Instance.activateQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContent);

            TMP_Text questNameText = entry.transform.Find("QuestNameText")?.GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("ObjectiveList");

            if (questNameText != null)
                questNameText.text = quest.quest.questName;

            if (objectiveList == null) continue;

            foreach (var objective in quest.objectives)
            {
                GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                if (objText != null)
                {
                    objText.text = $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})";
                }
            }
        }
    }
}
