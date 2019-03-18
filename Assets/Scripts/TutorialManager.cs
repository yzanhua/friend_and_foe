using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

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
    public GameObject rightGear;

    public static int leftTutorialState = 0;
    public static int rightTutorialState = 0;
    private List<int> task_map;
    private float original_target_time_left;
    private float original_target_time_right;
    private bool isStartingGame = false;

    static public bool TaskComplete(int task_num, bool isRight)
    {
        if (rightTutorialState == instance.task_map[task_num] && isRight)
        {
            if (task_num == 1)
            {
                instance.rightTaskBox.text = "Leave the blue gear";
            }
            else if (task_num == 4)
            {
                instance.rightMovingBox.SetActive(true);
                instance.rightTaskBox.text = "Shoot the red target";
            }
            else
            {
                instance.rightTaskBox.text = "Great!";
            }


            rightTutorialState++;
            return true;
        }
        else if (leftTutorialState == instance.task_map[task_num] && !isRight)
        {
            if (task_num == 1)
            {
                instance.leftTaskBox.text = "Leave the blue gear";
            }
            else if (task_num == 4)
            {
                instance.leftMovingBox.SetActive(true);
                instance.leftTaskBox.text = "Shoot the red target";
            }
            else
            {
                instance.leftTaskBox.text = "Great!";
            }

            leftTutorialState++;
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
        if (tutorialMode)
        {
            edge.SetActive(true);
            //TutorialUI.SetActive(true);
            // leftIndicator.SetActive(true);
            // rightIndicator.SetActive(true);
            leftTrigger.SetActive(true);
            rightTrigger.SetActive(true);


            leftTextBox.text = "Left sticker to control the character";
            rightTextBox.text = "Use action button 2 to seat on the gear";
            leftTaskBox.text = "Seat on the blue gear";
            rightTaskBox.text = "Seat on the blue gear";
            leftIndicator.transform.localPosition = new Vector3(1.76f, 0.19f, 0f);
            rightIndicator.transform.localPosition = new Vector3(1.76f, 0.19f, 0f);
            task_map = new List<int>();
            task_map.Insert(0, 0);
            task_map.Insert(1, 2);
            task_map.Insert(2, 3);
            task_map.Insert(3, 5);
            task_map.Insert(4, 7);
            task_map.Insert(5, 8);
            task_map.Insert(6, 10);
            original_target_time_left = leftGear.GetComponent<ChangeScene>().targetTime;
            original_target_time_right = rightGear.GetComponent<ChangeScene>().targetTime;
            leftGear.GetComponent<ChangeScene>().targetTime = 100000;
            rightGear.GetComponent<ChangeScene>().targetTime = 100000;
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
        if (leftTutorialState == 1 && rightTutorialState ==1)
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
            leftGear.GetComponent<ChangeScene>().targetTime = instance.original_target_time_left;
            rightGear.GetComponent<ChangeScene>().targetTime = instance.original_target_time_right;
            //SceneManager.LoadScene("Main");
            TransitionEffect.TriggerDarkTransition();
        }

    }

    IEnumerator StartTheGame()
    {
        isStartingGame = true;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Main");
    }




}
