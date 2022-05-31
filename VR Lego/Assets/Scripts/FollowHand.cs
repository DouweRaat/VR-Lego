using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHand : MonoBehaviour
{
    //Rigidbody rb;
    public Transform controller;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        transform.position = (LegoLogic.SnapToGrid(controller.transform.position));
    }
}
