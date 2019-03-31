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
    public Text WinText;


    private HealthCounter health_big;
    private HealthCounter health_small;
    private bool _is_end ;

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

    }

    // Update is called once per frame
    void Update()
    {
        InputSystemManager.UpdateGamePad();
        if (_is_end)
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
        //WinText.enabled = true;
        yield return new WaitForSeconds(2f);
        //int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Tutorial");
    }

}
