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
    public LayerMask foodCollisionMask;

    public int highScore {
        get {
            return PlayerPrefs.GetInt("ClassicHighScore", 0);
        }
        set {
            PlayerPrefs.SetInt("ClassicHighScore", value);
        }
    }

    PlayerController controller;
    SphereCollider sc;
    AudioSource eatAudio;
    Animation eatAnim;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        controller = GetComponent<PlayerController>();
        score = 0;
        currentState = States.idle;
        sc = GetComponent<SphereCollider>();
        isEating = false;
        eatAudio = GetComponent<AudioSource>();
        
        eatAnim = GetComponentInChildren<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Movement Input

        if (currentState == States.idle && direction != Vector3.zero) currentState = States.moving;


        controller.Move(direction * speed);
        CheckFoodCollision();
 
    }

    void CheckFoodCollision()
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 2, foodCollisionMask, QueryTriggerInteraction.Collide))
        {
            PlayEatAnimation();
        }
    }

    public void PlayEatAnimation()
    {
        eatAnim.Play();
    }

    private void OnTriggerEnter(Collider other)
    {

        switch (other.gameObject.tag)
        {
            case "Food":
                if (!isEating) StartCoroutine(EatFood(other.gameObject));
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
        if(!eatAudio.isPlaying) {
            eatAudio.pitch = Random.Range(1f, 2f);
            eatAudio.Play();
        }
            
        isEating = true;
        IncreaseSpeed();
        currentState = States.eating;
        Food f = foodObject.GetComponent<Food>();
        Transform lastBodySpawnPos = controller.GetLastBodySpawnPos();

        f.Eat(lastBodySpawnPos.position);
        score += f.points;
        if(score > highScore) highScore = score;
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
