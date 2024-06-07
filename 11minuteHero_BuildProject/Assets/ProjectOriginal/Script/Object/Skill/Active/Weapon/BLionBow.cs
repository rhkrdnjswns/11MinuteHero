using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BLionBow : RBow
{
    private IEnumerator speedUpCoroutine;
    private int speedUpCount;
    private bool isSpeedUp;
    [SerializeField] private float speedUpValue;
    [SerializeField] private float speedUpDuration;
    public override void InitSkill()
    {
        base.InitSkill();

        InGameManager.Instance.Player.ChangeWeapon(this);
        rangedAttackUtility.SetCount(6);
        rangedAttackUtility.ShotCount = 6;
        arrowAngle = GetArrowAngle();
        speedUpCoroutine = Co_SetSpeed();
    }
    protected override void SetCurrentDamage()
    {
        CurrentDamage = damage;
        rangedAttackUtility.SetDamage(CurrentDamage);
    }
    public override void SetSpeedUp() //�÷��̾� ���ǵ� ���� �Լ�
    {
        if(!isSpeedUp)
        {
            isSpeedUp = true;
        }
        StopCoroutine(speedUpCoroutine); //�������̴� �ӵ� ���� �ڷ�ƾ ����

        speedUpCoroutine = Co_SetSpeed(); //�ӵ� ���� �ڷ�ƾ ���� ����
        StartCoroutine(speedUpCoroutine); //�ڷ�ƾ ����

        if (speedUpCount < 5) speedUpCount++; //���� ���ǵ� ������ �� �� �޾Ҵ��� üũ
    }
    private IEnumerator Co_SetSpeed()
    {
        if (speedUpCount < 5) //���ǵ� ���� ī��Ʈ�� 5ȸ �̻��� �Ǹ� ���ǵ�� �� �̻� �������� �ʰ� ���� �ð��� ���ŵ�.
        {
            InGameManager.Instance.Player.IncreaseSpeed(speedUpValue, EApplicableType.Value);
        }

        float timer = speedUpDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        //Ÿ�̸Ӱ� 0�ʰ� �Ǳ� ���� �ڷ�ƾ�� �����ϸ� �Ʒ� ������ ������� �ʴ´�.

        InGameManager.Instance.Player.DecreaseSpeed(speedUpValue * speedUpCount, EApplicableType.Value);//�̵��ӵ� ������� �ʱ�ȭ
        speedUpCount = 0;
        isSpeedUp = false;
    }
}
