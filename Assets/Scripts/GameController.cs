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
    public GameObject confetti_prefab;
    public Text WinText;
    public GameObject sByeBye;
    public GameObject station_switch_text;

    private HealthCounter health_right;
    private HealthCounter health_left;
    private bool _is_end = false;
    private bool _is_start = false;
    private bool _in_switch_station = false;

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
        health_right = right_sub.GetComponent<HealthCounter>();
        health_left = left_sub.GetComponent<HealthCounter>();
        UpdateHealthBar(left_bar, left_sub);
        UpdateHealthBar(right_bar, right_sub);
        StartCoroutine(StartGame());
        Global.instance.cameraControl = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_is_end || _is_start)
            return;
        UpdateHealthBar(left_bar, left_sub);
        UpdateHealthBar(right_bar, right_sub);

        if (health_right.health <= 0 || health_left.health <= 0)
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
        health_right = right_sub.GetComponent<HealthCounter>();
        health_left = left_sub.GetComponent<HealthCounter>();
        Global.instance.AllPlayersMovementEnable = false;
        Global.instance.isGameEnd = true;

        if (health_right.health < health_left.health)
        {
            WinText.text = "Red Team Win!";
            StartCoroutine(DestroyLoser(right_sub, left_sub));
        }
        else if (health_right.health > health_left.health)
        {
            WinText.text = "Blue Team Win!";
            StartCoroutine(DestroyLoser(left_sub, right_sub));
        }
        else
        {
            WinText.text = "Draw!";
            InputSystemManager.SetVibration(-1, 0.3f, 10f);
            StartCoroutine(FixCamera(left_sub, right_sub, 1f, true));
            StartCoroutine(ReloadScene(12f));
        }
    }

    IEnumerator FixCamera(GameObject loser, GameObject winner, float wait_time, bool isDraw = false)
    {
        Global.instance.GameEndCustomizeScreen = true;
        Vector3 init_pos = Camera.main.transform.parent.position;
        Vector3 target_pos = loser.transform.position;
        target_pos.z = init_pos.z;
        float init_size = Camera.main.orthographicSize;
        float temp = 0f;
        while (temp < 1f)
        {
            temp += Time.deltaTime / 5f;
            Camera.main.transform.parent.position = Vector3.Lerp(init_pos, target_pos, temp);
            Camera.main.orthographicSize = Mathf.Lerp(init_size, 6.4f, temp);
            yield return null;
        }
        yield return new WaitForSeconds(wait_time);
        init_pos = Camera.main.transform.parent.position;
        target_pos = winner.transform.position;
        target_pos.z = init_pos.z;
        temp = 0f;
        if (isDraw)
        {
            GameObject confetti_lose = Instantiate(confetti_prefab, loser.transform);
            Destroy(confetti_lose, 2f);
        }

        while (temp < 1f)
        {
            temp += Time.deltaTime / 3f;
            Camera.main.transform.parent.position = Vector3.Lerp(init_pos, target_pos, temp);
            Camera.main.orthographicSize = Mathf.Lerp(6.4f, 7.5f, temp);
            yield return null;
        }
        GameObject confetti = Instantiate(confetti_prefab, winner.transform);
        Destroy(confetti, 2f);
    }

    private IEnumerator DestroyLoser(GameObject loser, GameObject winner)
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(FixCamera(loser, winner, 5f));
        InputSystemManager.SetVibration(-1, 0.3f, 7f);
        yield return new WaitForSeconds(7f);
        InputSystemManager.SetVibration(-1, 0.9f, 2.7f);
        GameObject temp = Instantiate(sByeBye);
        temp.transform.position = loser.transform.position;
        yield return new WaitForSeconds(1.56f);
        Destroy(loser.transform.parent.gameObject);
        yield return new WaitForSeconds(1.44f);

        SoundManager.instance.PlaySound("win");
        StartCoroutine(ReloadScene(7f));

    }
    IEnumerator ReloadScene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Selection");
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);
        Global.instance.AllPlayersMovementEnable = false;
        Global.instance.bombCreate = false;
        _is_start = true;
        Global.instance.godMode = true;

        left_sub.transform.localPosition = new Vector3(-12.75f, 0f, 0f);
        right_sub.transform.localPosition = new Vector3(12.75f, 0f, 0f);
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

        Rigidbody2D left_rd2 = left_sub.GetComponent<Rigidbody2D>();
        Rigidbody2D right_rd2 = right_sub.GetComponent<Rigidbody2D>();

        left_rd2.AddForce(new Vector2(80f, 0f) * left_rd2.mass, ForceMode2D.Impulse);
        right_rd2.AddForce(new Vector2(-80f, 0f) * left_rd2.mass, ForceMode2D.Impulse);


        while (left_sub.transform.localPosition.x < -2.5f)
        {
            yield return null;
        }


        Global.instance.cameraControl = true;

        Global.instance.godMode = false;
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
        Global.instance.bombCreate = true;
        _is_start = false;
    }

    public IEnumerator SwitchStation()
    {
        if (_in_switch_station)
        {
            yield break;
        }
        _in_switch_station = true;
        Vector3 pos = CameraShakeEffect.instance.transform.position;
        pos.z = 0;
        GameObject stationSwitchText = Instantiate(station_switch_text, pos, Quaternion.identity, transform);
        stationSwitchText.transform.localScale = new Vector3(8f, 8f);
        while (stationSwitchText.transform.localScale.x >= 1)
        {
            float old_value = stationSwitchText.transform.localScale.x;
            stationSwitchText.transform.localScale = new Vector3(old_value - 0.1f, old_value - 0.1f);
            yield return null;
        }
        Destroy(stationSwitchText);
        _in_switch_station = false;
    }
}
