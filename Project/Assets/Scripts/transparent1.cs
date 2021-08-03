using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparent1 : MonoBehaviour
{
    int i = 0;
    float tmp = 0f;
    // Start is called before the first frame update

    void Update()
    {
        i = (i + 1);
        if ((i / 60) % 3 == 0)
        {
            tmp = 0.0f;
        }
        if ((i / 60) % 3 == 1)
        {
            tmp = 0.5f;
        }
        if ((i / 60) % 3 == 2)
        {
            tmp = 1f;
        }
        ;
        if ((i % 60) == 0)
        {
            Material[] mats = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;
            for (int j = 0; j < mats.Length; j++)
            {
                Color tempcolor;
                tempcolor =  mats[j].color;
                tempcolor.a = tmp;
                mats[j].color = tempcolor;
            }
        }
    }
}
