using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionLionBow : ActiveBow
{
    private Coroutine speedUpCoroutine;
    private int speedUpCount;
    private int currentSpeedUpCount;
    private bool isSpeedUp;
    [SerializeField] private float speedUpValue;
    [SerializeField] private float speedUpDuration;
    public override void ActivateSkill()
    {
        base.ActivateSkill();

        InGameManager.Instance.Player.ChangeWeapon(this);
    }
    private void SetSpeedUp() //�÷��̾� ���ǵ� ���� �Լ�
    {
        if(!isSpeedUp)
        {
            isSpeedUp = true;
        }

        if(speedUpCoroutine != null) //���ǵ� ������ ������ ���� �ٽ� ȣ��� ���
        {
            StopCoroutine(speedUpCoroutine);
        }
        speedUpCoroutine = StartCoroutine(Co_SetSpeed()); //�ڷ�ƾ ����

        if (currentSpeedUpCount < speedUpCount) currentSpeedUpCount++; //���� ���ǵ� ������ �� �� �޾Ҵ��� üũ
    }
    private IEnumerator Co_SetSpeed()
    {
        if (currentSpeedUpCount < speedUpCount) //���ǵ� ���� ī��Ʈ�� 5ȸ �̻��� �Ǹ� ���ǵ�� �� �̻� �������� �ʰ� ���� �ð��� ���ŵ�.
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

        InGameManager.Instance.Player.DecreaseSpeed(speedUpValue * currentSpeedUpCount, EApplicableType.Value);//�̵��ӵ� ������� �ʱ�ȭ
        currentSpeedUpCount = 0;
        isSpeedUp = false;
    }
    protected override void InitProjectile()
    {
        base.InitProjectile();

        projectileUtility.SetAction(SetSpeedUp);
    }
    protected override void ReadEvolutionCSVData()
    {
        base.ReadEvolutionCSVData();

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 11);
        currentShotCount = shotCount;

        activateTime = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 12);
        speed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 13);

        penetrationCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 15);
        currentPenetrationCount = penetrationCount;

        speedUpValue = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 29);
        speedUpCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 30);
        speedUpDuration = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 31);
    }
}
