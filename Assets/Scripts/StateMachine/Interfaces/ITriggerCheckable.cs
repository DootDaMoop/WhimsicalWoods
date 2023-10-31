using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckable
{
    bool isAggro {get; set; }
    bool isWithinAttackRadius {get; set;}
    void setAggroStatus(bool aggroStatus);
    void setIsWithinAttackRadius(bool isWithinAttackRadius);
}
