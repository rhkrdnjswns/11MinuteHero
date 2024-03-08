using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MeleeWeapon
{
    private void Update() //��ƼŬ�� �÷��� ���� ��� �÷��̾� ��ġ�� ����ٴϵ��� ó��
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
        foreach (Monster monster in inRangeMonsterList) //���� �ִ� ���� ���� �� ���� ����
        {
            //monster.SetRigid(1.5f);
        }
    }
    protected override void PlayParticle() //��ƼŬ ���ġ ������. â�� ��ƼŬ�� �÷��̾� ��ġ�� ������
    {
        particleSystem.transform.position = InGameManager.Instance.Player.transform.position + (Vector3.up * 0.5f);
        particleSystem.transform.rotation = InGameManager.Instance.Player.transform.rotation;
        particleSystem.Play();
    }
    
}