using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BodySpawnPos))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{

    public enum States {  idle,moving,eating}

    public States currentState;

    public Transform previousPostion;
    public Vector3 direction = Vector3.zero;
    public float speed = 2;
    public Transform followT;
    public int score;
    PlayerController controller;

    public event System.Action OnDeath;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        controller = GetComponent<PlayerController>();
        score = 0;
        currentState = States.idle;
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

        if (currentState == States.idle && direction != Vector3.zero) currentState = States.moving;

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
                //OnDeath();
                StartCoroutine("Die");
                break;
            case "Obstacle":
                StartCoroutine("Die");
                break;
        }
    }

    void EatFood(GameObject foodObject)
    {
        currentState = States.eating;
        Food f = foodObject.GetComponent<Food>();
        Transform lastBodySpawnPos = controller.GetLastBodySpawnPos();
        f.Eat(lastBodySpawnPos.position);
        score += 1;
        controller.AddBody(controller.startingBody);
        currentState = States.moving;
    }

    IEnumerator Die()
    {

        print("Player Died");
        print(Time.timeScale);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }


}
