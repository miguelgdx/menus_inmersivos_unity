using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text winText;
    public Text loseText;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (GameData.playerWon == true)
        {
            winText.enabled = true;
            loseText.enabled = false;
        }
        else
        {
            winText.enabled = false;
            loseText.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
