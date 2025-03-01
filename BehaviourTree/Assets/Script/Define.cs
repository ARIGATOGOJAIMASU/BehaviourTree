using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    [SerializeField]
    Transform[] DefinePos;

    private void Awake()
    {
        EnemyFrontPosition = DefinePos[0].position;
        EnemyBackPosition = DefinePos[1].position;
        EnemyMidPosition = DefinePos[2].position;
        TeamFrontPosition = DefinePos[3].position;
        TeamBackPosition = DefinePos[4].position;
        TeamMidPosition = DefinePos[5].position;
        ActionFrontPoint = DefinePos[6].position;
    }

    public static Vector3 EnemyFrontPosition;
    public static Vector3 EnemyBackPosition;
    public static Vector3 EnemyMidPosition;
    public static Vector3 TeamFrontPosition;
    public static Vector3 TeamBackPosition;
    public static Vector3 TeamMidPosition;
    public static Vector3 ActionFrontPoint;
}
