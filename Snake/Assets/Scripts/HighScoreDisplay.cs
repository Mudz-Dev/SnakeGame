using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{

    public Player player;

    TMP_Text scoreText;

    void Start()
    {
        scoreText = GetComponent<TMP_Text>();
    }


    void Update()
    {
        scoreText.text = $"High Score: {player.highScore}";
    }
}
