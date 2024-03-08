using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileMotionUtility
{
    /// <summary>
    /// ������ ��� �ʱ� �ӵ� ��ȯ
    /// V(�ʱ�ӵ�) = d(���� ��� �Ÿ�) / (sin2��(�߻� ������ ���� ����(����) * 2) / g(�߷� ���ӵ�)
    /// ���� ��� �Ÿ��� �߻� ������ ���� �ʱ� �ӵ��� ���ϴ� ����
    /// </summary>
    /// <param name="distance">���� ��� �Ÿ�</param>
    /// <param name="angle">����</param>
    /// <returns></returns>
    public static float GetProjectileVelocity(float distance, float angle)
    {
        return distance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / ConstDefine.GRAVITY);
    }
    /// <summary>
    /// ������ ��� Vx ��ȯ
    /// Vx(x���������� �ʱ� �ӵ�) = V(�ʱ� �ӵ�) * cos��(�߻簢���� ���� �ڻ���(�غ�))
    /// �ܺ� ���� ���� �߷� ���ӵ��� �����ϴ� ȯ�濡�� x���������� �ӵ��� �׻� �ʱ� �ӵ��̴�.
    /// </summary>
    /// <param name="velocity">������ ��� �ʱ� �ӵ�</param>
    /// <param name="angle">����</param>
    /// <returns></returns>
    public static float GetVelocityX(float velocity, float angle)
    { 
        return Mathf.Sqrt(velocity) * Mathf.Cos(angle * Mathf.Deg2Rad);
    }
    /// <summary>
    /// ������ ��� Vy ��ȯ
    /// Vy(y���������� �ʱ� �ӵ�) = V(�ʱ� �ӵ�) * sin��(�߻簢���� ���� ����(����))
    /// �ܺ� ���� ���� �߷� ���ӵ��� �����ϴ� ȯ�濡�� y���������� �ӵ��� Vy(y���������� �ʱ� �ӵ�) - (g(�߷� ���ӵ�) * t(ü�� �ð�))�̴�. 
    /// Vy = 0 �̸� �������� �ְ� ���̿� ������ ��
    /// </summary>
    /// <param name="velocity">������ ��� �ʱ� �ӵ�</param>
    /// <param name="angle">����</param>
    /// <returns></returns>
    public static float GetVelocityY(float velocity, float angle)
    {
        return Mathf.Sqrt(velocity) * Mathf.Sin(angle * Mathf.Deg2Rad);
    }
}
