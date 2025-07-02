using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy")]
public class Enemies : ScriptableObject
{
    //public float maxHealth;
    //public float attackSpeed;
    public float damageAmount;
    public string enemyType;
    public bool itemDrop;
}