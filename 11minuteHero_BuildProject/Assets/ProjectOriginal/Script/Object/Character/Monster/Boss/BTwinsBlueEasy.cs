using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTwinsBlueEasy : Boss
{
    protected enum EDecalNumber
    {
        HeadAttack = 0,
        Targeting,
        Star1,
        Star2,
        Star3,
        Star4,
        Star5,
        Salvo1,
        Salvo2,
        Salvo3,
        Salvo4,
        Salvo5,
    }
    protected Vector3 appearPos;
    protected BTwinsYellowEasy twinsYellow;
    protected Vector3[] jarStandPosArray = new Vector3[5];
    protected Vector3[] jarPosArray = new Vector3[5];

    protected bool[] isBehaviorNotReady;

    public int currentJarIndex;

    protected bool isSearch;
    protected WaitForSeconds searchDelay = new WaitForSeconds(2f);
    protected WaitForSeconds moveDelay = new WaitForSeconds(1f);
    protected WaitForSeconds healDelay = new WaitForSeconds(4f);
    protected WaitForSeconds headAttackDelay = new WaitForSeconds(2f);
    protected WaitForSeconds targetingDelay = new WaitForSeconds(2f);
    protected WaitForSeconds starDelay = new WaitForSeconds(1f);
    protected WaitForSeconds salvoDelay = new WaitForSeconds(1f);

    protected List<ParticleSystem> headAttackParticleList = new List<ParticleSystem>();
    protected List<ParticleSystem> targetingParticleList = new List<ParticleSystem>();

    protected Collider[] headAttackCollisionArray = new Collider[1];
    protected Collider[] targetingCollisionArray = new Collider[1];

    protected Transform starDecalRotationController;
    protected Transform salvoDecalRotationController;
    protected override void InitBehaviorTree()
    {
        //throw new System.NotImplementedException();
    }

    protected override void PlayHpEvent(int index)
    {
        //throw new System.NotImplementedException();
    }
    public override void InitBoss()
    {
        base.InitBoss();
        Transform headAttackParticle = transform.Find("Effect_HeadAttack");
        for (int i = 0; i < headAttackParticle.childCount; i++)
        {
            headAttackParticleList.Add(headAttackParticle.GetChild(i).GetComponent<ParticleSystem>());
        }
        Transform targetingParticle = transform.Find("Effect_Targeting");
        for (int i = 0; i < targetingParticle.childCount; i++)
        {
            targetingParticleList.Add(targetingParticle.GetChild(i).GetComponent<ParticleSystem>());
        }

        Transform decalParent = transform.Find("Decal");
        for (int i = 0; i < decalParent.childCount; i++)
        {
            if(decalParent.GetChild(i).childCount > 1)
            {
                for (int j = 0; j < decalParent.GetChild(i).childCount; j++)
                {
                   decalList.Add(decalParent.GetChild(i).GetChild(j).GetComponent<Decal>());
                }
            }
            else
            {
                decalList.Add(decalParent.GetChild(i).GetComponent<Decal>());
            }
        }

        twinsYellow = transform.GetComponentInChildren<BTwinsYellowEasy>();
        twinsYellow.InitBoss();
        twinsYellow.ConnectTwins(Hit);

        isBehaviorNotReady = new bool[4];

        starDecalRotationController = transform.Find("StarDecalRotationController");
        salvoDecalRotationController = transform.Find("SalvoDecalRotationController");
        starDecalRotationController.SetParent(null);
        salvoDecalRotationController.SetParent(null);
    }
    private IEnumerator Co_Passive_IncreaseYellowSpeedByDistanceFromPlayer()
    {
        WaitForSeconds speedUpDuration = new WaitForSeconds(5f);

        while(true)
        {
            yield return null;

            float distanceYellow = (InGameManager.Instance.Player.transform.position - twinsYellow.transform.position).sqrMagnitude;
            float distanceBlue = (InGameManager.Instance.Player.transform.position - transform.position).sqrMagnitude;

            if(distanceYellow < distanceBlue)
            {
                float speedIncrease = twinsYellow.Speed * 50 / 100;
                twinsYellow.CurrentSpeed += speedIncrease;

                yield return speedUpDuration;

                twinsYellow.CurrentSpeed -= speedIncrease;

                yield return speedUpDuration;
            }
        }
    }
    protected IEnumerator Co_Behavior_MoveToJar()
    {
        int previousJarIndex = currentJarIndex;
        while(previousJarIndex == currentJarIndex)
        {
            currentJarIndex = Random.Range(0, jarStandPosArray.Length);
        }

        while (Vector3.Distance(transform.position, jarStandPosArray[currentJarIndex]) > 0.5f)
        {
            animator.SetBool(ConstDefine.BOOL_ISMOVE, Move());
            if (bHpEvent)
            {
                animator.SetBool(ConstDefine.BOOL_ISMOVE, false);
                yield break;
            }
            yield return null;
        }
        animator.SetBool(ConstDefine.BOOL_ISMOVE, false);

        transform.LookAt(appearPos);

        StartCoroutine(Co_Behavior_Searching());
    }
    protected virtual IEnumerator Co_Behavior_Searching()
    {
        isSearch = true;
        animator.SetBool("IsSearch", isSearch);
        yield return searchDelay;
        isSearch = false;
        animator.SetBool("IsSearch", isSearch);

        if(twinsYellow.IsDie)
        {
            yield return StartCoroutine(Co_Behavior_HealYellow());
        }
        else
        {
            //int rand = GetBehaviorByWeight(60, 70, 60, 60);
            int rand = GetBehaviorByWeight(1, 1, 1, 60);
            while (isBehaviorNotReady[rand] == true)
            {
                rand = GetBehaviorByWeight(60, 70, 60, 60);
            }
            isBehaviorNotReady[rand] = true;
            switch (rand)
            {
                case 0:
                    StartCoroutine(Co_Behavior_AttackHead());
                    yield return headAttackDelay;
                    animator.SetBool("UseSkillSelf", false);
                    break;
                case 1:
                    StartCoroutine(Co_Behavior_Targeting());
                    yield return targetingDelay;
                    animator.SetBool("UseSkillSelf", false);
                    break;
                case 2:
                    StartCoroutine(Co_Behavior_Star());
                    yield return starDelay;
                    animator.SetBool("UseSkillObj", false);
                    break;
                case 3:
                    StartCoroutine(Co_Behavior_Salvo());
                    yield return salvoDelay;
                    animator.SetBool("UseSkillObj", false);
                    break;
                default:
                    Debug.LogError("Behavior Weight Length is Not Valid");
                    break;
            }
        }
        yield return moveDelay;
        StartCoroutine(Co_Behavior_MoveToJar());
    }
    protected virtual IEnumerator Co_Behavior_AttackHead()
    {
        animator.SetBool("UseSkillSelf", true);
        decalList[(int)EDecalNumber.HeadAttack].transform.SetParent(null);
        decalList[(int)EDecalNumber.HeadAttack].transform.position = InGameManager.Instance.Player.transform.position;

        headAttackParticleList[0].transform.SetParent(null);
        headAttackParticleList[0].transform.position = decalList[(int)EDecalNumber.HeadAttack].transform.position;
        headAttackParticleList[0].Play();

        yield return StartCoroutine(decalList[(int)EDecalNumber.HeadAttack].Co_ActiveDecal(6, 2f));

        int num = Physics.OverlapSphereNonAlloc(decalList[(int)EDecalNumber.HeadAttack].transform.position, 3, headAttackCollisionArray, ConstDefine.LAYER_PLAYER);
        AttackInRangeUtility.AttackLayerInRange(headAttackCollisionArray, InGameManager.Instance.Player.MaxHp * 20 / 100, num);

        decalList[(int)EDecalNumber.HeadAttack].InActiveDecal(transform);

        Debug.Log("머리 찍기");

        isBehaviorNotReady[0] = false;
    }
    protected virtual IEnumerator Co_Behavior_Targeting()
    {
        animator.SetBool("UseSkillSelf", true);
        decalList[(int)EDecalNumber.Targeting].transform.SetParent(null);
        decalList[(int)EDecalNumber.Targeting].transform.position = InGameManager.Instance.Player.transform.position;

        yield return StartCoroutine(decalList[(int)EDecalNumber.Targeting].Co_ActiveDecal(new Vector3(2, 8, 1), 2f));

        targetingParticleList[0].transform.SetParent(null);
        targetingParticleList[0].transform.transform.forward = decalList[(int)EDecalNumber.Targeting].transform.right;
        targetingParticleList[0].transform.position = decalList[(int)EDecalNumber.Targeting].transform.position;
        targetingParticleList[0].transform.position -= targetingParticleList[0].transform.forward;
        targetingParticleList[0].Play();

        int num = Physics.OverlapBoxNonAlloc(decalList[(int)EDecalNumber.Targeting].transform.position, new Vector3(1, 1, 4), targetingCollisionArray, transform.rotation, ConstDefine.LAYER_PLAYER);
        AttackInRangeUtility.AttackLayerInRange(targetingCollisionArray, InGameManager.Instance.Player.MaxHp * 20 / 100, num);

        decalList[(int)EDecalNumber.Targeting].InActiveDecal(transform);

        Debug.Log("지적 타겟");

        isBehaviorNotReady[1] = false;
    }
    protected virtual IEnumerator Co_Behavior_Star()
    {
        animator.SetBool("UseSkillObj", true);
        for (int i = (int)EDecalNumber.Star1; i < (int)EDecalNumber.Star1 + 5; i++)
        {
            //데칼 회전용 빈게임 오브젝트 하위로 넣어주고 초기화해줌
            decalList[i].transform.SetParent(starDecalRotationController);
            decalList[i].transform.localPosition = Vector3.zero;
            decalList[i].transform.localRotation = Quaternion.Euler(90, 0, 0);

            starDecalRotationController.position = jarPosArray[GetJarIndexByForLoopIndex(i)] - Vector3.up * 0.5f;
            starDecalRotationController.forward = GetDecalDirectionByJarIndex(i);
            starDecalRotationController.position += starDecalRotationController.forward * GetJarDistanceHalfByIndex(i);
            decalList[i].transform.SetParent(null);

            if (i == (int)EDecalNumber.Star5)
            {
                yield return StartCoroutine(decalList[i].Co_ActiveDecal(new Vector3(2, GetJarDistanceHalfByIndex(i) * 2, 1), 7f));
            }
            else
            {
                StartCoroutine(decalList[i].Co_ActiveDecal(new Vector3(2, GetJarDistanceHalfByIndex(i) * 2, 1), 7f));
            }
        }
        for (int i = (int)EDecalNumber.Star1; i < (int)EDecalNumber.Star1 + 5; i++)
        {
            decalList[i].InActiveDecal(transform);
        }
        Debug.Log("별");
        animator.SetBool("UseSkillObj", false);

        isBehaviorNotReady[2] = false;
    }
    protected IEnumerator Co_Behavior_Salvo()
    {
        animator.SetBool("UseSkillObj", true);
        for (int i = (int)EDecalNumber.Salvo1; i < (int)EDecalNumber.Salvo1 + 5; i++)
        {
            //데칼 회전용 빈게임 오브젝트 하위로 넣어주고 초기화해줌
            decalList[i].transform.SetParent(salvoDecalRotationController);
            decalList[i].transform.localPosition = Vector3.zero;
            decalList[i].transform.localRotation = Quaternion.Euler(90, 0, 0);

            salvoDecalRotationController.position = jarPosArray[i - (int)EDecalNumber.Salvo1] - Vector3.up * 0.5f;
            salvoDecalRotationController.forward = (InGameManager.Instance.Player.transform.position - salvoDecalRotationController.position).normalized;
            //salvoDecalRotationController.forward = ()
            float distance = (InGameManager.Instance.Player.transform.position - salvoDecalRotationController.position).sqrMagnitude;
            distance = Mathf.Clamp(distance, 0, 20);

            salvoDecalRotationController.position += salvoDecalRotationController.forward * distance;

            decalList[i].transform.SetParent(null);

            if (i == (int)EDecalNumber.Salvo5)
            {
                yield return StartCoroutine(decalList[i].Co_ActiveDecal(new Vector3(2, 40, 1), 5f));
            }
            else
            {
                StartCoroutine(decalList[i].Co_ActiveDecal(new Vector3(2, 40, 1), 5f));
            }
        }
        for (int i = (int)EDecalNumber.Salvo1; i < (int)EDecalNumber.Salvo1 + 5; i++)
        {
            decalList[i].InActiveDecal(transform);
        }
        Debug.Log("일제 사격");
        isBehaviorNotReady[3] = false;
    }
    protected Vector3 GetDecalDirectionByJarIndex(int index)
    {
        Vector3 dir = Vector3.zero;
        switch (index)
        {
            case 2:
                dir = (jarPosArray[3] - jarPosArray[0]).normalized;
                break;
            case 3:
                dir = (jarPosArray[1] - jarPosArray[3]).normalized;
                break;
            case 4:
                dir = (jarPosArray[4] - jarPosArray[1]).normalized;
                break;
            case 5:
                dir = (jarPosArray[2] - jarPosArray[4]).normalized;
                break;
            case 6:
                dir = (jarPosArray[0] - jarPosArray[2]).normalized;
                break;
            default:
                break;
        }
        return dir;
    }
    protected float GetJarDistanceHalfByIndex(int index)
    {
        float distance = 0;
        switch (index)
        {
            case 2:
                distance = Vector3.Distance(jarPosArray[3], jarPosArray[0]) / 2;
                break;
            case 3:
                distance = Vector3.Distance(jarPosArray[1], jarPosArray[3]) / 2;
                break;
            case 4:
                distance = Vector3.Distance(jarPosArray[4], jarPosArray[1]) / 2;
                break;
            case 5:
                distance = Vector3.Distance(jarPosArray[2], jarPosArray[4]) / 2;
                break;
            case 6:
                distance = Vector3.Distance(jarPosArray[0], jarPosArray[2]) / 2;
                break;
            default:
                break;
        }
        return distance;
    }
    protected int GetJarIndexByForLoopIndex(int index)
    {
        int i = 0;
        switch (index)
        {
            case 2:
                i = 0;
                break;
            case 3:
                i = 3;
                break;
            case 4:
                i = 1;
                break;
            case 5:
                i = 4;
                break;
            case 6:
                i = 2;
                break;
            default:
                break;
        }
        return i;
    }
    protected IEnumerator Co_Behavior_HealYellow()
    {
        animator.SetBool("UseSkillObj", true);
        yield return healDelay;
        twinsYellow.ResetYellow(twinsYellow.MaxHp);
        animator.SetBool("UseSkillObj", false);
    }
    protected override bool Move()
    {
        Vector3 direction = (jarStandPosArray[currentJarIndex] - transform.position).normalized;
        transform.forward = direction;
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
        return true;
    }
    public override void ActiveBoss()
    {
        base.ActiveBoss();
        appearPos = transform.position;
        twinsYellow.transform.SetParent(null);
        StartCoroutine(Co_StartAnim());
    }
    private IEnumerator Co_StartAnim()
    {
        Vector3 target = transform.position + Vector3.back * 2f;
        transform.position += Vector3.back * bossAreaHeight;
        Vector3 pivot = transform.position;
        float timer = 0;
        while(timer < 2)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(pivot, target, timer / 2);
            yield return null;
        }
        animator.SetTrigger("AppearEnd");
        twinsYellow.ActiveBoss();
        yield return new WaitForSeconds(2.5f);
        modelObject.transform.rotation = Quaternion.LookRotation(Vector3.forward + Vector3.left);
        animator.SetTrigger("AppearEnd");

    }
    public override float GetStartAnimPlayTime()
    {
        return 6.5f;
    }
    public override void ActiveBossFunction()
    {
        SetJar();
        modelObject.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        base.ActiveBossFunction();
        StartCoroutine(Co_Behavior_MoveToJar());
        twinsYellow.ActiveBossFunction();
        StartCoroutine(Co_Passive_IncreaseYellowSpeedByDistanceFromPlayer());
    }
    private void SetJar()
    {
        Transform jarParent = transform.Find("Jars");
        jarParent.SetParent(null);
        for (int i = 0; i < jarStandPosArray.Length; i++)
        {
            Vector3 distanceToDirection = Vector3.zero;

            switch (i)
            {
                case 0:
                    jarStandPosArray[i] = transform.position + Vector3.forward * (bossAreaHeight / 2 + 2);
                    distanceToDirection = Vector3.forward * (bossAreaHeight / 2 - 2);
                    break;
                case 1:
                    jarStandPosArray[i] = transform.position + Vector3.right * (bossAreaWidth / 2 + 2);
                    distanceToDirection = Vector3.right * (bossAreaWidth / 2 - 2);
                    break;
                case 2:
                    jarStandPosArray[i] = transform.position + (Vector3.back * (bossAreaHeight / 2 + 2)) + (Vector3.right * (bossAreaWidth / 4));
                    distanceToDirection = (Vector3.back * (bossAreaHeight / 2 - 2)) + (Vector3.right * (bossAreaWidth / 4)); 
                    break;
                case 3:
                    jarStandPosArray[i] = transform.position + (Vector3.back * (bossAreaHeight / 2 + 2)) + (Vector3.left * (bossAreaWidth / 4));
                    distanceToDirection = (Vector3.back * (bossAreaHeight / 2 - 2)) + (Vector3.left * (bossAreaWidth / 4));
                    break;
                case 4:
                    jarStandPosArray[i] = transform.position + Vector3.left * (bossAreaWidth / 2 + 2);
                    distanceToDirection = Vector3.left * (bossAreaWidth / 2 - 2);
                    break;
                default:
                    Debug.LogError("Jar Count is Not Valid. Out Of Range");
                    break;
            }
            jarParent.GetChild(i).position += distanceToDirection;
            jarPosArray[i] = jarParent.GetChild(i).position;
            jarParent.GetChild(i).gameObject.SetActive(true);
        }
    }
}
