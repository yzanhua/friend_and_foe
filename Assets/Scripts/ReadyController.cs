using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyController : MonoBehaviour
{
    public int GamePadID = 0;
    public float fadeTime = 1f;
    public AnimationCurve fadeInCurve;

    private Transform ready_mask;
    private Vector3 init_mask_position;
    private Vector3 target_mask_position;
    private bool show_ready_img = false;
    private bool inTransitionAnim = false;

    void Start()
    {
        Global.initializeVariables();

        ready_mask = transform.Find("char").Find("mask");
        init_mask_position = ready_mask.localPosition;
        target_mask_position = init_mask_position;
        target_mask_position.y = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Global.instance.SelectionEnable || inTransitionAnim)
            return;

        if (InputSystemManager.GetAction1(GamePadID))
        {
            if (!Global.instance.SelectedStatus[GamePadID])
            {
                SelectThisCharacter();
            }
            else
            {
                UnSelectThisCharacter();
            }
        }       
    }

    private void SelectThisCharacter()
    {
        Global.instance.SelectedStatus[GamePadID] = true;
        show_ready_img = true;
        inTransitionAnim = true;

        if (SoundManager.instance != null)
            SoundManager.instance.PlaySound("ready_select");

        StartCoroutine(ShowReadyImg());

        for (int i = 0; i < Global.instance.numOfPlayers; i++)
            if (!Global.instance.SelectedStatus[i]) return;

        Global.instance.SelectionEnable = false;

        StartCoroutine(LoadNextScene());
    }

    private void UnSelectThisCharacter()
    {
        Global.instance.SelectedStatus[GamePadID] = false;
        show_ready_img = false;
        inTransitionAnim = true;

        if (SoundManager.instance != null)
            SoundManager.instance.PlaySound("ready_deselect");

        StartCoroutine(DeShowReadyImg());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Tutorial");
    }

    IEnumerator ShowReadyImg()
    {
        float temp = 0f;
        while (show_ready_img && temp < 1f)
        {
            temp += Time.deltaTime / fadeTime;
            ready_mask.localPosition = Vector3.Lerp(init_mask_position, target_mask_position, fadeInCurve.Evaluate(temp));
            yield return null;
        }
        inTransitionAnim = false;
    }

    IEnumerator DeShowReadyImg()
    {
        float temp = 0f;
        while (!show_ready_img && temp < 1f)
        {
            temp += Time.deltaTime / fadeTime;
            ready_mask.localPosition = Vector3.Lerp(init_mask_position, target_mask_position, fadeInCurve.Evaluate(1 - temp));
            yield return null;
        }
        inTransitionAnim = false;
    }
}
