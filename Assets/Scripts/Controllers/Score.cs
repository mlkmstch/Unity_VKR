using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    [SerializeField] Text scoreText;
    void Start()
    {
        scoreText.text = "";
    }

    void Update()
    {
        scoreText.text = "" + Pickup.coinCount;
    }
}
