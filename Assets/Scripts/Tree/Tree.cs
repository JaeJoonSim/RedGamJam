using BlueRiver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//나무
[RequireComponent(typeof(Temperature))]
public class Tree : MonoBehaviour
{
    public string LocalizationKey;
    public string treeDamageKey;
    //무게
    public float Weight;
    // UI 표시 무게
    public float DisplayWeight;
    //데미지
    public float Damage;
    //회복량
    public float RecoverValue;
    //데미지 시간
    public float DamageTime;

    //회복 정도는 현재 최대 온도 게이지의 n% 만큼 회복(움막)
    [SerializeField] private float RecoverPercent;


    //온도
    private Temperature TreeTemperature;
    private Coroutine CurrentCoroutine;
    private Coroutine CurrentCoroutine2;

    private float ReduceDamagePercent;

    private bool OutDoor;

    private void Awake()
    {
        TreeTemperature = GetComponent<Temperature>();
        StartDamageLoop();
    }

    public float GetWeight()
    {
        return Weight;
    }

    //온도 일시 정지
    public void PauseDamageTime(bool _value)
    {
        //퍼즈
        if (TreeTemperature != null)
            TreeTemperature.SetPause(_value);
    }

    //데미지 루프 시작
    public void StartDamageLoop()
    {
        CurrentCoroutine = StartCoroutine(DamageLoop());
    }

    //데미지 루프 끝
    public void EndDamageLoop()
    {
        StopCoroutine(DamageLoop());
    }

    //힐 루프 시작
    public void StartRecoverLoop(float _maxTime)
    {
        CurrentCoroutine2 = StartCoroutine(RecoverLoop(_maxTime));
    }

    public void SetOutDoor(bool _value)
    {
        OutDoor = _value;
    }

    //온도 감소 루프
    IEnumerator DamageLoop()
    {
        yield return new WaitForSecondsRealtime(DamageTime);

        if(OutDoor)
        {
            TakeDamage(Damage * ReduceDamagePercent);
        }
        else
        {
            TakeDamage();
        }
        

        Coroutine coroutine = CurrentCoroutine;
        StopCoroutine(coroutine);
        CurrentCoroutine = StartCoroutine(DamageLoop());
    }

    //온도 증가 루프
    IEnumerator RecoverLoop(float _maxTime)
    {
        float Count = _maxTime;

        yield return new WaitForSecondsRealtime(1.0f);

        RecoverByValue();

        Coroutine coroutine = CurrentCoroutine2;

        StopCoroutine(coroutine);

        if(Count > 0)
        {
            Count -= 1;
            CurrentCoroutine2 = StartCoroutine(DamageLoop());
        }
    }

    void PlayDeath()
    {
        if (IsDead())
        {
            SoundManager.Instance.PlaySound("Death");
        }
    }

    //나무 데미지만큼 온도 감소
    public void TakeDamage()
    {
        TreeTemperature.TakeDamage(Damage);
        PlayDeath();
    }

    //외부적 요인으로 온도 감소
    public void TakeDamage(float _value)
    {
        TreeTemperature.TakeDamage(_value);
        PlayDeath();
    }

    //나무 자체 힐량으로 온도 회복
    public void RecoverByValue()
    {
        TreeTemperature.Recover(RecoverValue);
    }

    //회복 정도는 현재 최대 온도 게이지의 n% 만큼 회복 (움막)
    public void RecoverByMaxTemperature()
    {
        TreeTemperature.Recover(TreeTemperature.GetMaxTemperature() * RecoverPercent);
    }

    //Outdoor는 Default와 마찬가지로 야외이지만, 해당 Platform에서는 묘목의 Temp_Damage의 n%만큼 감소량이 감소됨    Ex) Temp_Damage: 20, Outdoor: 20%, 총 초당 감소량: 16
    public void TakeReduceDamage(float _percent)
    {
        TreeTemperature.TakeDamage(Damage * _percent);
    }

    //죽었나연?
    public bool IsDead()
    {
        return TreeTemperature.GetTemperature() == 0;
    }

    public float GetTemperature()
    {
        if (TreeTemperature == null)
            TreeTemperature = GetComponent<Temperature>();

        return TreeTemperature.GetTemperature();
    }

    public float GetMaxTemperature()
    {
        if (TreeTemperature == null)
            TreeTemperature = GetComponent<Temperature>();

        return TreeTemperature.GetMaxTemperature();
    }

    public void SetReduceDamagePercent(float _value)
    {
        ReduceDamagePercent = _value;
    }
}