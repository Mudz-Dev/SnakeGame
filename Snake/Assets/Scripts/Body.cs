using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BodySpawnPos))]
[RequireComponent(typeof(BodyController))]
public class Body : MonoBehaviour
{

    BodyController controller;
    public Player player;
    public Transform followT;

    void Start()
    {
        controller = GetComponent<BodyController>();
    }

    // Update is called once per frame
    void Update()
    {

        if(followT != null) {
            controller.Move(followT);
        }

    }
}
