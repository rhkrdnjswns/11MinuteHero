using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MeleeWeapon
{
    private void Update() //파티클이 플레이 중인 경우 플레이어 위치를 따라다니도록 처리
    {
        if (particleSystem.isPlaying)
        {
            particleSystem.transform.position = InGameManager.Instance.Player.transform.position + (Vector3.up * 0.5f);
        }
    }
    public override void Attack()
    {
        base.Attack();
        //if (!bMaxLevel) return;
        foreach (Monster monster in inRangeMonsterList) //무기 최대 레벨 도달 시 몬스터 경직
        {
            //monster.SetRigid(1.5f);
        }
    }
    protected override void PlayParticle() //파티클 재배치 재정의. 창은 파티클이 플레이어 위치에 생성됨
    {
        particleSystem.transform.position = InGameManager.Instance.Player.transform.position + (Vector3.up * 0.5f);
        particleSystem.transform.rotation = InGameManager.Instance.Player.transform.rotation;
        particleSystem.Play();
    }
    
}