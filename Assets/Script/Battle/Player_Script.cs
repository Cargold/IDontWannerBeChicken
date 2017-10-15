using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Script : Character_Script
{
    public static Player_Script Instance;
    public Unit_Script[] playerUnitClassArr;
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

        for (int i = 0; i < playerUnitClassArr.Length; i++)
        {
            GetSpawn_Func(playerUnitClassArr[i]);
        }
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
        else if(Input.GetKeyUp(KeyCode.A) == true || Input.GetKeyUp(KeyCode.D) == true)
        {
            moveDir = MoveDir.None;
        }

        MovePlayer_Func();
    }

    protected override void Move_Func()
    {
        charState = CharacterState.Move;
    }

    void GetSpawn_Func(Unit_Script _unitClass)
    {
        StartCoroutine(CheckSpawn_Cor(_unitClass));
    }

    IEnumerator CheckSpawn_Cor(Unit_Script _unitClass)
    {
        while (true)
        {
            yield return new WaitForSeconds(_unitClass.spawnInterval);

            for (int i = 0; i < _unitClass.spawnNum; i++)
            {
                GameObject _charObj = ObjectPoolManager.Instance.Get_Func(_unitClass.charName);

                Vector3 _spawnPos = new Vector3(spawnPos.position.x + Random.Range(-0.5f, 0.5f), 0f, Random.Range(-1f, 1f));

                _charObj.transform.position = _spawnPos;
                _charObj.transform.localScale = Vector3.one;

                Unit_Script _spawnUnitClass = _charObj.GetComponent<Unit_Script>();
                _spawnUnitClass.Init_Func(GroupType.Ally);
            }
        }
    }
}
