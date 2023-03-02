using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour
{
    [System.Serializable]
    public struct SoundList
    {
        public string Hit;
        public string Attack;
        public string Skill;
    }

    public enum SoundType { Hit, Attack, Skill }

    public SoundList soundList;

    //�ִϸ��̼� �̺�Ʈ�Լ�
    public void PlaySound(SoundType type)
    {
        switch (type)
        {
            case SoundType.Attack:
                SoundManager.Instance().Play(soundList.Attack);
                break;
            case SoundType.Hit:
                SoundManager.Instance().Play(soundList.Hit);
                break;
            case SoundType.Skill:
                SoundManager.Instance().Play(soundList.Skill);
                break;
        }
    }
}
