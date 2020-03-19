using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject followed;
    public GameObject room;


    public float scaleRotation = 3.0f;
    public float scaleHeight = 1.0f;
    public float scaleZoom = 1.0f;
    public float deltaCameraPosition = 0.1f;
    public float maxDistance = 10.0f;
    public float minDistance = 0.5f;

    public Vector3 relativePosition;
    /*
    public Vector3 centerOfRoom;
    public Vector2 sizeOfRoom;
    */

    void Awake()
    {

        _cameraHeight = 0.0f;
        _cameraRotation = 0.0f;
        _cameraZoom = 0.0f;

        controls = new PlayerInput();

        controls.Play.CameraV.performed += ctx => _cameraHeight = ctx.ReadValue<float>();
        controls.Play.CameraV.canceled += ctx => _cameraHeight = 0.0f;
        controls.Play.CameraH.performed += ctx => _cameraRotation = ctx.ReadValue<float>();
        controls.Play.CameraH.canceled += ctx => _cameraRotation = 0.0f;
        controls.Play.CameraZoom.performed += ctx => _cameraZoom = ctx.ReadValue<float>();
        controls.Play.CameraZoom.canceled += ctx => _cameraZoom = 0.0f;

        Cursor.lockState = CursorLockMode.Locked;
    }
    void OnEnable()
    {
        controls.Play.Enable();
    }

    void OnDisable()
    {
        controls.Play.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _distance = relativePosition.magnitude;

        if (followed)
        {
            transform.position = followed.transform.position + relativePosition;
            transform.LookAt(followed.transform.position);

        }
        if (room)
            _bounds = calculateBounds(room, deltaCameraPosition);

    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        //float rot = 0.0f;
        //float zoom = 1.0f;
        //float height = 0.0f;
        float newdistance = 0.0f;
        Vector3 maxWithMin = new Vector3(0, 0, 0);
        Vector3 minWithMax = new Vector3(0, 0, 0);


        //rot = Input.GetAxis("Camera Horizontal");
        //zoom = Input.GetAxis("Camera Zoom");
        //height = Input.GetAxis("Camera Vertical");
        _cameraHeight = _cameraHeight * -1;
        transform.position += new Vector3(0.0f, scaleHeight * _cameraHeight, 0.0f);
        _distance += scaleZoom * _cameraZoom;
        _distance = Mathf.Clamp(_distance, minDistance, maxDistance);

        if (followed)
        {
            transform.RotateAround(followed.transform.position, new Vector3(0.0f, 1.0f, 0.0f), scaleRotation * _cameraRotation);
            newdistance = Vector3.Distance(followed.transform.position, transform.position);
            transform.position += transform.TransformDirection(new Vector3(0.0f, 0.0f, 1.0f)) * (newdistance - _distance);
            maxWithMin = Vector3.Max(transform.position, _bounds.min);
            minWithMax = Vector3.Min(maxWithMin, _bounds.max);
            transform.position = minWithMax;
            transform.LookAt(followed.transform.position);
        }



    }

    /* caculateBounds: calculate bound object for children pf obj
     *  Is is assumed that each child has a collision box to calculate bound
     *  A margin is also added  (to avoid seeing through the walls) (delta)*/

    private static Bounds calculateBounds(GameObject obj, float delta)
    {
        Bounds objBounds = new Bounds(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        Vector3 min = new Vector3(0, 0, 0);
        Vector3 max = new Vector3(0, 0, 0);
        Collider[] childColliders = obj.GetComponentsInChildren<Collider>();
        foreach (Collider col in childColliders)
        {
            min = col.bounds.min;
            max = col.bounds.max;
            min = Vector3.Min(min, objBounds.min);
            max = Vector3.Max(max, objBounds.max);
            objBounds.SetMinMax(min, max);
        }
        min = objBounds.min + obj.transform.position + delta * new Vector3(1, 1, 1);
        max = objBounds.max + obj.transform.position - delta * new Vector3(1, 1, 1);
        objBounds.SetMinMax(min, max);
        return objBounds;
    }

    private PlayerInput controls;
    private float _cameraHeight;
    private float _cameraRotation;
    private float _cameraZoom;
    private float _distance;
    private Bounds _bounds;


}
