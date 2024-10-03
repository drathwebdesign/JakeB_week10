using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControllerD4 : MonoBehaviour {
    [SerializeField] Camera cam;
    [SerializeField] NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray pointerRay = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(pointerRay, out hit)) {
                agent.SetDestination(hit.point);
            }
        }
    }
}