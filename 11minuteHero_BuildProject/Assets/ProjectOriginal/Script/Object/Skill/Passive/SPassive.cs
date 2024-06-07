using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SPassive : Skill //패시브 스킬 클래스.
{
    //모든 패시브 스킬은 종류에 맞는 수치의 현재 수치를 초기 수치의 percentage%만큼 증가시키거나 감소시킨다.
    [SerializeField] protected int percentage; //증감값
}
