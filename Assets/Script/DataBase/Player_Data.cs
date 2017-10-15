using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Data : MonoBehaviour
{
    public static Player_Data Instance;

    public int goldValue;
    public int mineralValue;

    public Unit_Script[] partyMemberClassArr;

    public Unit_Script[] unitClassArr;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        yield break;
    }
}