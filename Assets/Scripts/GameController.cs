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
    public GameObject big_sub;
    public GameObject small_sub;
    public Text WinText;

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
        SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar(left_bar, small_sub);
        UpdateHealthBar(right_bar, big_sub);
    }

    void UpdateHealthBar(GameObject bar, GameObject sub)
    {
        HealthBar bar_script = bar.GetComponent<HealthBar>();
        HealthCounter health = sub.GetComponent<HealthCounter>();

        bar_script.SetSize((float)health.health / (float)health.maxHealth);

    }

    public void GameEnd()
    {
        HealthCounter health_big = big_sub.GetComponent<HealthCounter>();
        HealthCounter health_small = small_sub.GetComponent<HealthCounter>();

        if (health_big.health < health_small.health)
        {
            WinText.text = "Red Team Win!";
            WinText.enabled = true;
        }
        else
        {
            WinText.text = "Blue Team Win!";
            WinText.enabled = true;
        }
        StartCoroutine(ReloadScene());
        
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(1);
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

}
