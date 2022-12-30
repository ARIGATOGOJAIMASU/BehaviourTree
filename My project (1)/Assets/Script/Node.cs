using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{ 
    private Information _information;
    private Transform _ownerTransform;

    public Information Info { get { return _information; } set { _information = value; } }
    public Transform OwnerTransform { get { return _ownerTransform; } set { _ownerTransform = value; } }

    public enum State
    {
        Running,
        Failure,
        Success
    }

    public State state = State.Running;
    public bool started = false;
    public string guid;
    public Vector2 position;

    public State Update()
    {
        //�̹� ������Ʈ���� �˻縦 ������ ���� �ִ����� Ȯ��
        if(!started)
        {
            //ù �˻� �� �ʱ�ȭ�� ���� started�� true�� �ξ� 2�� ���� �ʰ� ����ó��
            OnStart();
            started = true;
        }

        //�ڽ��� State�� �������� �˻縦 ����
        //�ڽ��� ������ �ִ� ��ü�� �ڽĿ� ���� State���� �޶�����.
        //�ڽ��� ���� ActionNode�� ��� �ڽ��� Update�� ��� ���� State�� �ȴ�.
        state = OnUpdate();

        //Running�� ������ ������� ����� Ȱ���� �����.
        if(state == State.Failure || state == State.Success)
        {
            OnStop();
            started = false;
        }

        return state;
    }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
