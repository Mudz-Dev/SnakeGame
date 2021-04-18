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
    float myCollisionRadius;
    float targetCollisionRadius;

    void Start() {
        body = GetComponent<Body>();
        rb = GetComponent<Rigidbody>();
        myCollisionRadius = GetComponent<SphereCollider>().radius;
        targetCollisionRadius = body.followT.GetComponent<SphereCollider>().radius;
    }


    public void Move(Transform moveToPosition) {
       if(body != null) {
        dis = Vector3.Distance(transform.position, moveToPosition.position);

        Vector3 dirToTarget = (moveToPosition.position - transform.position).normalized;

        Vector3 newPosition = moveToPosition.position;
        newPosition.y = body.player.transform.position.y;
        float T = Time.deltaTime * dis / minDistance * body.player.speed;

        if (T > 0.5f) T = 0.5f;

        //StartCoroutine(MoveBodySlerp(newPosition, T));
        //StartCoroutine(RotateBodySlerp(moveToPosition.rotation, T));
        transform.position = Vector3.Slerp(transform.position, newPosition, T);
        transform.rotation = Quaternion.Slerp(transform.rotation, moveToPosition.rotation, T);
       }
    }

    IEnumerator MoveBodySlerp(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Slerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    IEnumerator RotateBodySlerp(Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = transform.rotation;

        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endValue;
    }

}
