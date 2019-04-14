//<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolMovement : MonoBehaviour
{
    public int StartPos;
    public int DestPos;
    public float Speed = 2.0f;
    public float WaitTime;
    public GameObject[] fishes;

    //Vector3[] PresetPosition = { new Vector3(-21, 3, 0), new Vector3(16, 3, 0),
    //new Vector3(-21, -3, 0), new Vector3(16, -3, 0)};
    Vector3[] PresetPosition;
    int fishCount = 7;
    bool traveling;
    bool inTravelRountine;
    bool inWaitRoutine;
    float orgScale;
    Animator[] fishAnimators;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        PresetPosition = new Vector3[4];
        fishAnimators = new Animator[fishes.Length];
        for (int i = 0; i < transform.childCount - 1; ++i)
        {
            fishAnimators[i] = transform.GetChild(i).GetComponent<Animator>();
            orgScale = transform.GetChild(i).localScale.x;
        }
        rb = GetComponent<Rigidbody2D>();
        StartPos = 0;
        DestPos = 1;
        travel();
    }

    // Update is called once per frame
    void Update()
    {
        // Stop when reach destination
        if (Mathf.Abs(transform.position.x - PresetPosition[DestPos].x) < 0.1f && !inWaitRoutine)
        {
            for (int i = 0; i < fishes.Length; ++i)
            {
                // if the fish is still active
                if (transform.GetChild(i))
                {
                    fishAnimators[i].speed = 0;
                    fishAnimators[i].SetBool("moving", false);
                }
            }
            rb.velocity = Vector2.zero;
            StartCoroutine(Wait());
        }
        else if (!inWaitRoutine)
        {
            rb.velocity = (PresetPosition[DestPos] - PresetPosition[StartPos]).normalized * Speed;
        }
    }

    void travel()
    {
        traveling = true;
        Vector3 camCenter = CameraShakeEffect.instance.transform.position;
        float hHalfScreen = Global.instance.maxScreenSize * 2f;
        float vHalfScreen = Global.instance.maxScreenSize / 16 * 9 * 0.7f;
        PresetPosition[0] = new Vector3(camCenter.x - hHalfScreen, camCenter.y + vHalfScreen, 0);
        PresetPosition[1] = new Vector3(camCenter.x + hHalfScreen, camCenter.y + vHalfScreen, 0);
        PresetPosition[2] = new Vector3(camCenter.x - hHalfScreen, camCenter.y - vHalfScreen, 0);
        PresetPosition[3] = new Vector3(camCenter.x + hHalfScreen, camCenter.y - vHalfScreen, 0);

        transform.position = PresetPosition[StartPos];
        for (int i = 0; i < fishes.Length; ++i)
        {
            if (transform.GetChild(i))
            {
                fishAnimators[i].speed = 1;
                fishAnimators[i].SetBool("moving", true);
            }
        }
        rb.velocity = (PresetPosition[DestPos] - PresetPosition[StartPos]).normalized * Speed;
    }

    // Adjust fish count and deactive the killed fish
    public void KillFish()
    {
        fishCount--;
        //fishes[fishCount].SetActive(false);
        if (fishCount == 0)
        {
            StartCoroutine(Wait());
            transform.GetChild(fishes.Length).gameObject.SetActive(false);
        }
    }


    // Wait for a period of time and reset school
    IEnumerator Wait()
    {
        inWaitRoutine = true;
        yield return new WaitForSeconds(WaitTime);
        StartPos = new System.Random().Next(0, 4);
        DestPos = getDestination(StartPos);
        fishCount = fishes.Length;
        for (int i = 0; i < fishes.Length; ++i)
        {
            fishes[i].SetActive(true);
        }
        transform.GetChild(fishes.Length).gameObject.SetActive(true);
        // rotate the school to the correct direction
        if ((StartPos == 1 || StartPos == 3) && transform.localScale.x != -3.0f)
        {
            for (int i = 0; i < fishes.Length; ++i)
            {
                transform.GetChild(i).localScale = new Vector3(-1, 1, 1) * orgScale;
            }
            //set particle
            int index = transform.childCount - 1;
            transform.position = new Vector2(3.64f, -1.23f);
            ParticleSystem ps = transform.GetChild(index).GetComponent<ParticleSystem>();
            var sh = ps.shape;
            sh.rotation = new Vector3(sh.rotation.x, 90, sh.rotation.y);
            ps = transform.GetChild(index).GetChild(0).GetComponent<ParticleSystem>();
            sh = ps.shape;
            sh.rotation = new Vector3(sh.rotation.x, 90, sh.rotation.y);
        }
        if ((StartPos == 0 || StartPos == 2) && transform.localScale.x != 3.0f)
        {
            for (int i = 0; i < fishes.Length; ++i)
            {
                transform.GetChild(i).localScale = new Vector3(1, 1, 1) * orgScale;
            }
            //set particle
            int index = transform.childCount - 1;
            transform.position = new Vector2(2.18f, -1.23f);
            ParticleSystem ps = transform.GetChild(index).GetComponent<ParticleSystem>();
            var sh = ps.shape;
            sh.rotation = new Vector3(sh.rotation.x, -90, sh.rotation.y);
            ps = transform.GetChild(index).GetChild(0).GetComponent<ParticleSystem>();
            sh = ps.shape;
            sh.rotation = new Vector3(sh.rotation.x, -90, sh.rotation.y);
        }
        travel();
        inWaitRoutine = false;
    }

    // Given the start position of school, return the corresponding destination
    int getDestination(int start)
    {
        switch (start)
        {
            case 0:
                return 1;
            case 1:
                return 0;
            case 2:
                return 3;
            case 3:
                return 2;
            default:
                Debug.LogWarning("WARNING: No such school start position available");
                return 4;
        }
    }
}
