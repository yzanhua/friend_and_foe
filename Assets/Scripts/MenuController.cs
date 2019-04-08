using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject MenuUI;
    public static MenuController instance;


    private GameObject _box;
    private GameObject _option;
    private List<GameObject> _options;
    private int _curr_pos = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        _box = MenuUI.transform.Find("Box").gameObject;
        _option = MenuUI.transform.Find("Option").gameObject;
        _options = new List<GameObject>();


        foreach (Transform child in _option.transform)
        {
            _options.Add(child.gameObject);
        }

        _box.transform.position = _options[_curr_pos].transform.position;
        MenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Stop the scene
        if (InputSystemManager.GetMenuButton(0))
        {
            Time.timeScale = (1f - Time.timeScale);
            if (Time.timeScale == 0f)
            {
                MenuUI.SetActive(true);
            }
            else
            {
                MenuUI.SetActive(false);
            }
        }

        if (InputSystemManager.GetLeftSVertical(0) < -0.5f)
        {
            if (_curr_pos < _options.Count -1)
                _curr_pos ++;
        } else if (InputSystemManager.GetLeftSVertical(0) > 0.5f)
        {
            if (_curr_pos > 0)
                _curr_pos--;
        }

        _box.transform.position = _options[_curr_pos].transform.position;
    }
}
