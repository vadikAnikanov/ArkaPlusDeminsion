using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EventAggregation;

public class FpsController : MonoBehaviour {

    delegate void SimlpeDel();
    SimlpeDel CurrentInput;

    Rigidbody rb;

    public float speed = 5;


    public Camera cam;
    public Transform ballsStartPos;


    public Joystick joystick;
    float PlaceForAim;

    private void Awake()
    {
        EventAggregator.Subscribe<SpawnFpsPlayer>(OnSpawnStart);
    }

    void Start()
    {

        PlaceForAim = Screen.width / 2;

        rb = GetComponent<Rigidbody>();

    
        
#if UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        CurrentInput += InputFromWin;
#endif

#if !UNITY_EDITOR
        CurrentInput += InpuFromAndroid;
 #endif
        
        


    }

    void OnSpawnStart(IEventBase eventBase)
    {
        SpawnFpsPlayer startPos = eventBase as SpawnFpsPlayer;

        transform.position = startPos.startPos;

    }

    private void FixedUpdate()
    {
     FireAndroid();
     CurrentInput();
    }


    float LastShootTime = 0;
    void InputFromWin()
    {

        if (Input.GetMouseButton(0) && Time.time - LastShootTime > 0.3f)
        {
            LastShootTime = Time.time;
            Fire();
        }

        float AxisHor = Input.GetAxisRaw("Horizontal");
        float AxisVert = Input.GetAxisRaw("Vertical");

        if (rb.velocity.sqrMagnitude < speed)
        {
            Vector3 moveVec = cam.transform.right * AxisHor + cam.transform.forward * AxisVert;
            moveVec = Vector3.ProjectOnPlane(moveVec, Vector3.down).normalized;

            moveVec.x *= speed;
            moveVec.z *= speed;
            moveVec.y = 0;
                
              rb.AddForce(moveVec*0.4f, ForceMode.Impulse);
        }

        Vector3 rotationX = new Vector3(0, Input.GetAxisRaw("Mouse X"), 0);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotationX));
        Vector3 rotationY = new Vector3(Input.GetAxisRaw("Mouse Y"), 0, 0);
        cam.transform.Rotate(-rotationY);



        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
        
    }

    void InpuFromAndroid()
    {
        float AxisHor = joystick.Horizontal;
        float AxisVert = joystick.Vertical;

        if (rb.velocity.sqrMagnitude < speed)
        {
            Vector3 moveVec = cam.transform.right * AxisHor + cam.transform.forward * AxisVert;
            moveVec = Vector3.ProjectOnPlane(moveVec, Vector3.down).normalized;

            moveVec.x *= speed;
            moveVec.z *= speed;
            moveVec.y = 0;

            rb.AddForce(moveVec * 0.4f, ForceMode.Impulse);
        }

        for (int i = 0; i < 3; i++)
        {
            if (Input.touchCount > i)
                if (Input.GetTouch(i).position.x > PlaceForAim) ApplyAim(Input.GetTouch(i));
        }

    }

    public bool fireOn = false;
    public void FireOnOff(bool fireSwitch)
    {
        fireOn = fireSwitch;
    }

    public void FireAndroid()
    {
        if (Time.time - LastShootTime > 0.3f && fireOn)
        {
            LastShootTime = Time.time;
            Fire();
        }
    }
    


    void ApplyAim(Touch myTouch)
    {

        Vector3 rotationX = new Vector3(0, myTouch.deltaPosition.x*0.1f, 0);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotationX));
        Vector3 rotationY = new Vector3(myTouch.deltaPosition.y*0.1f, 0, 0);
        cam.transform.Rotate(-rotationY);
    }

    void Fire()
    {
        
        cam.transform.Rotate(new Vector3(-1, 0, 0));

        BallStartEvent ballstart = new BallStartEvent()
        {
            startPos = ballsStartPos.position,
            startVelocity = ballsStartPos.forward.normalized * 7f
        };
        EventAggregator.Publish(ballstart);

        SoundFireEvent soundfire = new SoundFireEvent();
        EventAggregator.Publish(soundfire);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        

    }


}
