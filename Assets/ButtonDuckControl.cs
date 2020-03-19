using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDuckControl : MonoBehaviour
{
    public GameObject duck;
    public bool pushed = false;
    Animation ani;
    Animator animator;
    private float TIMER_AMOUNT = 2;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        ani = duck.GetComponent<Animation>();
        animator = duck.GetComponent<Animator>();
        releasedButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (pushed && timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            releasedButton();
        }
    }

    public void OnClickControl()
    {
        
    }

    public void pushedButton()
    {
        if (!pushed) {
            pushed = true;
            GetComponentInChildren<TMPro.TextMeshPro>().color = Color.red;
            ani["BirdAnimation"].speed = 0;
        }
    }

    public void releasedButton()
    {
        ani["BirdAnimation"].speed = 1;
        pushed = false;
        timer = TIMER_AMOUNT;
        GetComponentInChildren<TMPro.TextMeshPro>().color = Color.black;
    }
}
