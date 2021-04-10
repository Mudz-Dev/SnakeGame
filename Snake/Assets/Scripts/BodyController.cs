using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BodyController : MonoBehaviour
{

    Body body;
    float dis;
    float minDistance = 0.50f;
    Rigidbody rb;

    void Start() {
        body = GetComponent<Body>();
        rb = GetComponent<Rigidbody>();
    }


    public void Move(Transform moveToPosition) {
        dis = Vector3.Distance(transform.position, moveToPosition.position);

        Vector3 newPosition = moveToPosition.position;
        newPosition.y = body.player.transform.position.y;
        float T = Time.deltaTime * dis / minDistance * body.player.speed;

        if (T > 0.5f) T = 0.5f;
        transform.position = Vector3.Slerp(transform.position, newPosition, T);
        transform.rotation = Quaternion.Slerp(transform.rotation, moveToPosition.rotation, T);

    }

}
