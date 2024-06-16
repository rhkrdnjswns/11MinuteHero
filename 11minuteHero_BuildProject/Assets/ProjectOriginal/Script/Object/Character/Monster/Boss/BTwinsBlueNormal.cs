using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTwinsBlueNormal : BTwinsBlueEasy
{
    protected override IEnumerator Co_Behavior_AttackHead()
    {
        animator.SetBool("UseSkillSelf", true);

        for (int i = 0; i < 3; i++)
        {
            decalList[(int)EDecalNumber.HeadAttack].transform.SetParent(null);
            decalList[(int)EDecalNumber.HeadAttack].transform.position = InGameManager.Instance.Player.transform.position;

            headAttackParticleList[i].transform.SetParent(null);
            headAttackParticleList[i].transform.position = decalList[(int)EDecalNumber.HeadAttack].transform.position;
            headAttackParticleList[i].Play();

            yield return StartCoroutine(decalList[(int)EDecalNumber.HeadAttack].Co_ActiveDecal(6, 2f));

            int num = Physics.OverlapSphereNonAlloc(decalList[(int)EDecalNumber.HeadAttack].transform.position, 3, headAttackCollisionArray, ConstDefine.LAYER_PLAYER);
            AttackInRangeUtility.AttackLayerInRange(headAttackCollisionArray, InGameManager.Instance.Player.MaxHp * 20 / 100, num);
        }
        decalList[(int)EDecalNumber.HeadAttack].InActiveDecal(transform);
    }
    protected override IEnumerator Co_Behavior_Targeting()
    {
        animator.SetBool("UseSkillSelf", true);

        for(int i = 0; i < 2; i++)
        {
            decalList[(int)EDecalNumber.Targeting].transform.SetParent(null);
            decalList[(int)EDecalNumber.Targeting].transform.position = InGameManager.Instance.Player.transform.position;

            yield return StartCoroutine(decalList[(int)EDecalNumber.Targeting].Co_ActiveDecal(new Vector3(2, 8, 1), 2f));

            targetingParticleList[i].transform.SetParent(null);
            targetingParticleList[i].transform.transform.forward = decalList[(int)EDecalNumber.Targeting].transform.right;
            targetingParticleList[i].transform.position = decalList[(int)EDecalNumber.Targeting].transform.position;
            targetingParticleList[i].transform.position -= targetingParticleList[i].transform.forward;
            targetingParticleList[i].Play();

            int num = Physics.OverlapBoxNonAlloc(decalList[(int)EDecalNumber.Targeting].transform.position, new Vector3(1, 1, 4), targetingCollisionArray, transform.rotation, ConstDefine.LAYER_PLAYER);
            if (num == 0) break;
            AttackInRangeUtility.AttackLayerInRange(targetingCollisionArray, InGameManager.Instance.Player.MaxHp * 20 / 100, num);
        }
        decalList[(int)EDecalNumber.Targeting].InActiveDecal(transform);
    }
    //protected override IEnumerator Co_Behavior_Star() //보류
    //{
    //    isPlayStar = true;
    //    animator.SetBool("UseSkillObj", true);
    //    for (int i = 0; i < 3; i++)
    //    {
    //        for (int j = (int)EDecalNumber.Star1; j < (int)EDecalNumber.Star1 + 5; j++)
    //        {
    //            //데칼 회전용 빈게임 오브젝트 하위로 넣어주고 초기화해줌
    //            decalList[j].transform.SetParent(decalRotationController);
    //            decalList[j].transform.localPosition = Vector3.zero;
    //            decalList[j].transform.localRotation = Quaternion.Euler(90, 0, 0);

    //            decalRotationController.position = jarPosArray[GetJarIndexByForLoopIndex(j)];
    //            decalRotationController.forward = GetDecalDirectionByJarIndex(j);
    //            decalRotationController.position += decalRotationController.forward * GetJarDistanceHalfByIndex(j);
    //            decalList[j].transform.SetParent(null);

    //            if (j == 6)
    //            {
    //                yield return StartCoroutine(decalList[j].Co_ActiveDecal(new Vector3(2, GetJarDistanceHalfByIndex(j) * 2, 1), 1f));
    //            }
    //            else
    //            {
    //                StartCoroutine(decalList[j].Co_ActiveDecal(new Vector3(2, GetJarDistanceHalfByIndex(j) * 2, 1), 1f));
    //            }
    //        }
    //    }
    //    for (int i = (int)EDecalNumber.Star1; i < (int)EDecalNumber.Star1 + 5; i++)
    //    {
    //        decalList[i].InActiveDecal(transform);
    //    }
    //    isPlayStar = false;
    //}
}
