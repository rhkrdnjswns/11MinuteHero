using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillAfterImage : MonoBehaviour
{
    private Transform rotTransform;
    [SerializeField] private ParticleSystem particle;
    public void SetAfterImage(SwordSkillObject.ESwordType eSwordType)
    {
        switch (eSwordType)
        {
            case SwordSkillObject.ESwordType.BloodSword:
                rotTransform = transform.GetChild(0).GetComponent<Transform>();
                break;
            case SwordSkillObject.ESwordType.CurseSword:
                rotTransform = transform;
                break;
            case SwordSkillObject.ESwordType.DevilSword:
                rotTransform = transform.GetChild(0).GetComponent<Transform>();
                break;
            default:
                break;
        }
    }
    public void SetAfterImage(Transform revAxis, Transform parent, Vector3 rotDir, float sec, float degree, Vector3 size, float rotateSpeed)
    {
        transform.localScale = size;
        gameObject.SetActive(true);

        particle.Play();
        StartCoroutine(Co_RotationSword(rotDir, rotateSpeed));
        StartCoroutine(Co_RevolutionSword(revAxis, parent, sec, degree));
    }
    private IEnumerator Co_RotationSword(Vector3 rotDir, float rotateSpeed) //검 오브젝트 자전
    {
        while (true)
        {
            rotTransform.Rotate(rotDir * rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator Co_RevolutionSword(Transform revAxis, Transform parent, float sec, float degree) //검 오브젝트 공전
    {
        float angle = 0;
        while (angle <= degree)
        {
            transform.RotateAround(revAxis.position, Vector3.up, degree / sec * Time.deltaTime);
            angle += degree / sec * Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
    }
}
