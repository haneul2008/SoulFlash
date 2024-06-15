using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Agent
{
    public void ResetHp()
    {
        HealthCompo.ResetHealth(HealthCompo.MaxHealth);
    }
    public override void SetDeadState()
    {
    }
}
