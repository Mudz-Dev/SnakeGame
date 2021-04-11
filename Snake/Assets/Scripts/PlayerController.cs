using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Body startingBody;
    Vector3 velocity;
    Rigidbody rb;
    Player player;
    List<Body> bodyParts;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        bodyParts = new List<Body>();
    }


    public void Move(Vector3 v) {
        velocity = v;
    }

    public void AddBody(Body bodyToSpawn) {

        Body b;
        b = Instantiate(bodyToSpawn, Vector3.zero, player.transform.rotation) as Body;
    
        b.followT = GetLastBodySpawnPos();
        b.player = player;
        bodyParts.Add(b);
    }

    public Transform GetLastBodySpawnPos() {
        if(bodyParts.Count == 0) {
            return player.transform;
        }
        else {
            return bodyParts[bodyParts.Count - 1].transform;
        }
    }

    public void Update() {
        //rb.MovePosition(rb.position + velocity * Time.deltaTime);
        transform.Translate(velocity * player.speed * Time.smoothDeltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(velocity);
    }

}
