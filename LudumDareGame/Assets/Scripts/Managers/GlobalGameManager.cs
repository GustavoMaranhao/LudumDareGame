using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static PlayerControls player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
    public static UIManager uiManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIManager>();

    void Start()
    {
    }
}
