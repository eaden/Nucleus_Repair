using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer RenderMat;

    private Material material;

    private float fDissolveAmount = 0;
    private bool isDissolving;

    private void Start()
    {
        material = RenderMat.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDissolving)
        {
            fDissolveAmount = Mathf.Clamp01(fDissolveAmount + Time.deltaTime);
            material.SetFloat("_DissolveAmount", fDissolveAmount);
            //Debug.Log(fDissolveAmount + " down");
        }
        else
        {
            fDissolveAmount = Mathf.Clamp01(fDissolveAmount - Time.deltaTime);
            material.SetFloat("_DissolveAmount", fDissolveAmount);
            //Debug.Log(fDissolveAmount +" up");
        }

        if (Input.GetKeyDown(KeyCode.X))
            isDissolving = true;
        if (Input.GetKeyDown(KeyCode.Y))
            isDissolving = false;

    }
}
