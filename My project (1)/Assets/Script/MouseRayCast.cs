using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRayCast : MonoBehaviour
{
    public float distance;
    [SerializeField] LayerMask layerMask;

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
            if (Physics.Raycast(ray, out hit, Mathf.Infinity,layerMask))
            {
                Information info  = hit.transform.GetComponent<Information>();
                if (info.characterState.GetState() != State.Ready)
                    return;
            }
        }
    }
}
