using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rising : MonoBehaviour
{
    float time;
    public Vector3 pos;

    // Update is called once per frame
    void Update()
    {
        if (time < 0.4f) //특정 위치에서 원점으로 이동
        {
            transform.GetChild(0).gameObject.transform.position = pos - new Vector3(0, 60 - 150 * time, 0);
        }
        else if (time < 0.5f) // 튕기고
        {
            transform.GetChild(0).gameObject.transform.position = pos - new Vector3(0, time - 0.4f, 0) * 40;
        }
        else if (time < 0.6f) //다시 제자리로
        {
            transform.GetChild(0).gameObject.transform.position = pos - new Vector3(0, 0.6f - time, 0) * 40;
        }
        else if (time < 0.7f) //튕기고
        {
            transform.GetChild(0).gameObject.transform.position = pos - new Vector3(0, (time - 0.6f) / 2, 0) * 40;
        }
        else if (time < 0.8f) //다시 제자리
        {
            transform.GetChild(0).gameObject.transform.position = pos - new Vector3(0, 0.05f - (time - 0.7f) / 2, 0) * 40;
        }
        else
        {
            resetAnim();
        }

        time += Time.deltaTime;

    }
    public void resetAnim()
    {
        transform.GetChild(0).gameObject.transform.position = pos;
        time = 0;
    }
}
