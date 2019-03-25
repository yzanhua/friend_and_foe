using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    private bool isRotate = true;

    private SpriteRenderer sprite;
    private Animator anim;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        StartCoroutine(Flash());
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotate)
            Rotate();
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("BombEnd"))
            Destroy(gameObject);
    }

    private void Rotate()
    {
        transform.Rotate(0f,0f,1f);
        
    }

    IEnumerator Flash()
    {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < 3; i++)
        {// flash
            yield return new WaitForSeconds(0.7f);
            sprite.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.3f);
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }
        yield return new WaitForSeconds(1.5f); // start "begin Explode"
        anim.SetTrigger("BeginExplode");
        transform.rotation = Quaternion.identity;
        isRotate = false;
    }

    

}
