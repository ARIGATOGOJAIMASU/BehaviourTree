using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRayCast : MonoBehaviour
{
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            //맞을 때까지 레이저를 쏨
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(ray.origin, ray.direction * 20, Color.red, 5f);
            }
        }
    }
}
