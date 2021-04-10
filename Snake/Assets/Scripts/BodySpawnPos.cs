using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySpawnPos : MonoBehaviour
{
    public Transform previousPosition;

    void Update() {
        previousPosition = transform;
    }

}
