using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiControl4 : MonoBehaviour
{
    int move;
    // Start is called before the first frame update
    void Start()
    {
        //transform.rotation = Quaternion.Euler(0, 0, 0);
        move = 1;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // 좌우로 회전하는 모션 
        if (transform.rotation.z > 0.25f) { move = -1; }
        if (transform.rotation.z < -0.25f) { move = 1; }
        transform.Rotate(new Vector3(0, 0, move * 70f * Time.deltaTime));
    }
}
