using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//나무
public class Tree : MonoBehaviour
{
    //무게
    [SerializeField]float Weight;
    //데미지
    [SerializeField]float Damage;
    //회복량
    [SerializeField]float RecoverValue;
    //데미지 시간
    [SerializeField]float DamageTime;

    //회복 정도는 현재 최대 온도 게이지의 n% 만큼 회복(움막)
    [SerializeField] private float RecoverPercent;


    //온도
    private Temperature TreeTemperature;
    private Coroutine CurrentCoroutine;
    private Coroutine CurrentCoroutine2;

    private void Awake()
    {
        TreeTemperature = GetComponent<Temperature>();
    }

    public float GetWeight()
    {
        return Weight;
    }

    //온도 일시 정지
    public void PauseDamageTime(Temperature _temperature, float _pauseTime)
    {
        //퍼즈
        TreeTemperature.SetPause(true);

        //코루틴
        StartCoroutine(UnPauseTemperature(_pauseTime));
    }

    //데미지 루프 시작
    public void StartDamageLoop()
    {
        StartCoroutine(DamageLoop());
    }

    //데미지 루프 끝
    public void EndDamageLoop()
    {
        StopCoroutine(DamageLoop());
    }

    //힐 루프 시작
    public void StartRecoverLoop(int _maxTime)
    {
        StartCoroutine(RecoverLoop(_maxTime));
    }

    //온도 감소 루프
    IEnumerator DamageLoop()
    {
        yield return new WaitForSeconds(DamageTime);

        TakeDamage();

        Coroutine coroutine = CurrentCoroutine;
        StopCoroutine(coroutine);
        CurrentCoroutine = StartCoroutine(DamageLoop());
    }

    //온도 증가 루프
    IEnumerator RecoverLoop(int _maxTime)
    {
        int Count = _maxTime;

        yield return new WaitForSeconds(1.0f);

        RecoverByValue();

        Coroutine coroutine = CurrentCoroutine2;

        StopCoroutine(coroutine);

        if(Count > 0)
        {
            Count -= 1;
            CurrentCoroutine2 = StartCoroutine(DamageLoop());
        }
    }

    IEnumerator UnPauseTemperature(float _pauseTime)
    {
        //퍼즈 시간 지나면
        yield return new WaitForSeconds(_pauseTime);

        //퍼즈 해제
        TreeTemperature.SetPause(false);
    }

    //나무 데미지만큼 온도 감소
    public void TakeDamage()
    {
        TreeTemperature.TakeDamage(Damage);
    }

    //나무 자체 힐량으로 온도 회복
    public void RecoverByValue()
    {
        TreeTemperature.Recover(RecoverValue);
    }


    //밑에 부터는 적재 적소에 호출해 주세용~~
    //회복 정도는 현재 최대 온도 게이지의 n% 만큼 회복 (움막)
    public void RecoverByMaxTemperature()
    {
        TreeTemperature.Recover(TreeTemperature.GetMaxTemperature() * RecoverPercent);
    }

    //외부적 요인으로 온도 감소
    public void TakeDamage(float _value)
    {
        TreeTemperature.TakeDamage(_value);
    }

    //죽었나연?
    public bool IsDead()
    {
        return TreeTemperature.GetTemperature() == 0;
    }
}