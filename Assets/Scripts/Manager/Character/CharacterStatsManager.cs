using CX;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;
    [Header("��������")]
    [SerializeField] float staminaRegenerationAmount = 2f;
    private float staminaRegenerationTimer = 0;//�����ظ���ʱ��
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
    /// ���������ȼ���������
    /// </summary>
    public int CalculateHealthBasedOnVitalityLevel(int vitality)
    {
        float health = 0;

        health += vitality * 15;

        return Mathf.RoundToInt(health);
    }
    /// <summary>
    /// ���������ȼ���������
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
        //ֹͣ���������� staminaRegenerationDelay ��ظ�
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
        //���ü�ʱ��
        if(newVelue < oldVlaue)
            staminaRegenerationTimer = 0;
    }
}
