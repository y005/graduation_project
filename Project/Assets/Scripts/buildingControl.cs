using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingControl : MonoBehaviour
{
    //�ִ� ��� ���� ����
    int lightLevel = 0;
    //�����Ӽ� ���� ����
    int frame = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //������Ʈ �Լ��� 1�ʿ� 60�� ������ ȣ��Ǳ� ������(60������)
        //0.5�� �ֱ�� ��⸦ ��ȭ�ϱ� ���ؼ��� ī��Ʈ ������ ���� ����� Ȯ��
        if(frame == 0)
        {
            lightLevel = (lightLevel + 1) % 3;
        }
        transform.GetChild(0).gameObject.GetComponent<Light>().intensity = lightLevel;
        frame = (frame + 1) % 60;
    }
}
