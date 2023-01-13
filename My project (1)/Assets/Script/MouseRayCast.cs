using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRayCast : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] bool IsHold = false;
    int readyNum;

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            layerMask = LayerMask.GetMask("Character");

            //���� ������ �������� ��
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                MouseEvent mouseEvent = hit.transform.GetComponent<MouseEvent>();
                if (mouseEvent.characterState.CurState != State.Ready)
                    return;

                ReadySenter.Instance().OnStartPosCollider();
                readyNum = mouseEvent.indexNum;
                IsHold = true;
            }
        }
        else if(Input.GetMouseButtonUp(0) && IsHold)
        {
            layerMask = LayerMask.GetMask("StartPos");

            //���� ������ �������� ��
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Transform StartPosition = hit.transform;
                ReadySenter.Instance().ChracterChangePosition(readyNum, StartPosition);
            }

            //Collider�ٽ� ��Ȱ��ȭ 
            ReadySenter.Instance().OffStartPosCollider();

            IsHold = false;
        }
    }
}
