using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleusScript : MonoBehaviour
{
    Rigidbody2D rigid;
    float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.velocity = new Vector2();
        if (Input.GetKey(KeyCode.UpArrow))
            rigid.velocity = new Vector2(rigid.velocity.x, speed);
        if (Input.GetKey(KeyCode.DownArrow))
            rigid.velocity = new Vector2(rigid.velocity.x, -speed);
        if (Input.GetKey(KeyCode.LeftArrow))
            rigid.velocity = new Vector2(-speed, rigid.velocity.y);
        if (Input.GetKey(KeyCode.RightArrow))
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
    }
}
