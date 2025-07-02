using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public List<InventorySaveData> inventorySaveData;
    public List <SpotSaveData> spotSaveData;
    public List<QuestProgress> questProgressData;
    public List<string> handinQuestIDs;

    public int scoreNumber;
    public string activeWeaponName;

    public int damageAmount;
    public int crossbowDamage;
    public int acidAmount;
    public int dashRegargeTime;
    public int playerHealth;

    // для апдейта и сохраниения апгрейдов
    public int damageIncrease;
    public int damageCrossbowIncrease;
    public int acidNumberIncrease;
    public int dashNumberIncrease;
    public int hpNumberIncrease;
}

[System.Serializable]
public class SpotSaveData
{
    public string spotID;
    public bool isOpened;
}
