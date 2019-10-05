using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static PlayerControls player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
