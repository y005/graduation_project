using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//관심도 높은 경우 나오는 효과
//점점 커지면서 페이드 아웃하는 효과
public class shining : MonoBehaviour
{
    float time=0;
    float _fadeTime = 1f;
    public Vector3 pos;

    void Start()
    {
        time = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).gameObject.transform.position = pos;
        if (time < _fadeTime)
        {
            transform.GetChild(0).gameObject.transform.localScale = Vector3.one * (0.8f + time);
            transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1f - time / _fadeTime );
        }
        else if((time > _fadeTime) && (time < _fadeTime + 0.1f))
        {
            transform.GetChild(0).gameObject.transform.localScale = Vector3.one * (0.8f + time);
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
        transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
        transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.8f,0.8f,1f);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
