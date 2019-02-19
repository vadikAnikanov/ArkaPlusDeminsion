using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventAggregation;

public class BallController : MonoBehaviour
{
    public Camera cam;
    public ParticleSystem particle;

    float timeAterLaunch = 0;

    Rigidbody rb;
    MeshRenderer mesh;
    CapsuleCollider ballCollider;

    void Awake()
    {
        EventAggregator.Subscribe<BallStartEvent>(OnBallStart);
        EventAggregator.Subscribe<BallsCollideWrongplaneEvent>(OnBallCollideWrongPlane);
        EventAggregator.Subscribe<BallCollideFinalPlane>(EndBallFlyNoSound);
    }

    void OnBallStart(IEventBase eventBase)
    {
        BallStartEvent ballData = eventBase as BallStartEvent;

        ballCollider.enabled = true;
        transform.SetPositionAndRotation(ballData.startPos, Quaternion.identity);
        GetComponent<Rigidbody>().velocity = ballData.startVelocity;

        mesh.enabled = true;
        cam.transform.position = transform.position;
        cam.gameObject.SetActive(true);

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        ballCollider = GetComponent<CapsuleCollider>();
        cam.gameObject.SetActive(false);
    }



  
    private void LateUpdate()
    {
       cam.transform.position = Vector3.Lerp(cam.transform.position, transform.position, Time.deltaTime*4f);

       if (!(rb.velocity == Vector3.zero))
        cam.transform.forward = rb.velocity;

    }



    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Plane"))
        {
            EndBallFly();

            BallCollideWallEvent ballfail = new BallCollideWallEvent();
            EventAggregator.Publish(ballfail);
        }
        if (collision.gameObject.CompareTag("Plane"))
        {
            BallsPlaneCollideEvent planeCollideEvent = new BallsPlaneCollideEvent()
            {
                planeNum = collision.gameObject.GetComponent<PlaneInfo>().planeNum,
                levelNum = collision.gameObject.GetComponent<PlaneInfo>().levelNum,
                finalPlane = collision.gameObject.GetComponent<PlaneInfo>().finalPlane
            };
            EventAggregator.Publish(planeCollideEvent);
        }
    }

    
    void OnBallCollideWrongPlane(IEventBase eventbase)
    {
        EndBallFly();
    }

    void EndBallFlyNoSound(IEventBase eventbase)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        particle.Play(true);
        cam.gameObject.SetActive(false);
        mesh.enabled = false;
        ballCollider.enabled = false;
    }

    void EndBallFly()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        particle.Play(true);
        cam.gameObject.SetActive(false);
        mesh.enabled = false;
        ballCollider.enabled = false;
        SoundBallCrash ballcrash = new SoundBallCrash();
        EventAggregator.Publish(ballcrash);
    }
}
