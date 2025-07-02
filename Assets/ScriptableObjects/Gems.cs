using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGem", menuName = "Gem")]
public class Gems : ScriptableObject
{
    public Sprite gemSprite;
    public float hardness;
    public string syngony;
    public string gloss;
    public bool transparency;
    public int cost;
    public bool analyzed;
}
