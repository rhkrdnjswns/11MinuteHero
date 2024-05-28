using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartelWall : Monster
{
    public System.Action dieAction;
    public System.Action<float> hitAction;
    public override void Hit(float damage)
    {
        base.Hit(damage);
        if(IsDie)
        {
            dieAction();
        }
    }

}
