using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon")]
public class Weapons : ScriptableObject
{
    public string weaponType;
    public string weaponName;
    public float damageAmount;
    public int usageAmount;
    public Sprite weaponSprite;
}