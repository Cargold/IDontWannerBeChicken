using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Data : MonoBehaviour
{
    public Player_Data Instance;

    public int goldValue;
    public int mineralValue;

    public Unit_Script[] partyMemberClassArr;

    void Awake()
    {
        Instance = this;
    }
}