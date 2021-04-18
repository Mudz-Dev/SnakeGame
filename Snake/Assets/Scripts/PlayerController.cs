using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Body startingBody;
    Vector3 velocity;

    Player player;
    List<Body> bodyParts;
    SphereCollider sc;
    SnakeControls controls;

    void Start()
    {
        player = GetComponent<Player>();
        bodyParts = new List<Body>();
        sc = GetComponent<SphereCollider>();
        
    }

    void Awake() {
        controls = new SnakeControls();

    }

    void OnEnable() {
    controls.PlayerControl.Enable();
    }
    void OnDisable() {
    controls.PlayerControl.Disable();
    }

    void OnMoveUp(InputValue value) {
        print("Move Up");
    }

    public void Move(Vector3 v) {
        velocity = v;
    }

    public void AddBody(Body bodyToSpawn) {

        Transform lastBodyPos = GetLastBodySpawnPos();
        Vector3 spawnPosition = lastBodyPos.position  + (-player.transform.forward * 2);
        Body b;
        b = Instantiate(bodyToSpawn, spawnPosition, player.transform.rotation) as Body;
        
        b.followT = lastBodyPos;
        b.player = player;
        if (bodyParts.Count < 3) b.tag = "Neck";
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
        if (player.currentState != Player.States.idle)
        {
            transform.Translate(velocity * player.speed * Time.smoothDeltaTime, Space.World);
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

}
