using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {
    public Transform barrel;
    public Transform firePoint;
    public GameObject cannonballPrefab;
    public float launchForce = 15f;
    public float torqueAmount = 5f;

    public float maxHoldTime = 3f;
    public float maxScaleMultiplier = 5f;

    private float holdTime = 0f;

    public float rotationSpeed = 10f;

    void Update() {
        RotateBarrel();
        if (Input.GetButton("Fire1")) {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0, maxHoldTime); // Clamp the hold time to maxHoldTime
        }
        if (Input.GetButtonUp("Fire1")) {
            FireCannon();
            holdTime = 0f; // Reset hold time after firing
        }
    }

    void FireCannon() {

        float scaleMultiplier = Mathf.Lerp(1f, maxScaleMultiplier, holdTime / maxHoldTime);

        GameObject cannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);

        cannonball.transform.localScale = Vector3.one * 0.25f * scaleMultiplier;

        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * launchForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * torqueAmount, ForceMode.Impulse);
        Destroy(cannonball, 5f);
    }

    void RotateBarrel() {
        float horizontalRotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        float verticalRotation = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;

        barrel.Rotate(Vector3.up, horizontalRotation, Space.World);
        barrel.Rotate(Vector3.right, -verticalRotation, Space.Self);
    }
}