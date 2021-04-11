using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BodySpawnPos))]
[RequireComponent(typeof(BodyController))]
public class Body : MonoBehaviour
{

    public enum States { idle, spawning, moving }

    public States currentState;

    BodyController controller;
    public Player player;
    public Transform followT;
    public bool isFirstBody;

    void Start()
    {
        controller = GetComponent<BodyController>();
        currentState = States.spawning;
        isFirstBody = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (followT != null)
        {
            controller.Move(followT);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                if (currentState == States.moving)
                {
                    StartCoroutine(Die());
                }
                break;
        }
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