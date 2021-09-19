using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//관심도 낮은 경우 나오는 효과
//위로 이동하며 페이드 아웃하는 효과 코드
public class sleeping: MonoBehaviour
{
    public float time;
    float _fadeTime = 1f;
    Vector3 dir;
    public Vector3 pos;

    void Start()
    {
        time = 0f;
        dir = new Vector3(0.8f,1f, 0).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (time < _fadeTime)
        {
            transform.GetChild(0).gameObject.transform.Translate(50f*dir*Time.deltaTime);
            transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1f - time / _fadeTime);
        }
        else if ((time > _fadeTime) && (time < _fadeTime + 0.2f))
        {
            transform.GetChild(0).gameObject.transform.Translate(50f*dir*Time.deltaTime);
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            resetAnim();
        }
        time += Time.deltaTime;
    }

    public void resetAnim()
    {
        time = 0;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
        transform.GetChild(0).gameObject.transform.position = pos;
    }
}
