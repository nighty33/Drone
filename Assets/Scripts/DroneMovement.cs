using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    public bool throttle => Input.GetKey(KeyCode.Space);

    public float pitchPower, rollPower, yawPower, dronePower;
    private float activeRoll, activePitch, activeYaw;
    private float gravity = -9.81f;
    public float distToGround = 1f;

    Rigidbody rbody;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    void _rotateDrone(float activePitch, float activeRoll, float activeYaw)
    {
        transform.Rotate(activePitch * pitchPower * Time.deltaTime,
            activeYaw * yawPower * Time.deltaTime,
            -activeRoll * rollPower * Time.deltaTime, Space.Self);
    }

    private void FixedUpdate()
    {
        float pp, rp, yp;
        if (throttle)
        {
            transform.position += transform.up * dronePower * Time.deltaTime;
            pp = pitchPower;
            rp = rollPower;
            yp = yawPower;
        }
        else
        {
            pp = pitchPower / 2;
            rp = rollPower / 2;
            yp = yawPower / 2;
        }
        activePitch = Input.GetAxisRaw("Vertical") * pp * Time.deltaTime;
        activeRoll = Input.GetAxisRaw("Horizontal") * rp * Time.deltaTime;
        activeYaw = Input.GetAxisRaw("Yaw") * yp * Time.deltaTime;
        _rotateDrone(activePitch, activeRoll, activeYaw);

        GroundCheck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isGrounded = false;

    void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.1f))
        {
            rbody.velocity = Vector3.zero;
            print("Grounded");
        }
        else
        {
            rbody.velocity = new Vector3(0, (gravity * 3/dronePower), 0);
            print("Flying");
        }
    }
}