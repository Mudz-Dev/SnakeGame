using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{

    public Player player;

    Text scoreText;

    void Start()
    {
        scoreText = GetComponent<Text>();
    }


    void Update()
    {
        scoreText.text = $"Score: {player.score}";
    }
}
