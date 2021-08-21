using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiControl2 : MonoBehaviour
{
    float scale;
    // Start is called before the first frame update
    void Start()
    {
        scale = 1.005f;
        transform.localScale = new Vector3(0.45f, 0.45f, 1.0f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //크기가 커졌다가 작아지는 모션
        if ((transform.localScale.x > 0.55f))
        {
            scale = 0.995f;
        }
        if((transform.localScale.x < 0.45f))
        {
            scale = 1.005f;
        }
        transform.localScale = new Vector3(scale*transform.localScale.x, scale*transform.localScale.y, transform.localScale.z);        
    }
}
