using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Ready, Battle }

public class CharacterState : MonoBehaviour
{
    State characterState = State.Ready;

    public State GetState()
    {
        return characterState;
    }
}
