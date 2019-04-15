using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBackground : MonoBehaviour
{
    [Range(0f, 0.1f)]
    public float speed;

    private Transform _bg1;
    private Transform _bg2;

    private List<Transform> _bgs;
    private Vector3 _last;

    // Start is called before the first frame update
    void Start()
    {
        _last = new Vector3(100000f, 0f, 0f);
        _bgs = new List<Transform>();
        foreach (Transform child in transform)
        {
            _bgs.Add(child);

            if (_last.x > child.position.x)
            {
                _last = child.position;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        _last += new Vector3(speed, 0f, 0f);
        foreach (Transform tf in _bgs)
        {
            tf.position += new Vector3(speed, 0f, 0f);
            if (tf.position.x >= 30f)
            {
                tf.transform.position = _last - new Vector3(30f, 0f, 0f);
                _last = tf.transform.position;
            }
        }
    }
}
