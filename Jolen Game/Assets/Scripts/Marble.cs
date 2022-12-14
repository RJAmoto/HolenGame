using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    float lifetime = 1.2f;

    bool exit = false;

    bool scored = false;
   
    void Update()
    {
        if (exit)
        {
            lifetime -= 1 * Time.deltaTime;

            if (lifetime<=0)
            {
                lifetime = 0;
                scored = true;
                exit = false;
                Destroy(gameObject);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        exit = true;
    }


    public bool hasScored()
    {
        return scored;
    }

    public void ResetScore()
    {
        scored = false;
    }

    public bool exited() {
        return exit;
    }
}
