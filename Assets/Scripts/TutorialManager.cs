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
        START,
        SKIPTUTORIAL,
        TUTORIAL_START,
        CHARACTER,
        MOVEMENT,
        SHIELD,
        WEAPON,
        PRE_FINISHED,
        FINISHED,
        STATE
    };


    public enum TaskType
    {
        START,
        SKIP,
        TUTORIAL_START,
        HOOK_MOVE,
        DASH,
        SEAT,
        MOVE,
        DASH_SUB,
        GET_DOWN,
        SHIELD,
        SHIELD_DES,
        BOUNCE,
        SEAT_WEAPON,
        SHOOT,
        REFILL,
        CANON_INTR_ONE,
        CANON_INTR_TWO,
        CANON_INTR_THREE,
        HUGE_CANON,
        TASK
    }


    public bool tutorialMode = false;

    public GameObject RightSubmarine;
    // public GameObject RightSubmarine_NPC;
    public GameObject LeftSubmarine;
    // public GameObject LeftSubmarine_NPC;

    public GameObject LeftHealthBar;
    public GameObject RightHealthBar;

    public GameObject PreparedText;
    public GameObject TutorialUI;

    // public GameObject CanvsGame;


    public static int leftTutorialState = 0;
    public static int rightTutorialState = 0;
    public State state;
    public float speed = 75f;

    public delegate void CallBack();

    private bool _isStartingGame = false;

    // private Hashtable _rightTaskList = new Hashtable();
    // private Hashtable _leftTaskList = new Hashtable();


    private List<Hashtable> _TaskList = new List<Hashtable>(2);
   
    private bool[] _TaskState = new bool[2];

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

    private TaskType _task;

    private Hashtable _startMap = new Hashtable();
    private Hashtable _skipMap = new Hashtable();

    private bool _InStateTransition = false;
    private List<List<TaskType>> _task_list = new List<List<TaskType>>();

    private bool isPosition1 = true;
    private float _frame_num = 0;

    private Dictionary<TaskType, string> _task2str = new Dictionary<TaskType, string>() 
    { 
        { TaskType.MOVE , "Move" },
        { TaskType.DASH, "Dash" },
        { TaskType.SEAT, "Seat"},
        { TaskType.DASH_SUB, "DashSub"},
        { TaskType.SHIELD, "Shield"},
        { TaskType.REFILL, "Refill"},
        { TaskType.SHOOT, "Shoot"},
        { TaskType.BOUNCE, "Bounce"},
        { TaskType.SEAT_WEAPON, "Seat_weapon"},
        { TaskType.HUGE_CANON, "HugeCanon"},
        { TaskType.SKIP, "SkipTutorial"},
        { TaskType.START, "Start"},
        { TaskType.TUTORIAL_START, "tutorial_start"},
        { TaskType.HOOK_MOVE, "Hook_move"},
        { TaskType.GET_DOWN, "Get_down"},
        { TaskType.SHIELD_DES, "Shield_des"},
        { TaskType.CANON_INTR_ONE, "Laser_intro_1"},
        { TaskType.CANON_INTR_TWO, "Laser_intro_2"},
        { TaskType.CANON_INTR_THREE, "Laser_intro_3"}
        };
        

    private List<TaskType> _transitionTask = new List<TaskType>()
    {
        TaskType.START,
        TaskType.TUTORIAL_START,
        TaskType.HOOK_MOVE,
        TaskType.SHIELD_DES,
        TaskType.CANON_INTR_ONE,
        TaskType.CANON_INTR_TWO,
        TaskType.CANON_INTR_THREE,
    }; 




    static public bool CompleteTask(TaskType task, bool isRight)
    {
        if (!instance.tutorialMode || instance._InStateTransition)
            return false;


        int pos = isRight ? 1 : 0;

        if (instance.state < State.PRE_FINISHED)
        {
            for (int index = 0; index < instance._task_list[(int)instance.state].Count; index++)
            {
                List<TaskType> list = instance._task_list[(int)instance.state];
                if (list[index] == task)
                {
                    if (index == 0)
                    {
                        if (((bool)instance._TaskList[0][task] && (bool)instance._TaskList[1][task]))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (!((bool)instance._TaskList[0][list[index - 1]] && (bool)instance._TaskList[1][list[index - 1]]))
                        {
                            return false;
                        }
                        else if (((bool)instance._TaskList[0][task] && (bool)instance._TaskList[1][task]))
                        {
                            return true;
                        }
                    }

                    instance._TaskList[pos][task] = true;
                    int num = 0;

                    if ((bool)instance._TaskList[0][task] && (bool)instance._TaskList[1][task])
                    {
                        num = 2;
                        if (index < list.Count - 1)
                        {
                            instance._task = instance._task + 1;
                            instance._InStateTransition = true;
                            instance.StartCoroutine(instance.ChangeState(2f, () =>
                            {
                           
                                instance.StartCoroutine((instance.DialogBoxAnimation(instance._task2str[list[index + 1]])));
                                //instance.EnableText(instance._task2str[list[index + 1]]);
                            }));
                        }

                    }
                    else if ((bool)instance._TaskList[0][task] || (bool)instance._TaskList[1][task])
                    {
                        num = 1;
                    }

                    Transform num_text = instance.PreparedText.transform.Find(instance._task2str[task]).Find("flash_text");
                    if (num_text != null)
                    {

                        num_text.Find("num").gameObject.GetComponent<Text>().text = num + " / 2";
                    }

                    foreach (TaskType tt in list)
                    {
                        if (!(bool)instance._TaskList[pos][tt])
                        {
                            return true;
                        }
                    }

                    instance._TaskState[pos] = true;
                    return true;

                }
            }
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
        Global.instance.AllPlayersMovementEnable = false;
        state = State.START;
        _task = TaskType.START;
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

        _task_list.Add(new List<TaskType>() { TaskType.START });
        _task_list.Add(new List<TaskType>() { TaskType.SKIP });
        _task_list.Add(new List<TaskType>() { TaskType.TUTORIAL_START });
        _task_list.Add(new List<TaskType>() { TaskType.HOOK_MOVE, TaskType.DASH });
        _task_list.Add(new List<TaskType>() { TaskType.SEAT, TaskType.MOVE, TaskType.DASH_SUB, TaskType.GET_DOWN});
        _task_list.Add(new List<TaskType>() { TaskType.SHIELD, TaskType.SHIELD_DES, TaskType.BOUNCE });
        _task_list.Add(new List<TaskType>() { TaskType.SEAT_WEAPON,TaskType.SHOOT, TaskType.REFILL, TaskType.CANON_INTR_ONE, TaskType.CANON_INTR_TWO, TaskType.HUGE_CANON });


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

        GameObject num_text = PreparedText.transform.Find("SkipTutorial").Find("flash_text").gameObject;

        num_text.transform.Find("start_num").gameObject.GetComponent<Text>().text = 0 + " / " + Global.instance.numOfPlayers;
        num_text.transform.Find("skip_num").gameObject.GetComponent<Text>().text = 0 + " / " + Global.instance.numOfPlayers;

        LeftSubmarine.SetActive(false);
        RightSubmarine.SetActive(false);
        AlterChangeSceneState(false);
        AlterGearState(false);
        EnableText("Start");
        Global.instance.godMode = true;
        // StartCoroutine(DialogBoxAnimation(false));
        SoundManager.instance.SoundTransition("background_battle", "main_scene_background");

    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialMode && !_isStartingGame && !_InStateTransition)
        {
            MoveToNextState();
        }

        if (!isPosition1)
        {
            UpdateHealthBar(LeftHealthBar, LeftSubmarine);
            UpdateHealthBar(RightHealthBar, RightSubmarine);
        }

    }

    void MoveToNextState()
    {
        if (state == State.SKIPTUTORIAL)
        {
            for (int i = 0; i < Global.instance.numOfPlayers; i++)
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

            for (int i = 0; i < Global.instance.numOfPlayers; i++)
            {
                if ((bool)_skipMap[i]) skip_num++;
                else if ((bool)_startMap[i]) start_num++;
            }

            if (skip_num == Global.instance.numOfPlayers)
            {
                StartCoroutine(DialogBoxAnimation("Prefinished"));
                StartCoroutine(ChangeState(3f, () =>
                {
                    state = State.PRE_FINISHED;
                }));
            }
            else if (start_num == Global.instance.numOfPlayers)
            {
                _InStateTransition = true;
                StartCoroutine(StartTutorialTransition());
            }

            GameObject num_text = PreparedText.transform.Find("SkipTutorial").Find("flash_text").gameObject;

            num_text.transform.Find("start_num").gameObject.GetComponent<Text>().text = start_num + " / " + Global.instance.numOfPlayers;
            num_text.transform.Find("skip_num").gameObject.GetComponent<Text>().text = skip_num + " / " + Global.instance.numOfPlayers;
        } else if (state == State.PRE_FINISHED)
        {
            Global.instance.AllPlayersMovementEnable = true;
            TransitionManager.Instance.TransitionOutAndLoadScene("Game");
        } else if (_transitionTask.Contains(_task))
        {
            _frame_num += Time.deltaTime;
            if (_frame_num > 3)
            {
                _frame_num = 0;
               
                if (_task == TaskType.START || _task == TaskType.TUTORIAL_START)
                {
                    state = (State)((int) state + 1);
                }

                instance._TaskList[0][_task] = true;
                instance._TaskList[1][_task] = true;
                _task = (TaskType)((int)_task + 1);
                _InStateTransition = true;
                StartCoroutine(DialogBoxAnimation(_task2str[_task]));

            }
        }
        else if (_TaskState[0] && _TaskState[1])
        {
            _InStateTransition = true;
            _task = (TaskType)((int)_task + 1);
            if (state == State.CHARACTER)
            {
                
                StartCoroutine(ChangeState(2f, () =>
                {
                    state = State.MOVEMENT;
                    AlterGearState(true);
                    ResetTaskState();
                    StartCoroutine(DialogBoxAnimation(_task2str[_task]));
                    EnableGear("MovementGear");
                }));


            } else if (state == State.MOVEMENT)
            {
            

                StartCoroutine(ChangeState(3f, () =>
                {
                    state = State.SHIELD;
                    ResetTaskState();
                    StartCoroutine(DialogBoxAnimation(_task2str[_task]));
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
                    StartCoroutine(DialogBoxAnimation(_task2str[_task]));
                    EnableGear("WeaponGear");
                    Global.instance.godMode = false;
                    isPosition1 = false;

                    LeftHealthBar.SetActive(true);
                    RightHealthBar.SetActive(true);
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

        while ((container_transform.position - target_position).magnitude > 0.5f)
        {
            container_transform.position += (target_position - container_transform.position).normalized * 10 * Time.deltaTime;
            yield return null;
        }

        container_transform.position = target_position;


        Global.instance.AllPlayersMovementEnable = true;
        state = State.TUTORIAL_START;
        _task = (TaskType)((int)_task + 1);
        StartCoroutine(DialogBoxAnimation("tutorial_start"));

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


    IEnumerator DialogBoxAnimation(string text)
    {
        instance._InStateTransition = true;
        Vector3 target;
        target = new Vector3(0f, 120f, 0f);

        RectTransform rt = TutorialUI.GetComponent<RectTransform>();

        while ((rt.localPosition - target).magnitude > 1f)
        {
            rt.localPosition += (target - rt.localPosition).normalized * speed * Time.deltaTime;
            yield return null;
        }
        rt.localPosition = target;

        EnableText(text);
        if (text == _task2str[TaskType.REFILL])
        {
            _leftStaticRefill.gameObject.SetActive(true);
            _rightStaticRefill.gameObject.SetActive(true);
            _leftRefill.gameObject.SetActive(true);
            _rightRefill.gameObject.SetActive(true);
        }

        if (!isPosition1)
        {
            target = new Vector3(0f, -70f, 0f);
        }
        else
        {
            target = new Vector3(0f, 0f, 0f);
        }

        while ((rt.localPosition - target).magnitude > 1f)
        {
            rt.localPosition += (target - rt.localPosition).normalized *  speed * Time.deltaTime;
            yield return null;
        }

        rt.localPosition = target;
        if (text == _task2str[TaskType.HUGE_CANON])
        {
            instance.LeftSubmarine.transform.Find("Submarine_proxy").gameObject.GetComponent<HealthCounter>().SetChargeBar(1000f);
            instance.RightSubmarine.transform.Find("Submarine_proxy").gameObject.GetComponent<HealthCounter>().SetChargeBar(1000f);
        }
        instance._InStateTransition = false;


    }


    void UpdateHealthBar(GameObject bar, GameObject sub)
    {
        HealthBar bar_script = bar.GetComponent<HealthBar>();
        HealthCounter health = sub.transform.Find("Submarine_proxy").gameObject.GetComponent<HealthCounter>();

        bar_script.SetSize((float)health.health / (float)health.maxHealth);

    }

    private void OnDestroy()
    {
        Global.instance.godMode = false;
    }

}
