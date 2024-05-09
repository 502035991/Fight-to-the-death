using CX;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;
    [Header("耐力再生")]
    [SerializeField] float staminaRegenerationAmount = 2f;
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 2;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    //根据耐力等级计算耐力
    public int CalculateStaminaBasedOnEndurancelevel(int endurance)
    {
        float stamina = 0;

        stamina += endurance * 10;

        return Mathf.RoundToInt(stamina);
    }
    public virtual void RegenerateStamina()
    {
        if (!character.IsOwner)
            return;
        if (character.characterNetworkManager.isSprinting.Value)
            return;
        if (character.isPerformingAction)
            return;
        staminaRegenerationTimer += Time.deltaTime;
        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;
                if (staminaTickTimer >= 0.1f)
                {
                    staminaTickTimer = 0;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                }
            }
        }
    }
    public virtual void ResetStaminaRegenerationTimer(float oldVlaue , float newVelue)
    {
        if(newVelue < oldVlaue)
            staminaRegenerationTimer = 0;
    }
}
