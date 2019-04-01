using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject left_bar;
    public GameObject right_bar;
    public GameObject right_sub;
    public GameObject left_sub;
    public GameObject ready_text;
    public GameObject go_text;
    public Text WinText;


    private HealthCounter health_big;
    private HealthCounter health_small;
    private bool _is_end = false;
    private bool _is_start = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health_big = right_sub.GetComponent<HealthCounter>();
        health_small = left_sub.GetComponent<HealthCounter>();
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (_is_end || _is_start)
            return;
        UpdateHealthBar(left_bar, left_sub);
        UpdateHealthBar(right_bar, right_sub);

        if (health_big.health <= 0 || health_small.health <= 0)
        {
            GameEnd();
        }
    }

    void UpdateHealthBar(GameObject bar, GameObject sub)
    {
        HealthBar bar_script = bar.GetComponent<HealthBar>();
        HealthCounter health = sub.GetComponent<HealthCounter>();

        bar_script.SetSize((float)health.health / (float)health.maxHealth);

    }

    public void GameEnd()
    {
        _is_end = true;
        health_big = right_sub.GetComponent<HealthCounter>();
        health_small = left_sub.GetComponent<HealthCounter>();

        SoundManager.instance.PlaySound("win");

        if (health_big.health < health_small.health)
        {
            WinText.text = "Red Team Win!";

        }
        else
        {
            WinText.text = "Blue Team Win!";

        }
        StartCoroutine(ReloadScene());

    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Selection");
    }

    IEnumerator StartGame()
    {
        Global.instance.AllPlayersMovementEnable = false;
        _is_start = true;
        GameObject startText = Instantiate(ready_text, transform);
        startText.transform.localScale = new Vector3(10f, 10f);
        startText.transform.position = new Vector3(0f, 4.0f);

        while (startText.transform.localScale.x >= 1)
        {
            float old_value = startText.transform.localScale.x;
            startText.transform.localScale = new Vector3(old_value - 0.2f, old_value - 0.2f);
            yield return null;
            
        }
        Destroy(startText);

        GameObject goText = Instantiate(go_text, transform);
        goText.transform.localScale = new Vector3(10f, 10f);
        goText.transform.position = new Vector3(0f, 4.0f);

        while (goText.transform.localScale.x >= 1)
        {
            float old_value = goText.transform.localScale.x;
            goText.transform.localScale = new Vector3(old_value - 0.2f, old_value - 0.2f);
            yield return null;

        }

        Destroy(goText);
        Global.instance.AllPlayersMovementEnable = true;
        _is_start = false;


    }

}
