using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GamePlay : MonoBehaviour
{

    public Marble[] marbles;
    public PlayerControl[] players;

    public Animator anim;

    //UI Texts
    public TextMeshProUGUI turns;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI score;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI notifs;

    public GameObject components;

    public string winnerName;
    public int winnerScore;
    public bool gameOver;
    public bool draw;

    bool clear = true;

    public int turnCount;

    public float countdown = 5;
    // Start is called before the first frame update
    void Start()
    {
        notifs.fontSize = 0;
        gameOver = false;
        draw = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver) {
            for (int a = 0; a < players.Length; a++)
            {
                if (a != turnCount)
                {
                    players[a].setPlaying(false);
                }
                else
                {
                    players[turnCount].setPlaying(true);
                }


                if (players[a].turn <= 0 && !gameOver)
                {
                    playerScores();
                    gameOver = true;
                    if (!draw) {
                        notifs.SetText(winnerName + " is the Winner, scored " + winnerScore);
                        notifs.fontSize = 36;
                        anim.SetTrigger("win");
                        components.SetActive(false);
                    }
                    else
                    {
                        notifs.SetText("Draw");
                        notifs.fontSize = 36;
                        anim.SetTrigger("win");
                        components.SetActive(false);
                    }
                    


                }
            }

            for (int b = 0; b < marbles.Length; b++)
            {
                if (marbles[b].hasScored())
                {
                    players[turnCount].addScore();
                    marbles[b].ResetScore();
                    clear = true;

                    players[turnCount].endTurn = false;
                    countdown = 5;
                    players[turnCount].resetThrow();

                    notifs.SetText(players[turnCount].playerName + " Has scored, turn again");
                    notifs.fontSize = 20;
                    anim.SetTrigger("noty");

                }

                if (marbles[b].exited())
                {
                    clear = false;
                }

            }

            if (players[turnCount].endTurn)
            {
                turnCount += 1;
                countdown = 5;

                if (turnCount >= players.Length)
                {
                    turnCount = 0;
                }

                StopMarbles();
                notifs.fontSize = 36;
                notifs.SetText(players[turnCount].playerName + " turn");
                anim.SetTrigger("noty");
                players[turnCount].resetThrow();
            }


            if (players[turnCount].throwCount <= 0 && clear) {

                countdown -= 1 * Time.deltaTime;

                if (countdown <= 0)
                {
                    countdown = 0;

                    players[turnCount].endTurn = true;
                }

            }

            if ((int)players[turnCount].time <= 5)
            {
                timer.color = Color.red;
            }
            else
            {
                timer.color = Color.white;
            }

            timer.SetText("Time: " + (int)players[turnCount].time);
            turns.SetText("Turns: " + players[turnCount].turn);
            score.SetText("Score: " + players[turnCount].score);
            playerName.SetText(" " + players[turnCount].playerName + " ");

        }

    }

    public void StopMarbles()
    {
        for (int b = 0; b < marbles.Length; b++)
        {
            if (marbles[b] != null)
            {
                marbles[b].GetComponent<Rigidbody>().velocity = Vector3.zero;
                marbles[b].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                marbles[b].GetComponent<Rigidbody>().drag = 0;
            }

        }
    }

    public void playerScores()
    {
        int score = players[0].score;
        string name = players[0].playerName;


        for (int a = 0; a < players.Length; a++)
        {
            if (players[a].score > score)
            {
                score = players[a].score;
                name = players[a].playerName;
            }
            else if (players[a].score == score)
            {
                draw = true;
            }
        }

        winnerName = name;
        winnerScore = score;


    }
}
