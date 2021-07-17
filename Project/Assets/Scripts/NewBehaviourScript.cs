using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private RaycastHit hit; //마우스에 클릭된 객체 
    private Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //카메라의 법선 좌표: transform.localRotation* Vector3.forward;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                Plane plane = new Plane(Vector3.up, hit.collider.gameObject.transform.position);
                Vector3 v3Center = new Vector3(0.5f, 0.5f, 0.0f);
                ray = Camera.main.ViewportPointToRay(v3Center);
                float fDist;
                //Camera.main.transform.Translate(transform.localRotation * Vector3.right);
                if (plane.Raycast(ray, out fDist))
                {
                    Vector3 v3Hit = ray.GetPoint(fDist);
                    Vector3 v3Delta = hit.collider.gameObject.transform.position - v3Hit;
                    //hit.collider.gameObject.transform.position = v3Hit;
                    //Camera.main.transform.Translate(v3Delta);
                    Camera.main.transform.Translate(transform.localRotation * v3Delta);
                }
            }
        }
    }
}