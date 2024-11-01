using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//온도
public class Temperature : MonoBehaviour
{
    //기본 최대 온도
    [SerializeField] private float DefaultMaxTemperature;    

    //온도 게이지가 최대 온도 게이지의 n%만큼 감소할 때마다 최대 온도 게이지가 m만큼 감소 m 0.00 ~ 1.00
    [SerializeField] private float ReducePercent;

    //최대 온도 깎이는 값
    [SerializeField] private float DecreaseMaxTemperature;

    //현재 온도
    private float CurrentTemperature;

    private float MaxTemperature;
    private float PrevTemperature;

    private bool IsPause;

    private void Start()
    {
        CurrentTemperature = MaxTemperature = DefaultMaxTemperature;
        PrevTemperature = CurrentTemperature;
    }

    public void Recover(float _value)
    {
        CurrentTemperature = Mathf.Clamp(CurrentTemperature += _value, 0, MaxTemperature);
    }

    public void RecoverMaxTemperature(float _value)
    {
        CurrentTemperature = Mathf.Clamp(CurrentTemperature += _value, 0, MaxTemperature);
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

        int calcValue = 0;
        calcValue = (int)(DefaultMaxTemperature * ReducePercent);

        if (PrevTemperature - calcValue >= CurrentTemperature)
        {
            PrevTemperature = CurrentTemperature;
            MaxTemperature = Mathf.Clamp(MaxTemperature -= DecreaseMaxTemperature, 0, DefaultMaxTemperature);
        }
    }

    public float GetMaxTemperature()
    {
        return MaxTemperature;
    }
}