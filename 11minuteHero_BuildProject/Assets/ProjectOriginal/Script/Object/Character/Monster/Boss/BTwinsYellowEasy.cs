using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTwinsYellowEasy : Boss
{
    private System.Action<float> hitAction;
    public override void InitBoss()
    {
        base.InitBoss();
        gameObject.SetActive(false);
    }
    public void ConnectTwins(System.Action<float> action)
    {
        hitAction = action;
    }
    protected override void InitBehaviorTree()
    {
       // throw new System.NotImplementedException();
    }

    protected override void PlayHpEvent(int index)
    {
       // throw new System.NotImplementedException();
    }

    public override void ActiveBoss()
    {
        base.ActiveBoss();
        StartCoroutine(Co_StartAnim());
    }
    private IEnumerator Co_StartAnim()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.right);

        Vector3 target = transform.position + Vector3.left * 2f;

        transform.position += Vector3.left * bossAreaWidth;
        Vector3 pivot = transform.position;

        float timer = 0;
        while (timer < 2)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(pivot, target, timer / 2);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("AppearEnd");
    }
    public override void Hit(float damage)
    {
        if (!IsActivate) return;
        if (IsDie) return;
        DecreaseHp(damage);
        if(IsDie)
        {
            animator.SetBool("IsDizzy", true);
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        damageUIContainer.ActiveDamageUI(damage);
        hitAction(damage);
    }
    public void ResetYellow(float hp)
    {
        IsDie = false;
        currentHp = hp;
        gameObject.layer = LayerMask.NameToLayer("Monster");
        animator.SetBool("IsDizzy", false);
    }
}
