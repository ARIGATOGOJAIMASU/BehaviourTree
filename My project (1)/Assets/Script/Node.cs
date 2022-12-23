using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    public State state = State.Running;
    public bool started = false;

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

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
