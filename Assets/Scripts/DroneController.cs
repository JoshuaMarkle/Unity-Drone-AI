using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{

    private Rigidbody rb;

    [Header("Wing Information")]
    public float thrust = 5f;
    public float wingAcc = 10f;
    public float rotateSpeed = 10f;
    [Space]
    public Transform wing1;
    public Transform wing2;
    public Transform wing3;
    public Transform wing4;
    [Space]
    public float wingForce1;
    public float wingForce2;
    public float wingForce3;
    public float wingForce4;
    public float generalForce;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        // Debug.DrawLine(wing1.position, thrust * transform.up, Color.red, 0.1f, true);
        // Debug.DrawLine(wing2.position, thrust * transform.up, Color.red, 0.1f, true);
        // Debug.DrawLine(wing3.position, thrust * transform.up, Color.red, 0.1f, true);
        // Debug.DrawLine(wing4.position, thrust * transform.up, Color.red, 0.1f, true);

        // if (Input.GetKey(KeyCode.Alpha1)) {wingForce1 = Mathf.Lerp(wingForce1, thrust, wingAcc * Time.deltaTime);} else {wingForce1 = Mathf.Lerp(wingForce1, 0f, wingAcc * Time.deltaTime);}
        // if (Input.GetKey(KeyCode.Alpha2)) {wingForce2 = Mathf.Lerp(wingForce2, thrust, wingAcc * Time.deltaTime);} else {wingForce2 = Mathf.Lerp(wingForce2, 0f, wingAcc * Time.deltaTime);}
        // if (Input.GetKey(KeyCode.Alpha3)) {wingForce3 = Mathf.Lerp(wingForce3, thrust, wingAcc * Time.deltaTime);} else {wingForce3 = Mathf.Lerp(wingForce3, 0f, wingAcc * Time.deltaTime);}
        // if (Input.GetKey(KeyCode.Alpha4)) {wingForce4 = Mathf.Lerp(wingForce4, thrust, wingAcc * Time.deltaTime);} else {wingForce4 = Mathf.Lerp(wingForce4, 0f, wingAcc * Time.deltaTime);}
        // if (Input.GetKey(KeyCode.Alpha5)) {
        //     generalForce = Mathf.Lerp(generalForce, thrust, wingAcc * Time.deltaTime);
        // } else {generalForce = Mathf.Lerp(generalForce, 0f, wingAcc * Time.deltaTime);}

        rb.AddForceAtPosition((generalForce + wingForce1) * transform.up, wing1.position);
        rb.AddForceAtPosition((generalForce + wingForce2) * transform.up, wing2.position);
        rb.AddForceAtPosition((generalForce + wingForce3) * transform.up, wing3.position);
        rb.AddForceAtPosition((generalForce + wingForce4) * transform.up, wing4.position);
        //rb.AddForceAtPosition(generalForce * 4f * transform.up, transform.position);

        wing1.Rotate(0f, 0f, (generalForce + wingForce1) * rotateSpeed);
        wing2.Rotate(0f, 0f, (generalForce + wingForce2) * rotateSpeed);
        wing3.Rotate(0f, 0f, (generalForce + wingForce3) * rotateSpeed);
        wing4.Rotate(0f, 0f, (generalForce + wingForce4) * rotateSpeed);

        // wing1.Rotate(0f, 0f, generalForce * rotateSpeed);
        // wing2.Rotate(0f, 0f, generalForce * rotateSpeed);
        // wing3.Rotate(0f, 0f, generalForce * rotateSpeed);
        // wing4.Rotate(0f, 0f, generalForce * rotateSpeed);

        if (Input.GetKeyDown(KeyCode.Escape)) {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
    }
}
