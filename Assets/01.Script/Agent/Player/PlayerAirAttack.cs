using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirAttack : MonoBehaviour
{
    public bool InAir{ get; private set; }

    public void SetAirState(bool isInAir)
    {
        InAir = isInAir;
    }
}
