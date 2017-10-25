using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Script : Character_Script
{
    public static Player_Script Instance;
    public Transform spawnPos;
    enum MoveDir
    {
        None,
        Left,
        Right,
    }
    [SerializeField]
    private MoveDir moveDir;

    public void BattleEnter_Func()
    {
        Instance = this;

        base.Init_Func(GroupType.Ally);
    }

    protected override void Idle_Func()
    {
        if (CheckTargetAlive_Func() == false)
            charState = CharacterState.Idle;
        else
            SetState_Func(CharacterState.Attack);
    }

    public void MoveLeft_Func()
    {
        moveDir = MoveDir.Left;
    }
    public void MoveRight_Func()
    {
        moveDir = MoveDir.Right;
    }
    public void MoveOver_Func()
    {
        moveDir = MoveDir.None;
    }
    void MovePlayer_Func()
    {
        if (moveDir == MoveDir.Left)
        {
            if (charState != CharacterState.Move)
            {
                SetState_Func(CharacterState.Move);
            }

            this.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        else if (moveDir == MoveDir.Right)
        {
            if (charState != CharacterState.Move)
            {
                SetState_Func(CharacterState.Move);
            }

            this.transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else
        {
            if (charState == CharacterState.Move)
                SetState_Func(CharacterState.Idle);
        }
    }
    protected override void Move_Func()
    {
        charState = CharacterState.Move;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) == true)
        {
            moveDir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.D) == true)
        {
            moveDir = MoveDir.Right;
        }
        else if (Input.GetKeyUp(KeyCode.A) == true || Input.GetKeyUp(KeyCode.D) == true)
        {
            moveDir = MoveDir.None;
        }

        MovePlayer_Func();
    }
}
