using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Body startingBody;
    public float jumpForce = 7;
    Vector3 velocity;
    public float gravity = -12;

    Player player;
    List<Body> bodyParts;
    SphereCollider sc;
    SnakeControls controls;
    Rigidbody rb;
    float velocityY;


    void Start()
    {
        player = GetComponent<Player>();
        bodyParts = new List<Body>();
        sc = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }

    void Awake() {
        controls = new SnakeControls();
        controls.PlayerControls.MoveUp.performed += x => {if(player.direction.z != -1) player.direction = Vector3.forward;};
        controls.PlayerControls.MoveDown.performed += x => {if(player.direction.z != 1) player.direction = Vector3.back;};
        controls.PlayerControls.MoveRight.performed += x => {if(player.direction.x != -1) player.direction = Vector3.right;};
        controls.PlayerControls.MoveLeft.performed += x => {if(player.direction.x != 1) player.direction = Vector3.left;};
        controls.PlayerControls.Jump.performed += x => { rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); };

        

        controls.DebugControls.AddBody.performed += x => {AddBody(startingBody);};
    }

    void OnEnable() {
        controls.Enable();
    }
    void OnDisable() {
        controls.Disable();
    }

    void TogglePause() {
        if(Time.timeScale == 1) { 
            Time.timeScale = 0;
        } 
        else { 
            Time.timeScale = 1;
        }
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
