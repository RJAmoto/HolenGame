using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControl : MonoBehaviour
{


    /*this script contains the controls, behavior and attributes of the player*/

    public GameObject marble;

    public Transform Camera;
    public Transform attackpoint;

    public GameObject cam;

    public float throwForce;
    public float throwUpwardForce;

    public string playerName;

    public float time = 15;
    public int turn = 15;

    public int score = 0;
    public int throwCount = 1;
    bool isPlaying = false;
    bool hasEnteredCircle = false;

    public CharacterController controller;
    public float speed = 2f;

    public bool endTurn = false;

    public Animator anim;
    public TextMeshProUGUI text;


    void Update()
    {
        if (isPlaying) {
            cam.SetActive(true);

            //function for movement

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);


            //firing marble

            if (Input.GetMouseButtonDown(0)&&throwCount >= 1)
            {
                flick();
            }
        }
        else
        {
            cam.SetActive(false);
        }

        if (throwCount >= 1) {
            timer();
        }
    }

    //the countdown timer that starts at 15 seconds
    public void timer()
    {
        time -= 1 * Time.deltaTime;

        if (time <= 0)
        {
            time = 0;
            endTurn = true;
        }
    }

    /*firing mechanism, it ensures that the player will not fire inside the circle.*/
    public void flick()
    {
        if (hasEnteredCircle)
        {
            text.SetText("Cannot flick inside circle");
            anim.SetTrigger("noty");
        }
        else {
            throwCount -= 1;
            GameObject marbleProjectile = Instantiate(marble, attackpoint.position, Camera.rotation);

            Rigidbody shooter = marbleProjectile.GetComponent<Rigidbody>();

            Vector3 forceDirection = Camera.transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(Camera.position, Camera.forward, out hit, 1000f))
            {
                forceDirection = (hit.point - attackpoint.position).normalized;
            }

            Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

            shooter.AddForce(forceToAdd, ForceMode.Impulse);

            Destroy(marbleProjectile, 3f);

            turn -= 1;
        }
    }

    public void addScore()
    {
        score += 1;
    }

    public void resetThrow()
    {
        throwCount = 1;
        time = 15.5f;
        endTurn = false;
    }

    public bool Endturn()
    {
        return endTurn;
    }

    public void setPlaying(bool playing)
    {
        isPlaying = playing;
    }

    public void OnTriggerEnter(Collider other)
    {
        hasEnteredCircle = true;
    }

    public void OnTriggerExit(Collider other)
    {
        hasEnteredCircle = false;
    }
}
