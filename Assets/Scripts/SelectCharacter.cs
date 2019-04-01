using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    public List<GameObject> players;
    public List<GameObject> zones;
    public int GamePadID = 0;
    public int CurrentSelection = 0;

    private bool cdOK = true;
    private Vector3 player_pos;
    private Vector3 zone_pos;

    private GameObject curr_player;
    private GameObject curr_zone;

    private void Start()
    {
        Global.instance.PlayerID2GamePadID = new int[] { -1, -1, -1, -1 };
        InputSystemManager.instance.PlayerID2GamePadID = new int[] {0, 1, 2, 3};
        Global.instance.SelectionEnable = true;

        player_pos = players[CurrentSelection].transform.position;
        zone_pos = zones[CurrentSelection].transform.position;
        curr_player = Instantiate(players[CurrentSelection], player_pos, Quaternion.identity);
        curr_player.SetActive(true);
    }

    void Update()
    {
        if (!Global.instance.SelectionEnable)
            return;
        if (!Global.instance.SelectedStatus[GamePadID])
        {
            int inputX = GetInputX();
            UpdateCharacter(inputX);
            if (InputSystemManager.GetAction1(GamePadID))
            {
                SelectThisCharacter();
            }
        }
        else if (InputSystemManager.GetAction1(GamePadID))
        {
            UnSelectThisCharacter();
        }
    }

    void UnSelectThisCharacter()
    {
        Global.instance.PlayerID2GamePadID[CurrentSelection] = -1;
        Global.instance.SelectedStatus[GamePadID] = false;
        Destroy(curr_zone);
    }
    void SelectThisCharacter()
    {
        if (Global.instance.PlayerID2GamePadID[CurrentSelection] != -1)
        {
            return;
        }
        Global.instance.PlayerID2GamePadID[CurrentSelection] = GamePadID;
        Global.instance.SelectedStatus[GamePadID] = true;
        curr_zone = Instantiate(zones[CurrentSelection], zone_pos, Quaternion.identity);
        curr_zone.SetActive(true);

        for (int i = 0; i < Global.instance.numOfPlayers; i++)
            if (!Global.instance.SelectedStatus[i]) return;

        Global.instance.SelectionEnable = false;
        for (int i = 0; i < 4; i++)
            InputSystemManager.instance.PlayerID2GamePadID[i] = Global.instance.PlayerID2GamePadID[i];

        StartCoroutine(LoadNextScene());
    }

    void UpdateCharacter(int inputX)
    {
        if (Global.instance.PlayerID2GamePadID[CurrentSelection] == -1)
            curr_player.GetComponent<SpriteRenderer>().color = Color.white;
        else if (Global.instance.PlayerID2GamePadID[CurrentSelection] != GamePadID)
            if (curr_player != null)
                curr_player.GetComponent<SpriteRenderer>().color = Color.black;

        if (inputX == 0 || !cdOK) return;
        cdOK = false;
        StartCoroutine(WaitCD());

        Destroy(curr_player);
        CurrentSelection += inputX;

        if (CurrentSelection < 0)
            CurrentSelection += 4;
        else if (CurrentSelection > 3)
            CurrentSelection -= 4;

        curr_player = Instantiate(players[CurrentSelection], player_pos, Quaternion.identity);
        curr_player.SetActive(true);
    }
    int GetInputX()
    {
        float verticalInput = InputSystemManager.GetLeftSVertical(GamePadID);
        float horizontalInput = InputSystemManager.GetLeftSHorizontal(GamePadID);
        if (horizontalInput == 0f) return 0;
        if (Mathf.Abs(verticalInput) < Mathf.Abs(horizontalInput))
        {
            return (int)Mathf.Sign(horizontalInput);
        }
        return 0;
    }
    IEnumerator WaitCD()
    {
        yield return new WaitForSeconds(0.3f);
        cdOK = true;
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Tutorial");
    }
}
