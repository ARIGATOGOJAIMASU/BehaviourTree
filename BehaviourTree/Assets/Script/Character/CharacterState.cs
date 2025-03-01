using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Ready, Battle }

public class CharacterState : MonoBehaviour
{
    public State characterState = State.Ready;

    public State CurState { get { return characterState; } set { characterState = value; } }
}
