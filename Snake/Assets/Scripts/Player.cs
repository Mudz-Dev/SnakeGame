using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BodySpawnPos))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{

    public Transform previousPostion;
    public Vector3 direction = Vector3.zero;
    public float speed = 2;
    public Transform followT;
    public int score;
    PlayerController controller;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Movement Input
        if(Input.GetKey(KeyCode.W) && direction.z != -1)
        {
            direction = new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S) && direction.z != 1)
        {
            direction = new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.D) && direction.x != -1)
        {
            direction = new Vector3(1, 0, 0);
        }

        if (Input.GetKey(KeyCode.A) && direction.x != 1)
        {
            direction = new Vector3(-1, 0, 0);
        }

        controller.Move(direction * speed);

        if(Input.GetKeyUp(KeyCode.E))
        {
            controller.AddBody(controller.startingBody);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Food":
                EatFood(other.gameObject);
                break;
            case "Body":

                break;
            case "Obstacle":

                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    void EatFood(GameObject foodObject)
    {
        Food f = foodObject.GetComponent<Food>();
        Transform lastBodySpawnPos = controller.GetLastBodySpawnPos();
        f.Eat(lastBodySpawnPos.position);
        score += 1;
        controller.AddBody(controller.startingBody);
    }


}
