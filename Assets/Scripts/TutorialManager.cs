using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeautifulTransitions.Scripts.Transitions.Components;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    
    public enum State
    {
        SKIPTUTORIAL,
        CHARACTER,
        MOVEMENT,
        WEAPON,
        SHIELD,
        PRE_FINISHED,
        FINISHED,
        STATE
    };


    public enum TaskType
    {
        DASH,
        JUMP,
        SEAT,
        MOVE,
        DASH_SUB,
        REFILL,
        SHOOT,
        SHIELD,
        TASK
    }


    public bool tutorialMode = false;

    public GameObject RightSubmarine;
    public GameObject LeftSubmarine;
    public GameObject PreparedText;

    public static int leftTutorialState = 0;
    public static int rightTutorialState = 0;
    public State state;

    public delegate void CallBack();

    private bool _isStartingGame = false;

    // private Hashtable _rightTaskList = new Hashtable();
    // private Hashtable _leftTaskList = new Hashtable();


    public List<Hashtable> _TaskList = new List<Hashtable>(2);
    public bool[] _TaskState = new bool[2];

    // private bool _leftTaskState = false;
    // private bool _rightTaskState = false;


    private GameObject _leftGear;
    private GameObject _leftStaticGear;
    private GameObject _rightGear;
    private GameObject _rightStaticGear;
    private GameObject _leftRefill;
    private GameObject _rightRefill;
    private GameObject _leftStaticRefill;
    private GameObject _rightStaticRefill;

    private Hashtable _startMap = new Hashtable();
    private Hashtable _skipMap = new Hashtable();

    private bool _InStateTransition = false;




    static public bool CompleteTask(TaskType task, bool isRight)
    {
        if (!instance.tutorialMode || instance._InStateTransition)
            return false;


        int pos = isRight ? 1 : 0;

        if (instance.state == State.CHARACTER)
        {
            if (task == TaskType.DASH)
            {
                instance._TaskList[pos][task] = true;
                // instance._rightTaskBox.text = "Press Button A to jump from the ladder";

                if ((bool)instance._TaskList[pos][TaskType.DASH])
                {
                    // instance._rightTaskBox.text = "Great!";
                    instance._TaskState[pos] = true;
                }

                return true;
            }
        }
        else if (instance.state == State.MOVEMENT)
        {
            if (task == TaskType.SEAT || task == TaskType.MOVE || task == TaskType.DASH_SUB)
            {

                if (task == TaskType.SEAT)
                {
                    if (((bool)instance._TaskList[0][TaskType.SEAT] && (bool)instance._TaskList[1][TaskType.SEAT]))
                    {
                        return true;
                    }

                    int num = 0;

                    instance._TaskList[pos][task] = true;
                    if ((bool)instance._TaskList[0][TaskType.SEAT] && (bool)instance._TaskList[1][TaskType.SEAT])
                    {
                        num = 2;
                        instance._InStateTransition = true;
                        instance.StartCoroutine(instance.ChangeState(1.5f, () =>
                        {
                            instance._InStateTransition = false;
                            instance.EnableText("Move");
                        }));

                    }
                    else if ((bool)instance._TaskList[0][TaskType.SEAT] || (bool)instance._TaskList[1][TaskType.SEAT])
                    {
                        num = 1;
                    }

                    GameObject num_text = instance.PreparedText.transform.Find("Seat").Find("flash_text").gameObject;
                    num_text.transform.Find("seat_num").gameObject.GetComponent<Text>().text = num + " / 2";
                }
                else if (task == TaskType.MOVE)
                {
                    if (!((bool)instance._TaskList[0][TaskType.SEAT] && (bool)instance._TaskList[1][TaskType.SEAT]))
                    {
                        return false;
                    }
                    else if (((bool)instance._TaskList[0][TaskType.MOVE] && (bool)instance._TaskList[1][TaskType.MOVE]))
                    {
                        return true;
                    }
                    else
                    {
                        instance._TaskList[pos][task] = true;
                        int num = 0;


                        if ((bool)instance._TaskList[0][TaskType.MOVE] && (bool)instance._TaskList[1][TaskType.MOVE])
                        {
                            num = 2;
                            instance._InStateTransition = true;
                            instance.StartCoroutine(instance.ChangeState(2f, () =>
                            {
                                instance._InStateTransition = false;
                                instance.EnableText("DashSub");
                            }));
                        }
                        else if ((bool)instance._TaskList[0][TaskType.MOVE] || (bool)instance._TaskList[1][TaskType.MOVE])
                        {
                            num = 1;
                        }
                        GameObject num_text = instance.PreparedText.transform.Find("Move").Find("flash_text").gameObject;
                        num_text.transform.Find("move_num").gameObject.GetComponent<Text>().text = num + " / 2";

                    }
                }
                else if (task == TaskType.DASH_SUB)
                {
                    if (!((bool)instance._TaskList[0][TaskType.MOVE] && (bool)instance._TaskList[1][TaskType.MOVE]))
                    {
                        return false;
                    }
                    else if (((bool)instance._TaskList[0][TaskType.DASH_SUB] && (bool)instance._TaskList[1][TaskType.DASH_SUB]))
                    {
                        return true;
                    }
                    else
                    {
                        instance._TaskList[pos][task] = true;
                        int num = 0;

                        if ((bool)instance._TaskList[0][TaskType.DASH_SUB] && (bool)instance._TaskList[1][TaskType.DASH_SUB])
                        {
                            num = 2;
                        }
                        else if ((bool)instance._TaskList[0][TaskType.DASH_SUB] || (bool)instance._TaskList[1][TaskType.DASH_SUB])
                        {
                            num = 1;
                        }
                        GameObject num_text = instance.PreparedText.transform.Find("DashSub").Find("flash_text").gameObject;
                        num_text.transform.Find("dash_num").gameObject.GetComponent<Text>().text = num + " / 2";

                    }
                }


                if ((bool)instance._TaskList[pos][TaskType.SEAT] && (bool)instance._TaskList[pos][TaskType.MOVE] && (bool)instance._TaskList[pos][TaskType.DASH_SUB])
                {
                    //instance.rightTaskBox.text = "Great!";
                    instance._TaskState[pos] = true;
                }

                return true;
            }

        }
        else if (instance.state == State.WEAPON)
        {
            if (task == TaskType.REFILL || task == TaskType.SHOOT)
            {
                if (task == TaskType.REFILL)
                {
                    if (((bool)instance._TaskList[0][TaskType.REFILL] && (bool)instance._TaskList[1][TaskType.REFILL]))
                    {
                        return true;
                    }

                    int num = 0;
                    instance._TaskList[pos][task] = true;
                    if ((bool)instance._TaskList[0][TaskType.REFILL] && (bool)instance._TaskList[1][TaskType.REFILL])
                    {
                        num = 2;
                        instance._InStateTransition = true;
                        instance.StartCoroutine(instance.ChangeState(1.5f, () =>
                        {
                            instance._InStateTransition = false;
                            instance.EnableText("Shoot");
                        }));

                    }
                    else if ((bool)instance._TaskList[0][TaskType.REFILL] || (bool)instance._TaskList[1][TaskType.REFILL])
                    {
                        num = 1;
                    }

                    GameObject num_text = instance.PreparedText.transform.Find("Refill").Find("flash_text").gameObject;
                    num_text.transform.Find("num").gameObject.GetComponent<Text>().text = num + " / 2";
                }
                else if (task == TaskType.SHOOT)
                {
                    if (!((bool)instance._TaskList[0][TaskType.REFILL] && (bool)instance._TaskList[1][TaskType.REFILL]))
                    {
                        return false;
                    }
                    else if (((bool)instance._TaskList[0][TaskType.SHOOT] && (bool)instance._TaskList[1][TaskType.SHOOT]))
                    {
                        return true;
                    }
                    else
                    {
                        instance._TaskList[pos][task] = true;
                    }
                }

                if ((bool)instance._TaskList[pos][TaskType.REFILL] && (bool)instance._TaskList[pos][TaskType.SHOOT])
                {
                    instance._TaskState[pos] = true;
                }

                return true;
            }

        }
        else if (instance.state == State.SHIELD)
        {
            if (task == TaskType.SHIELD)
            {
                instance._TaskList[pos][task] = true;
                int num = 0;

                if ((bool)instance._TaskList[0][TaskType.SHIELD] && (bool)instance._TaskList[1][TaskType.SHIELD])
                {
                    num = 2;
                }
                else if ((bool)instance._TaskList[0][TaskType.SHIELD] || (bool)instance._TaskList[1][TaskType.SHIELD])
                {
                    num = 1;
                }
                GameObject num_text = instance.PreparedText.transform.Find("Shield").Find("flash_text").gameObject;
                num_text.transform.Find("num").gameObject.GetComponent<Text>().text = num + " / 2";
            }

            if ((bool)instance._TaskList[pos][TaskType.SHIELD])
            {
                //instance.rightTaskBox.text = "Great!";
                instance._TaskState[pos] = true;
            }

            return true;

        }

        return false;
    }



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
        // leftTaskBox.text = "Press B to start the tutorial.";
        // rightTaskBox.text = "Press A to skip the tutorial";
        Global.instance.AllPlayersMovementEnable = false;
        state = State.SKIPTUTORIAL;
        _TaskList.Add(new Hashtable());
        _TaskList.Add(new Hashtable());

        for (int i = 0; i < (int) TaskType.TASK; i++)
        {
            _TaskList[0][(TaskType)i] = false;
            _TaskList[1][(TaskType)i] = false;
        }

        for (int i = 0; i < 4; i++)
        {
            _skipMap[i] = false;
            _startMap[i] = false;
        }

        GameObject leftSubmarineProxy = LeftSubmarine.transform.Find("Submarine_proxy").gameObject;
        GameObject leftSubmarineStatic = LeftSubmarine.transform.Find("Submarine_static").gameObject;
        GameObject rightSubmarineProxy = RightSubmarine.transform.Find("Submarine_proxy").gameObject;
        GameObject rightSubmarineStatic = RightSubmarine.transform.Find("Submarine_static").gameObject;

        _leftGear = leftSubmarineProxy.transform.Find("Gear").gameObject;
        _leftStaticGear = leftSubmarineStatic.transform.Find("Gear").gameObject;
        _leftRefill = leftSubmarineProxy.transform.Find("refillStation").gameObject;
        _leftStaticRefill = leftSubmarineStatic.transform.Find("refillStation").gameObject;

        _rightGear = rightSubmarineProxy.transform.Find("Gear").gameObject;
        _rightStaticGear = rightSubmarineStatic.transform.Find("Gear").gameObject;
        _rightRefill = rightSubmarineProxy.transform.Find("refillStation").gameObject;
        _rightStaticRefill = rightSubmarineStatic.transform.Find("refillStation").gameObject;

        LeftSubmarine.SetActive(false);
        RightSubmarine.SetActive(false);
        AlterChangeSceneState(false);
        AlterGearState(false);
        EnableText("SkipTutorial");

    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialMode && !_isStartingGame)
        {
            MoveToNextState();
        }

    }

    void MoveToNextState()
    {
        if (state == State.SKIPTUTORIAL)
        {
            for (int i = 0; i < 4; i++)
            {
                bool a3_value = InputSystemManager.GetAction3(i);
                bool a4_value = InputSystemManager.GetAction4(i);

                if (a3_value)
                {
                    _startMap[i] = true;
                    _skipMap[i] = false;

                } else if (a4_value)
                {
                    _skipMap[i] = true;
                    _startMap[i] = false;
                }

 
            }

            int start_num = 0;
            int skip_num = 0;

            for (int i = 0; i < 4; i++)
            {
                if ((bool)_skipMap[i]) skip_num++;
                else if ((bool)_startMap[i]) start_num++;
            }

            if (skip_num == 1)
            { 
                state = State.PRE_FINISHED;
            }
            else if (start_num == 1)
            {
                StartCoroutine(StartTutorialTransition());
            }

            GameObject num_text = PreparedText.transform.Find("SkipTutorial").Find("flash_text").gameObject;

            num_text.transform.Find("start_num").gameObject.GetComponent<Text>().text = start_num + " / 4";
            num_text.transform.Find("skip_num").gameObject.GetComponent<Text>().text = skip_num + " / 4";
        } else if (state == State.PRE_FINISHED)
        {
            Global.instance.AllPlayersMovementEnable = true;
            TransitionManager.Instance.TransitionOutAndLoadScene("Game");
        } else if (_TaskState[0] && _TaskState[1])
        {
            if (state == State.CHARACTER)
            {
                StartCoroutine(ChangeState(2f, () =>
                {
                    state = State.MOVEMENT;
                    AlterGearState(true);
                    ResetTaskState();
                    EnableText("Seat");
                    EnableGear("MovementGear");
                }));


            } else if (state == State.MOVEMENT)
            {
            

                StartCoroutine(ChangeState(3f, () =>
                {
                    state = State.SHIELD;
                    ResetTaskState();
                    EnableText("Shield");
                    EnableGear("ShieldGear");
                }));

                ResetTaskState();
            } else if (state == State.WEAPON)
            {
                state = State.FINISHED;
                _isStartingGame = true;
                StartCoroutine(ChangeState(5f, () =>
                {
                    StartCoroutine(StartTheGame());
                }));

            } else if (state == State.SHIELD)
            {
                StartCoroutine(ChangeState(3f, () =>
                {
                    state = State.WEAPON;
                    ResetTaskState();

                    EnableText("Refill");
                    EnableGear("WeaponGear");
 
                    _leftStaticRefill.gameObject.SetActive(true);
                    _rightStaticRefill.gameObject.SetActive(true);
                    _leftRefill.gameObject.SetActive(true);
                    _rightRefill.gameObject.SetActive(true);
                }));


            }
        }

    }

    IEnumerator StartTheGame()
    {
        EnableText("Finished");
        yield return new WaitForSeconds(3f);
        TransitionManager.Instance.TransitionOutAndLoadScene("Game");
    }


    IEnumerator ChangeState(float delay, CallBack cb)
    {
        yield return new WaitForSeconds(delay);
        cb();
    }

    IEnumerator StartTutorialTransition()
    {
        Vector3 target_position = new Vector3(0f, 3.5f, -10f);
        Transform container_transform = Camera.main.transform.parent;

        LeftSubmarine.SetActive(true);
        RightSubmarine.SetActive(true);

        while ((container_transform.position - target_position).magnitude > 0.01f)
        {
            container_transform.position += (target_position - container_transform.position) * Time.deltaTime * 0.2f;
            yield return null;
        }


        Global.instance.AllPlayersMovementEnable = true;
        state = State.CHARACTER;
        EnableText("Dash");


    }

    private void AlterGearState(bool value)
    {
        //
        _leftGear.gameObject.SetActive(value);
        _rightGear.gameObject.SetActive(value);
        _leftStaticGear.gameObject.SetActive(value);
        _rightStaticGear.gameObject.SetActive(value);



        foreach (Transform child in _leftGear.transform)
        {
            child.gameObject.SetActive(false);
        }

        foreach (Transform child in _rightGear.transform)
        {
            child.gameObject.SetActive(false);
        }

        foreach (Transform child in _leftStaticGear.transform)
        {
            child.gameObject.SetActive(false);
        }

        foreach (Transform child in _rightStaticGear.transform)
        {
            child.gameObject.SetActive(false);
        }

        _leftStaticRefill.gameObject.SetActive(false);
        _rightStaticRefill.gameObject.SetActive(false);
        _leftRefill.gameObject.SetActive(false);
        _rightRefill.gameObject.SetActive(false);
    }

    private void AlterChangeSceneState(bool value)
    {
        _leftGear.GetComponent<ChangeScene>().enabled = value;
        _rightGear.GetComponent<ChangeScene>().enabled = value;
        _leftStaticGear.GetComponent<ChangeScene>().enabled = value;
        _rightStaticGear.GetComponent<ChangeScene>().enabled = value;
    }

    private void EnableText(string go_name)
    {
        foreach (Transform child in PreparedText.transform)
        {
            if (child.gameObject.name == go_name)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void EnableGear(string gear_name)
    {
        foreach (Transform child in _leftGear.transform)
        {
            if (child.gameObject.name == gear_name)
            {

                child.gameObject.SetActive(true);
            }
            else if (gear_name == "ShieldGear" && child.name == "BubbleShield")
            {
                child.gameObject.SetActive(true);
            }
            else if (gear_name == "WeaponGear" && child.name == "Weapon")
            {
                child.gameObject.SetActive(true);
            }
        }

        foreach (Transform child in _rightGear.transform)
        {
            if (child.gameObject.name == gear_name)
            {

                child.gameObject.SetActive(true);
            }
            else if (gear_name == "ShieldGear" && child.name == "BubbleShield")
            {
                child.gameObject.SetActive(true);
            } else if (gear_name == "WeaponGear" && child.name == "Weapon")
            {
                child.gameObject.SetActive(true);
            }
        }

        foreach (Transform child in _leftStaticGear.transform)
        {
            if (child.gameObject.name == gear_name)
            {
                child.gameObject.SetActive(true);
            }
        }

        foreach (Transform child in _rightStaticGear.transform)
        {
            if (child.gameObject.name == gear_name)
            {
                child.gameObject.SetActive(true);
            }
        }

    }

    private void ResetTaskState()
    {
        _TaskState[0] = false;
        _TaskState[1] = false;
    }


}
