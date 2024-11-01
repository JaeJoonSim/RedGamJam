using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//온도
public class Temperature : MonoBehaviour
{
    //기본 최대 온도
    [SerializeField] private float DefaultMaxTemperature;    

    //온도 게이지가 최대 온도 게이지의 n%만큼 감소할 때마다 최대 온도 게이지가 m만큼 감소 m 0.00 ~ 1.00
    [SerializeField] private float Percent;

    //최대 온도 깎이는 값
    [SerializeField] private float DecreaseMaxTemperature;

    //현재 온도
    private float CurrentTemperature;

    //온도 변동 값 음수면 깎이고 양수면 증가
    private float Value;

    private float MaxTemperature;
    private float PrevTemperature;

    private void Start()
    {
        CurrentTemperature = MaxTemperature = DefaultMaxTemperature;
        PrevTemperature = CurrentTemperature;
    }

    //온도 변동 값 Set
    public void SetValue(float _value)
    {
        Value += _value;
    }

    //최대 온도 값 Set
    public void SetMaxTemperature(float _value)
    {
        MaxTemperature += _value;
    }

    //온도 변동 값 Get
    public float GetValue()
    {
        return Value;
    }

    //온도 값 Get
    public float GetTemperature()
    {
        return CurrentTemperature;
    }

    //온도 값 Set
    public float GetMaxTemperature()
    {
        return MaxTemperature;
    }

    //온도 계산
    public void SetTemperature()
    {
        //클램프로 0 ~ DefaultMaxTemperature 범위 제한
        CurrentTemperature = Mathf.Clamp(CurrentTemperature += Value, 0, DefaultMaxTemperature);

        //Debug.Log("현재 온도 : " + CurrentTemperature + "변동 값 : " + Value);

        int calcValue = 0;
        //온도 깎였을 때
        if (Value < 0)            
        {
            calcValue = (int)(DefaultMaxTemperature * Percent);

            //Debug.Log("calcValue : " + (PrevTemperature - calcValue));
            if (PrevTemperature - calcValue >= CurrentTemperature)
            {
                //MaxTemperature DecreaseMaxTemperature 만큼 감소
                PrevTemperature = CurrentTemperature;
                MaxTemperature = Mathf.Clamp(MaxTemperature -= DecreaseMaxTemperature, 0, DefaultMaxTemperature);
                //Debug.Log("최대 온도 : " + MaxTemperature);
            }
        }
    }
}