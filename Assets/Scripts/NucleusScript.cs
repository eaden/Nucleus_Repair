using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NucleusScript : MonoBehaviour
{
    Rigidbody2D rigid;
    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private float fRotationSpeed = 300f;
    [SerializeField]
    private float fConnectionRadius = 1.2f;
    [SerializeField]
    private float fRadiusSpeed = 1f;

    public bool isKeyDown = false;

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

        if (Input.GetKey(KeyCode.B))
        {
            isKeyDown = true;
            foreach (GameObject pixel in GameObject.FindGameObjectsWithTag("Pixel").Where(c => c.gameObject.GetComponent<PixelController>().isConnected))
            {
                pixel.GetComponent<PixelController>().Rotate(transform, new Vector3(0, 0, 1), fRotationSpeed, fConnectionRadius, fRadiusSpeed);
            }
        }

        if (Input.GetKey(KeyCode.M))
        {
            isKeyDown = true;
            foreach (GameObject pixel in GameObject.FindGameObjectsWithTag("Pixel").Where(c => c.gameObject.GetComponent<PixelController>().isConnected))
            {
                pixel.GetComponent<PixelController>().Rotate(transform, new Vector3(0, 0, 1), -fRotationSpeed, fConnectionRadius, fRadiusSpeed);
            }
        }

        if (Input.GetKeyUp(KeyCode.M) || Input.GetKeyUp(KeyCode.B))
            isKeyDown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pixel")
        {
            collision.gameObject.GetComponent<PixelController>().isConnected = true;
        }
    }
}
