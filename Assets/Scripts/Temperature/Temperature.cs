using BlueRiver;
using BlueRiver.UI;
using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//온도
public class Temperature : MonoBehaviour
{
    [SerializeField] float MaxTemperature;

    //온도 게이지가 최대 온도 게이지의 n%만큼 감소할 때마다 최대 온도 게이지가 m만큼 감소 m 0.00 ~ 1.00
    [SerializeField] private float ReducePercent;

    //최대 온도 깎이는 값
    [SerializeField] private float DecreaseMaxTemperature;

    //현재 온도
    private float CurrentTemperature = 1f;
    
    private float PrevTemperature;

    private bool IsPause;

    private void OnEnable()
    {
        ConversationManager.OnConversationStarted += SetStartPause;
        ConversationManager.OnConversationEnded += SetStopPause;
    }

    private void OnDisable()
    {
        ConversationManager.OnConversationStarted -= SetStartPause;
        ConversationManager.OnConversationEnded -= SetStopPause;
    }

    private void Awake()
    {
        CurrentTemperature = MaxTemperature;
        PrevTemperature = CurrentTemperature;
    }

    public void Recover(float _value)
    {
        CurrentTemperature = Mathf.Clamp(CurrentTemperature += _value, 0, MaxTemperature);
    }


    private void SetStartPause()
    {
        SetPause(true);
    }

    private void SetStopPause()
    {
        SetPause(false);
    }

    public void SetPause(bool _value)
    {
        IsPause = _value;
    }

    //온도 계산
    public void TakeDamage(float _value)
    {
        if (IsPause)
        {
            return;
        }

        CurrentTemperature = Mathf.Clamp(CurrentTemperature -= _value, 0, MaxTemperature);
        Debug.Log("현재 온도" + CurrentTemperature);

        int calcValue = 0;
        calcValue = (int)(MaxTemperature * ReducePercent);
        Debug.Log("calcValue" + calcValue);
        if (PrevTemperature - calcValue >= CurrentTemperature)
        {
            PrevTemperature = CurrentTemperature;
            MaxTemperature -= DecreaseMaxTemperature;
            if(MaxTemperature <= 0)
            {
                CurrentTemperature = MaxTemperature = 0;
            }
        }

        Debug.Log("최대 온도" + MaxTemperature);
    }

    public float GetMaxTemperature()
    {
        return MaxTemperature;
    }

    public float GetTemperature()
    {
        return CurrentTemperature;
    }
}