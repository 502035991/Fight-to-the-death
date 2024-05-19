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
    private float staminaRegenerationTimer = 0;//耐力回复计时器
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 1;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    protected virtual void Start()
    {

    }
    /// <summary>
    /// 根据体力等级计算体力
    /// </summary>
    public int CalculateHealthBasedOnVitalityLevel(int vitality)
    {
        float health = 0;

        health += vitality * 15;

        return Mathf.RoundToInt(health);
    }
    /// <summary>
    /// 根据耐力等级计算耐力
    /// </summary>
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
        //停止降低耐力后 staminaRegenerationDelay 后回复
        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                //character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                staminaTickTimer += Time.deltaTime;
                if (staminaTickTimer >= 0.05f)
                {
                    staminaTickTimer = 0;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                }
            }
        }
    }
    public virtual void ResetStaminaRegenerationTimer(float oldVlaue , float newVelue)
    {
        //重置计时器
        if(newVelue < oldVlaue)
            staminaRegenerationTimer = 0;
    }
}
