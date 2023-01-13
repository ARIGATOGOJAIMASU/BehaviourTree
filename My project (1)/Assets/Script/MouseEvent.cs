using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEvent : MonoBehaviour
{
    Vector3 StartPos;
    public CharacterState characterState;
    public int num;

    private void Start()
    {
        StartPos = transform.position;
        num = GetComponent<Information>().indexNum;
    }

    private void OnMouseDrag()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        transform.position = point;
    }

    private void OnMouseUp()
    {
        transform.position = StartPos;
    }

    private void OnMouseEnter()
    {
        if (characterState.GetState() == State.Battle)
            OnInfomation();
    }

    private void OnMouseExit()
    {
        if (characterState.GetState() == State.Battle)
            OffInfomation();
    }

    //StatUI호출 및 Setting
    public void OnInfomation()
    {
        BattleManager.Instance.statExpantionUI.SetStatExplanation(num);
        BattleManager.Instance.statExpantionUI.gameObject.SetActive(true);
    }

    //StatUI비활성화
    public void OffInfomation()
    {
        BattleManager.Instance.statExpantionUI.gameObject.SetActive(false);
    }
}
