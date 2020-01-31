using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleusOrbitScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.tag == "Pixel")
        {
            // hier im Pixel sagen, dass er innerhalb des Orbits ist
            //collision.gameObject.GetComponent<PIXELSCRIPT>().BOOLVARIABLE = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.tag == "Pixel")
        {
            // hier im Pixel sagen, dass er innerhalb des Orbits ist
            //collision.gameObject.GetComponent<PIXELSCRIPT>().BOOLVARIABLE = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
