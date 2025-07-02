using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private string savePath;
    public GameObject mainMenu;

    public void PlayGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            mainMenu.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Сохранение не найдено. Загружаем новую игру.");
            SceneManager.LoadScene("Scene1");
            mainMenu.SetActive(false);
        }
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Для редактора
        #else
            Application.Quit(); // Для сборки
        #endif
    }

    private void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

}
