﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOffControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        releasedButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickControl()
    {
        
    }

    public void pushedButton()
    {
        GetComponentInChildren<TMPro.TextMeshPro>().color = Color.red;
    }

    public void releasedButton()
    {
        GetComponentInChildren<TMPro.TextMeshPro>().color = Color.black;
    }
}
