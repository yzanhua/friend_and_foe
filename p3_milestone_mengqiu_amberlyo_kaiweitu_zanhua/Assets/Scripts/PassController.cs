using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassController : MonoBehaviour
{
    public int task_num;


    private bool rising = false;
    private bool moving = true;

    void OnCollisionEnter2D(Collision2D collision)
    {
        bool isRight = transform.position.x > 0f;
        if (collision.collider.tag.Contains("Submarine") && task_num == 1)
        {
            if (TutorialManager.TaskComplete(task_num, isRight))
            {
                GetComponent<SpriteRenderer>().color = new Color(37f / 255f, 250f / 255f, 18f / 255f, 1f);
            }

        } else if (collision.collider.tag.Contains("Bullet") && task_num == 5)
        {

            if (TutorialManager.TaskComplete(task_num, isRight))
            {
                GetComponent<SpriteRenderer>().color = new Color(37f / 255f, 250f / 255f, 18f / 255f, 1f);
                moving = false;
            }
            Destroy(collision.collider.gameObject);
        }
    }

    private void Update()
    {
        if (!moving)
            return;
        if (task_num == 5)
        {
            if (rising)
            {
                Vector3 target_position = new Vector3(transform.position.x, -0.25f, 0f);

                if ((transform.position - target_position).magnitude < 0.1f)
                {
                    transform.position = target_position;
                    rising = false;
                }
                else
                {
                    transform.position += (target_position - transform.position).normalized * 2f * Time.deltaTime;
                }
            }
            else
            {
                Vector3 target_position = new Vector3(transform.position.x, -4.75f, 0f);
                if ((transform.position - target_position).magnitude < 0.1f)
                {
                    transform.position = target_position;
                    rising = true;
                }
                else
                {
                    transform.position += (target_position - transform.position).normalized * 2f * Time.deltaTime;
                }
            }
            
        }
    }
}
