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
    public bool isEating;
    public int score;
    PlayerController controller;
    SphereCollider sc;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        controller = GetComponent<PlayerController>();
        score = 0;
        currentState = States.idle;
        sc = GetComponent<SphereCollider>();
        isEating = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Movement Input


        if (currentState == States.idle && direction != Vector3.zero) currentState = States.moving;

        controller.Move(direction * speed);




    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Food":
                if(!isEating) StartCoroutine(EatFood(other.gameObject));
                break;
            case "Body":
                speed = 0f;
                StartCoroutine("Die");
                break;
            case "Obstacle":
                speed = 0f;
                StartCoroutine("Die");
                break;
        }
    }

    IEnumerator EatFood(GameObject foodObject)
    {
        isEating = true;
        IncreaseSpeed();
        currentState = States.eating;
        Food f = foodObject.GetComponent<Food>();
        Transform lastBodySpawnPos = controller.GetLastBodySpawnPos();

        f.Eat(lastBodySpawnPos.position);
        score += 1;
        controller.AddBody(controller.startingBody);
        currentState = States.moving;
        yield return new WaitForSeconds(0.25f);        
        isEating = false;       
    }

    IEnumerator Die()
    {

        print("Player Died");
        print(Time.timeScale);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    void IncreaseSpeed() {
        speed += 0.1f;
        speed = Mathf.Clamp(speed, 2, 3);
    }


}
