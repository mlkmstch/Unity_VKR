using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadCanvas : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
