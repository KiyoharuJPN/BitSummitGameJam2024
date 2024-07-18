using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChargeUp
{
    void DoChargeUp(float chargepower);

    int LimitSkillTime();
}
