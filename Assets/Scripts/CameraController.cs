using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// requirement:
// 1): initial positions of two submarines (proxy):
// 3): initial Camera's z-position = -10f;
public class CameraController : MonoBehaviour
{
    // How many units should we keep from the players
    public float followTimeDelta = 1f;

    public Transform sub1;
    public Transform sub2;

    private Camera cam;
    private float size;
    private Vector3 center;
    private float radius;

    private void Start()
    {
        cam = GetComponent<Camera>();
        center.z = -10f;
        radius = sub1.gameObject.GetComponent<CircleCollider2D>().radius;
        radius = radius * sub1.localScale.x * sub1.parent.localScale.x;
    }

    private void Update()
    {
        CalculateShape();
        cam.orthographicSize = size;
        cam.transform.position = Vector3.Slerp(cam.transform.position, center, followTimeDelta);

        if ((center - cam.transform.position).magnitude <= 0.05f)
            cam.transform.position = center;
    }

    private void CalculateShape()
    {
        float left = (Mathf.Min(sub1.position.x, sub2.position.x) - radius - Global.instance.maxScreenSize / 9f * 16f) / 2f;
        float right = (Mathf.Max(sub1.position.x, sub2.position.x) + radius + Global.instance.maxScreenSize / 9f * 16f) / 2f;
        float left_dis = left + Global.instance.maxScreenSize / 9f * 16f;
        float right_dis = Global.instance.maxScreenSize / 9f * 16f - right;

        float down = (Mathf.Min(sub1.position.y, sub2.position.y) - radius - Global.instance.maxScreenSize) / 2f;
        float up = (Mathf.Max(sub1.position.y, sub2.position.y) + radius + Global.instance.maxScreenSize) / 2f;
        float down_dis = down + Global.instance.maxScreenSize;
        float up_dis = Global.instance.maxScreenSize - up;

        size = Mathf.Max((right - left) / 32f * 9f, (up - down) / 2f);

        if (left_dis < right_dis)
            center.x = Mathf.Min(left + size * 16f / 9f, 0f);
        else
            center.x = Mathf.Max(right - size * 16f / 9f, 0f);

        if (down_dis < up_dis)
            center.y = Mathf.Min(down + size, 0f);
        else if (down_dis > up_dis)
            center.y = Mathf.Max(up - size, 0f);
    }
}
