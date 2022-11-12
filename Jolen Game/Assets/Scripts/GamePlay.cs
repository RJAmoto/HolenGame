using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GamePlay : MonoBehaviour
{
    /*the function of this script 
     is to analyze all the events
    and controls the overall gameplay*/

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

    string winnerName;
    int winnerScore;

    bool gameOver;
    bool draw;

    bool clear = true;

    int turnCount;

    float countdown = 5;
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
                // checks all the players and sets who is playing
                if (a != turnCount)
                {
                    players[a].setPlaying(false);
                }
                else
                {
                    players[turnCount].setPlaying(true);
                }


                /*when one of the players has no turns left,
                 the script will check who is the winner 
                 and if it is draw It will create an animation
                 on the screen to show who is the winner*/

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

            /*checks if one of the marbles are outside the circle,
             it determines if a player has scored*/

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
            /*checks if a player has triggered an endturn,
             an endturn is triggered when a player is already finished
             at his turn or the time is over for his turn*/

            if (players[turnCount].endTurn)
            {
                players[turnCount].turn -= 1;
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

                /*countdown after a player has thrown his marble
                  and waits if it has hit something, 
                 if no score was added after the throw the turn 
                 will end*/
            if (players[turnCount].throwCount <= 0 && clear) {

                countdown -= 1 * Time.deltaTime;

                if (countdown <= 0)
                {
                    countdown = 0;

                    players[turnCount].endTurn = true;
                }
                
            }

            //sets the color of the timer, to red if 5 seconds is left

            if ((int)players[turnCount].time <= 5)
            {
                timer.color = Color.red;
            }
            else
            {
                timer.color = Color.white;
            }

            //updates the things that are written on the ui

            timer.SetText("Time: " + (int)players[turnCount].time);
            turns.SetText("Turns: " + players[turnCount].turn);
            score.SetText("Score: " + players[turnCount].score);
            playerName.SetText(" " + players[turnCount].playerName + " ");

        }

    }

    /*this function stops the marbles when called, 
     this is to ensure that no outside forces are 
     moving the ball once a new player is playing*/
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

    /*this function determines 
     who has the highest score
     and sets draw if players
     have equal scores*/
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
