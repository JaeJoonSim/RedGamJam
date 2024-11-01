using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//온도 매니저
public class TemperatureManager : MonoBehaviour
{
    //온도들이 바뀌는 시간 주기
    [SerializeField]float ChangeTime;

    //온도들 담은 리스트
    [SerializeField]List<Temperature> TemperatureList = new List<Temperature>();

    Coroutine CurrentCoroutine;

    public void Start()
    {
        BeginTemperatureCalc();
    }

    //온도 일시 정지
    public void PauseTemperature(Temperature _temperature, float _pauseTime)
    {
        //온도 찾음
        Temperature temperature = TemperatureList.Find(t => t == _temperature);

        //퍼즈
        temperature.SetPause(true);

        //코루틴
        StartCoroutine(UnPauseTemperature(temperature, _pauseTime));
    }

    //온도 변화 시작 (게임 시작시)
    public void BeginTemperatureCalc()
    {
        CurrentCoroutine = StartCoroutine(TemperatureCalc());
    }

    //온도 변화 끝 (게임 끝날시)
    public void EndTemperatureCalc()
    {
        StopCoroutine(CurrentCoroutine);
    }

    //온도 변화 루프
    IEnumerator TemperatureCalc()
    {
        yield return new WaitForSeconds(ChangeTime);

        for (int i = 0; i < TemperatureList.Count; i++)
        {
            TemperatureList[i].SetTemperature();
        }

        Coroutine coroutine = CurrentCoroutine;
        StopCoroutine(coroutine);        
        CurrentCoroutine = StartCoroutine(TemperatureCalc());
    }

    IEnumerator UnPauseTemperature(Temperature _temperature, float _pauseTime)
    {
        //퍼즈 시간 지나면
        yield return new WaitForSeconds(_pauseTime);

        //퍼즈 해제
        _temperature.SetPause(false);
    }
}