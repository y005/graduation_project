using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiControl1 : MonoBehaviour
{
    int move;
    
    // Start is called before the first frame update
    void Start()
    {
        move = 1;
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        // 좌우로 회전하는 모션 
        if (transform.rotation.z > 0.25f) {move = -1;}
        if (transform.rotation.z < -0.25f) {move = 1;}
        transform.Rotate(new Vector3(0, 0, move*70f* Time.deltaTime));

        /* 한 바퀴 회전하는 모션 
        Debug.Log(transform.rotation.z);
        transform.Rotate(new Vector3(0, 0, 60f* Time.deltaTime));
        */

        /*흔들리는 모션
        pos = transform.position;
        pos.x += Random.Range(0f,100f) * 0.1f - 50f;
        pos.y += Random.Range(0f,100f) * 0.1f - 50f;
        transform.position = pos;
        */
    }
}
