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

    //온도
    Temperature TreeTemperature;    

    private void Awake()
    {
        TreeTemperature = GetComponent<Temperature>();
        TreeTemperature.SetDefaultDamage(Damage);
    }

    //무게 Get
    public float GetWeight()
    {
        return Weight;
    }

    //온도 Get
    public float GetTemprature()
    {
        return TreeTemperature.GetValue();
    }

    public bool IsDead()
    {
        return TreeTemperature.GetValue() == 0;
    }
}