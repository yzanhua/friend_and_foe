using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolMovement : MonoBehaviour
{
    public Vector3 PausePosition;
    public Vector3 LeavePosition;
    public float speed = 2.0f;
    public float pauseTime = 10.0f;

    Animator[] fishAnimators = new Animator[11];
    bool approaching = true;
    bool leaving = false;
    bool inStopRountine = false;
    // Start is called before the first frame update
    void Start()
    {
        //print(transform.childCount);
        for (int i = 0; i < transform.childCount; ++i)
        {
            //if (!transform.GetChild(i).GetComponent<Animator>())
                //print(i + " th child has no animator");
            fishAnimators[i] = transform.GetChild(i).GetComponent<Animator>();
            //print(fishAnimators[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - PausePosition).magnitude < 0.3f && !inStopRountine)
        {
            //for (int i = 0; i < fishAnimators.Length; ++i)
            //{
            //    fishAnimators[i].speed = 0;
            //    fishAnimators[i].SetBool("moving", false);
            //}
            StartCoroutine(pause());
        }
        //else if ((transform.position - LeavePosition).magnitude < 1)
        //{
        //    leaving = false;
        //}

        if (approaching)
        {
            //print(fishAnimators.Length);
            for (int i = 0; i < fishAnimators.Length; ++i)
            {
                fishAnimators[i].speed = 1;
                fishAnimators[i].SetBool("moving", true);
            }
            transform.position = Vector3.Lerp(transform.position, PausePosition, Time.deltaTime * speed);
        }
        // moving to target position

        else
        // leaving
        {
            for (int i = 0; i < fishAnimators.Length; ++i)
            {
                fishAnimators[i].speed = 1;
                fishAnimators[i].SetBool("moving", true);
            }
            transform.position = Vector3.Lerp(transform.position, LeavePosition, Time.deltaTime * speed);

        }

    }

    IEnumerator pause()
    {
        inStopRountine = true;
        //approaching = false;
        yield return new WaitForSeconds(pauseTime);        
        approaching = false;
        //leaving = true;
        inStopRountine = false;
    }

    public void Move()
    {
        //Vector3.Lerp(transform.position, PausePosition, moveTime);

    }
}
