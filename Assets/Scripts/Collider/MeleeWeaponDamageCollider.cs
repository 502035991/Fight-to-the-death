using CX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Attacking Character")]
    public CharacterManager characterCausingDamage;

    protected override void DamageTarget(CharacterManager damageTarget)
    {

        base.DamageTarget(damageTarget);
    }
}
