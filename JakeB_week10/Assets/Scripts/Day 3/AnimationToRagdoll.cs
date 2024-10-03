using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToRagdoll : MonoBehaviour {
    [SerializeField] Collider myCollider;
    [SerializeField] float respawnTime = 30f;
    [SerializeField] float decapitationForceThreshold = 5f;
    [SerializeField] GameObject head;
    Rigidbody[] rigidbodies;
    bool bIsRagdoll = false;
    Rigidbody headRigidbody;
    CharacterJoint headJoint;

    void Start() {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        headRigidbody = head.GetComponent<Rigidbody>();
        headJoint = head.GetComponent<CharacterJoint>();
        ToggleRagdoll(true);
    }

    private void OnCollisionEnter(Collision collision) {
        if (!bIsRagdoll && collision.gameObject.tag == "Projectile") {
            ToggleRagdoll(false);

            // Calculate the collision impact force
            float impactForce = collision.impulse.magnitude;

            if (impactForce >= decapitationForceThreshold && headJoint != null) {
                DecapitateHead(impactForce);
            }

            StartCoroutine(GetBackUp());
        }
    }

    private void ToggleRagdoll(bool bisAnimating) {
        bIsRagdoll = !bisAnimating;
        myCollider.enabled = bisAnimating;

        foreach (Rigidbody ragdollBone in rigidbodies) {
            ragdollBone.isKinematic = bisAnimating;
        }
    }

    private IEnumerator GetBackUp() {
        yield return new WaitForSeconds(respawnTime);
        ToggleRagdoll(true);
    }

    private void DecapitateHead(float force) {
        Destroy(headJoint);

        head.transform.parent = null;

        Vector3 forceDirection = headRigidbody.transform.position - transform.position;
        headRigidbody.AddForce(forceDirection.normalized * force, ForceMode.Impulse);
    }
}