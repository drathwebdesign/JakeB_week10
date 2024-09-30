using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour {
    public int gunDamage = 1;
    public float fireRate = 0.5f;
    public float fireRange = 50f;
    public float hitForce = 15f;
    public float waitTime;
    public Transform firePoint;


    Camera fpsCam;
    AudioSource audioSource;
    LineRenderer lineRenderer;
    float nextFire;

    void Start() {
        fpsCam = Camera.main;
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFire) {
            nextFire = Time.time + fireRate;
            StartCoroutine(shootingEffect());

            // raycast
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            lineRenderer.SetPosition(0, firePoint.position);


            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, fireRange)) {
                lineRenderer.SetPosition(1, hit.point);

                ShootableBox targetbox = hit.transform.GetComponent<ShootableBox>();

                if (targetbox != null) {
                    targetbox.Damage(gunDamage);
                }
                if (hit.rigidbody != null) {
                    hit.rigidbody.AddForce((hit.point - rayOrigin).normalized * hitForce, ForceMode.Impulse);
                }
            } else {
                lineRenderer.SetPosition(1, fpsCam.transform.forward * fireRange);
            }
        }
    }

    IEnumerator shootingEffect() {
        audioSource.Play();
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(waitTime);
        lineRenderer.enabled = false;
    }
}