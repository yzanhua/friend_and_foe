using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        REFILL,
        SHOOT,
        SHIELD,
        TASK
    }

    public bool tutorialMode = false;

    public Text leftTextBox;
    public Text rightTextBox;
    public Text leftTaskBox;
    public Text rightTaskBox;
    public GameObject leftIndicator;
    public GameObject rightIndicator;
    public GameObject rightTrigger;
    public GameObject leftTrigger;
    public GameObject leftMovingBox;
    public GameObject rightMovingBox;
    public GameObject TutorialUI;
    public GameObject GameUI;
    public GameObject gameController;
    public GameObject edge;
    public GameObject leftGear;
    public GameObject leftStaticGear;
    public GameObject rightStaticGear;
    public GameObject rightGear;
    public GameObject leftStaticRefill;
    public GameObject leftRefill;
    public GameObject rightStaticRefill;
    public GameObject rightRefill;

    public static int leftTutorialState = 0;
    public static int rightTutorialState = 0;
    public State state;
    private List<int> task_map;
    private float original_target_time_left;
    private float original_target_time_right;
    private bool isStartingGame = false;
    public static int rightTaskIndex;
    public static int leftTaskIndex;

    private Hashtable rightTaskList = new Hashtable();
    private Hashtable leftTaskList = new Hashtable();

    public bool leftTaskState = false;
    public bool rightTaskState = false;



    static public bool CompleteTask(TaskType task, bool isRight)
    {
        if (!instance.tutorialMode)
            return false;

        if (isRight)
        {
            if (instance.state == State.CHARACTER)
            {
                if (task == TaskType.DASH || task == TaskType.JUMP)
                {
                    instance.rightTaskList[task] = true;
                    instance.rightTaskBox.text = "Press Button A to jump from the latter";

                    if ((bool)instance.rightTaskList[TaskType.DASH] && (bool)instance.rightTaskList[TaskType.JUMP])
                    {
                        instance.rightTaskBox.text = "Great!";
                        instance.rightTaskState = true;
                    }

                    return true;
                }
            } else if (instance.state == State.MOVEMENT)
            {
                if (task == TaskType.SEAT || task == TaskType.MOVE)
                {
                    instance.rightTaskList[task] = true;
                    instance.rightTaskBox.text = "Drive the submarine to the yellow area";
                    instance.rightTrigger.SetActive(true);

                    if ((bool)instance.rightTaskList[TaskType.SEAT] && (bool)instance.rightTaskList[TaskType.MOVE])
                    {
                        instance.rightTaskBox.text = "Great!";
                        instance.rightTaskState = true;
                    }

                    return true;
                }
               
            } else if (instance.state == State.WEAPON)
            {
                if (task == TaskType.REFILL|| task == TaskType.SHOOT)
                {
                    instance.rightTaskList[task] = true;
                    instance.rightTaskBox.text = "Use Weapon controller to shoot the red target";
                    instance.rightMovingBox.SetActive(true);

                    if ((bool)instance.rightTaskList[TaskType.REFILL] && (bool)instance.rightTaskList[TaskType.SHOOT])
                    {
                        instance.rightTaskBox.text = "Great!";
                        instance.rightTaskState = true;
                    }
                    return true;
                }

            }
            else if (instance.state == State.SHIELD)
            {
                if (task == TaskType.SHIELD)
                {
                    instance.rightTaskBox.text = "Great!";
                    instance.rightTaskState = true;
                    return true;
                }

            }
        }
        else
        {
            if (instance.state == State.CHARACTER)
            {
                if (task == TaskType.DASH || task == TaskType.JUMP)
                {
                    instance.leftTaskList[task] = true;
                    instance.leftTaskBox.text = "Press Button A to jump from the latter";

                    if ((bool)instance.leftTaskList[TaskType.DASH] && (bool)instance.leftTaskList[TaskType.JUMP])
                    {
                        instance.leftTaskBox.text = "Great!";
                        instance.leftTaskState = true;
                    }

                    return true;
                }

            }
            else if (instance.state == State.MOVEMENT)
            {
                if (task == TaskType.SEAT || task == TaskType.MOVE)
                {
                    instance.leftTaskList[task] = true;
                    instance.leftTaskBox.text = "Drive the submarine to the yellow area";
                    instance.leftTrigger.SetActive(true);

                    if ((bool)instance.leftTaskList[TaskType.SEAT] && (bool)instance.leftTaskList[TaskType.MOVE])
                    {
                        instance.leftTaskBox.text = "Great!";
                        instance.leftTaskState = true;
                    }

                    return true;
                }

            }
            else if (instance.state == State.WEAPON)
            {
                if (task == TaskType.REFILL || task == TaskType.SHOOT)
                {
                    instance.leftTaskList[task] = true;
                    instance.leftTaskBox.text = "Use Weapon controller to shoot the red target";
                    instance.leftMovingBox.SetActive(true);

                    if ((bool)instance.leftTaskList[TaskType.REFILL] && (bool)instance.leftTaskList[TaskType.SHOOT])
                    {
                        instance.leftTaskBox.text = "Great!";
                        instance.leftTaskState = true;
                    }
                    return true;
                }

            }
            else if (instance.state == State.SHIELD)
            {
                if (task == TaskType.SHIELD)
                {
                    instance.leftTaskBox.text = "Great!";
                    instance.leftTaskState = true;
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
        if (tutorialMode)
        {
            // Activate the tutorial component
            // edge.SetActive(true);
            // leftTrigger.SetActive(true);
            // rightTrigger.SetActive(true);
            // Use the right text for the text box
            // leftTextBox.text = "Use left sticker to control the character";
            // rightTextBox.text = "Use action button 2 to attach to the control station";
            leftTaskBox.text = "Press B to start the tutorial.";
            rightTaskBox.text = "Press A to skip the tutorial";
            Global.instance.AllPlayersMovementEnable = false;
            state = State.SKIPTUTORIAL;
            // leftIndicator.transform.localPosition = new Vector3(1.76f, 0.19f, 0f);
            // rightIndicator.transform.localPosition = new Vector3(1.76f, 0.19f, 0f);
            task_map = new List<int>();
            task_map.Insert(0, 0);
            task_map.Insert(1, 2);
            task_map.Insert(2, 3);
            task_map.Insert(3, 5);
            task_map.Insert(4, 7);
            task_map.Insert(5, 8);
            task_map.Insert(6, 10);

            for (int i = 0; i < (int) TaskType.TASK; i++)
            {
                leftTaskList[(TaskType)i] = false;
                rightTaskList[(TaskType)i] = false;
            }


            // original_target_time_left = leftGear.GetComponent<ChangeScene>().targetTime;
            // original_target_time_right = rightGear.GetComponent<ChangeScene>().targetTime;
            AlterChangeSceneState(false);
            AlterGearState(false);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialMode)
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
                bool a1_value = InputSystemManager.GetAction1(i);
                bool a2_value = InputSystemManager.GetAction2(i);
                if (a1_value)
                {
                    state = State.PRE_FINISHED;
                } else if (a2_value)
                {
                    state = State.CHARACTER;
                    leftTextBox.text = "Left sticker to control the player　";
                    rightTextBox.text = "Use action button 1 to dash";
                    rightTaskBox.text = "Press button A to dash";
                    leftTaskBox.text = "Press button A to dash";
                    edge.SetActive(true);
                    Global.instance.AllPlayersMovementEnable = true;
                }
            }
        } else if (state == State.PRE_FINISHED)
        {
            /*tutorialMode = false;
            GameUI.SetActive(true);
            gameController.SetActive(true);
            edge.SetActive(false);
            TutorialUI.SetActive(false);
            leftTrigger.SetActive(false);
            rightTrigger.SetActive(false);
            AlterChangeSceneState(true);
            AlterGearState(true);
            state = State.FINISHED;*/
            Global.instance.AllPlayersMovementEnable = true;
            SceneManager.LoadScene("Game");
        } else if (leftTaskState && rightTaskState)
        {
            if (state == State.CHARACTER)
            {
                state = State.MOVEMENT;
                AlterGearState(true);
                leftTextBox.text = "Left sticker to control the player, submarine or weapon";
                rightTextBox.text = "Use action button 1 to attack and trigger the shield";
                leftTaskBox.text = "Press action button 2 near the rudder area";
                rightTaskBox.text = "Press action button 2 near the rudder area";
                //GameObject movementGear = rightGear.gameObject.transform.Find("MovementGear").gameObject;
                leftTaskState = false;
                rightTaskState = false;

            } else if (state == State.MOVEMENT)
            {
                state = State.WEAPON;
                leftTextBox.text = "Left sticker to control the player, submarine or weapon";
                rightTextBox.text = "Use action button 1 to attach and detach from the station";
                leftTaskBox.text = "Press action button 2 near the bullet refill area";
                rightTaskBox.text = "Press action button 2 near the bullet refill area";
                leftTaskState = false;
                rightTaskState = false;
            } else if (state == State.WEAPON)
            {
                state = State.SHIELD;
                leftTextBox.text = "Left sticker to control the player, submarine or weapon";
                rightTextBox.text = "Use action button 1 to attack and trigger the shield";
                leftTaskBox.text = "Press action button 2 near the shield area";
                rightTaskBox.text = "Press action button 2 near the shield area";
                leftTaskState = false;
                rightTaskState = false;
            } else if (state == State.SHIELD)
            {
                state = State.FINISHED;
                leftTaskBox.text = "Tutorial complete";
                rightTaskBox.text = "Tutorial complete";
                leftTaskState = false;
                rightTaskState = false;
                StartCoroutine(StartTheGame());
            }
        }
        /*if (leftTutorialState == 1 && rightTutorialState ==1)
        {
            leftTutorialState++;
            rightTutorialState++;
            leftTextBox.text = "Left sticker to control the submarine";
            rightTextBox.text = "Use action button 1 to leave the gear";
            leftTaskBox.text = "Drive to the yellow area";
            rightTaskBox.text = "Drive to the yellow area";
        } else if (leftTutorialState == 4 && rightTutorialState == 4)
        {
            leftTutorialState++;
            rightTutorialState++;
            leftTrigger.SetActive(false);
            rightTrigger.SetActive(false);
            leftIndicator.transform.localPosition = new Vector3(-1.65f, 0f, 0f);
            rightIndicator.transform.localPosition = new Vector3(-1.65f, 0f, 0f);
            leftTextBox.text = "Left sticker to control the player";
            rightTextBox.text = "Use action button 2 to fill the bullet";
            leftTaskBox.text = "Fill the bullet";
            rightTaskBox.text = "Fill the bullet";
        } else if (leftTutorialState == 6 && rightTutorialState == 6)
        {
            leftTutorialState++;
            rightTutorialState++;
            leftIndicator.transform.localPosition = new Vector3(1.17f, 1.59f, 0f);
            rightIndicator.transform.localPosition = new Vector3(1.17f, 1.59f, 0f);
            leftTextBox.text = "Left sticker to control the weapon";
            rightTextBox.text = "Use action button 2 to trigger the attack";
            leftTaskBox.text = "Seat on the red gear";
            rightTaskBox.text = "Seat on the red grea";
        }
        else if (leftTutorialState == 9 && rightTutorialState == 9)
        {
            leftTutorialState++;
            rightTutorialState++;
            leftIndicator.transform.localPosition = new Vector3(-0.5f, -1.12f, 0f);
            rightIndicator.transform.localPosition = new Vector3(-0.5f, -1.12f, 0f);
            leftTextBox.text = "Left sticker to rotate the shield";
            rightTextBox.text = "Use action button 2 to trigger the shield";
            leftMovingBox.SetActive(false);
            rightMovingBox.SetActive(false);
            leftTaskBox.text = "Trigger the shield";
            rightTaskBox.text = "Trigger the shield";
        } else if (leftTutorialState == 11 && rightTutorialState == 11)
        {
            leftTutorialState ++;
            rightTutorialState ++;
            leftTaskBox.text = "Tutorial complete";
            rightTaskBox.text = "Tutorial complete";
            instance.StartCoroutine(StartTheGame());
            //StartCoroutine(instance.StartTheGame());
        } else if ( !isStartingGame && leftTutorialState == 12 && rightTutorialState == 12)
        {
            tutorialMode = false;
            GameUI.SetActive(true);
            gameController.SetActive(true);
            edge.SetActive(false);
            TutorialUI.SetActive(false);
            leftIndicator.SetActive(false);
            rightIndicator.SetActive(false);
            leftTrigger.SetActive(false);
            rightTrigger.SetActive(false);
            // leftGear.GetComponent<ChangeScene>().targetTime = instance.original_target_time_left;
            // rightGear.GetComponent<ChangeScene>().targetTime = instance.original_target_time_right;
            //SceneManager.LoadScene("Main");
            TransitionEffect.TriggerDarkTransition();
        }*/

    }

    IEnumerator StartTheGame()
    {
        isStartingGame = true;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Game");
    }


    private void AlterGearState(bool value)
    {
        //
        leftGear.gameObject.SetActive(value);
        leftRefill.gameObject.SetActive(value);
        rightRefill.gameObject.SetActive(value);
        rightGear.gameObject.SetActive(value);

        leftStaticGear.gameObject.SetActive(value);
        rightStaticGear.gameObject.SetActive(value);
        leftStaticGear.gameObject.SetActive(value);
        leftStaticGear.gameObject.SetActive(value);
    }

    private void AlterChangeSceneState(bool value)
    {
        leftGear.GetComponent<ChangeScene>().enabled = value;
        rightGear.GetComponent<ChangeScene>().enabled = value;
        leftStaticGear.GetComponent<ChangeScene>().enabled = value;
        rightStaticGear.GetComponent<ChangeScene>().enabled = value;
    }


}
