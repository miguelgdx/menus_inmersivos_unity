using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            // Restarle vida a su script.
            Status sScript = player.GetComponent<Status>();
            GameData.playerWon = true;
            GameData.playerTime = sScript.currentGameTime;
            SceneManager.LoadScene("End", LoadSceneMode.Single);
        }
    }

}
