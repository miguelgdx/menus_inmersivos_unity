using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    Vector3 moveDirection = Vector3.zero;
    CharacterController controller;
    float maxSpeed = 5;
    float speed = 4;
    Camera mainCamera;
    float gravity = 9.8f;
    public float vSpeed = 0;
    float jumpSpeed = 5;
    float groundRayLength = 2000;
    public string status = "idle";
    Animator animator;
    Vector3 moveDir = Vector3.zero;
    //float acc = 0;

    [SerializeField] 
    public GameObject pauseMenu;
    [SerializeField] 
    public bool Paused;

    // Start is called before the first frame update

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        animator = GetComponentInChildren<Animator>();
    }

    private void updateRotation()
    {
        // Rotacion del personaje
        Vector3 newRot = moveDir;
        newRot.y = 0;
        transform.forward = newRot;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector de camara a personaje
        //Vector3 camToCharacterDir = (this.transform.position - Camera.main.transform.position).normalized;
        // Le quitamos la componente "y" (vertical).
        //camToCharacterDir.y = 0;
        moveDir = Vector3.zero;

        // Key control
        if (Input.GetKey(KeyCode.W))
        {
            moveDir = mainCamera.transform.forward;
            updateRotation();
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir = mainCamera.transform.right;
            updateRotation();
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir = mainCamera.transform.right * -1;
            updateRotation();
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir = mainCamera.transform.forward * -1;
            updateRotation();
        }

        vSpeed -= gravity * Time.deltaTime;
        Debug.Log("On ground? : " + controller.isGrounded);

        if (controller.isGrounded)
        {
            vSpeed = 0.0f; // grounded character has vSpeed = 0...
        }
        if (Input.GetKeyDown("space") && canJump())
        { // unless it jumps:
            vSpeed = jumpSpeed;
        }

        Debug.Log("Velocity es: " + controller.velocity);
        

        if ((controller.velocity.x != 0 || controller.velocity.z != 0) && onGround())
        {
            status = "run";
        }
        if ((controller.velocity.x == 0 && controller.velocity.z == 0) && onGround())
        {
                status = "idle";
        }
        if (controller.velocity.y > 0.1f && !onGround())
        {
            status = "jump";
        }

        Debug.Log("Status: " + status);

        playAnimation(status);


        /*
        if (onGround())
        {
            vSpeed = 0; // grounded character has vSpeed = 0...
            if (Input.GetKeyDown("space"))
            { // unless it jumps:
                vSpeed = jumpSpeed;
            }
        }*/

        // Normalizamos sin componente "y" para que sea la direccion real del personaje.
        moveDir.y = 0;
        moveDir = moveDir.normalized;

        // Después aplicamos cualquier gravedad o movimiento vertical.
        moveDir.y = vSpeed;

        Debug.DrawRay(this.transform.position, moveDir, Color.red);

        controller.Move(moveDir * Time.deltaTime * speed);

        // Comprobar Pause.
        //Podría ser también al pulsar el botón de Pause Input.GetButtonDown ("Pause")
        if (Input.GetKeyDown(KeyCode.P))
        {
            Paused = !Paused;
            if (Paused)
            {
                ActivateMenu();
            }
        }
        


    }


    private void playAnimation(string stat)
    {
        if (stat.Equals("idle")) {
            animator.SetTrigger("idle");
        }
        if (stat.Equals("run"))
        {
            animator.SetTrigger("run");
        }
        if (stat.Equals("jump"))
        {
            animator.SetTrigger("jump");
        }
    }

    
    private bool onGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, new Vector3(0, -groundRayLength, 0));


        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            Debug.Log("Collision con: " + hit.collider.name + " distancia: " + hit.distance);
        }
        else
        {
            return false;
        }
        Debug.DrawLine(this.transform.position, new Vector3(0, -groundRayLength, 0));
        //Debug.Log("Distancia a: " + r.collider + " es de: " + r.distance);
        if (hit.collider != null && hit.distance < 1)
            return true;

        
        if (hit.collider.gameObject.tag.Equals("stairs"))
        {
            if (hit.collider != null && hit.distance < 2)
                return true;
        }
            

        return false;
    }

    private bool canJump()
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, new Vector3(0, -groundRayLength, 0));


        Physics.Raycast(ray, out hit);
        Debug.DrawLine(this.transform.position, new Vector3(0, -groundRayLength, 0));
        //Debug.Log("Distancia a: " + r.collider + " es de: " + r.distance);
        if (hit.collider != null && hit.distance < 2)
            return true;
        else
            return false;
    }


    public void ActivateMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        MouseRaycast mr = Camera.main.GetComponent<MouseRaycast>();
        mr.enabled = false;
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        MouseRaycast mr = Camera.main.GetComponent<MouseRaycast>();
        mr.enabled = true;
        Paused = !Paused;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
