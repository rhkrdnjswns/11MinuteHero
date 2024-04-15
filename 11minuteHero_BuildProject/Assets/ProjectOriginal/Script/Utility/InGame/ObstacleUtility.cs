using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleUtility : MonoBehaviour
{
    private bool isTransparent; //���� ���� �������� üũ

    private Renderer renderer; //������Ʈ�� ������ ������Ʈ
    private float targetAlphaValue = 0.25f; //���� ó���� ������ ��ǥ ���� ��ġ
    private float applyTime = 0.5f; //������ ���������� ���ư��� ���� ��� �ð� (���� ���¿��� 0.5�� ������ ���������� �ٽ� ���ư�)

    private bool isReseting;
    private float timer = 0f;


    private Coroutine setOpaqueCoroutine;
    private Coroutine timeCheckCoroutine;


    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        if(GetComponent<BoxCollider>().isTrigger) //������Ʈ�� �´� ���̾� ����
        {
            gameObject.layer = LayerMask.NameToLayer("PenetrateObstacle");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Obstacle");
        }
    }

    public void SetTransparent() //������Ʈ�� ������ ���� ���¿� �°� ����
    {
        if (isTransparent) //�̹� ���� ������ ��� timer�� �ʱ�ȭ�Ͽ� ���� ���� ������ �ʰ� ����
        {
            timer = 0f;
            return;
        }

        if (isReseting) //���������� ���� �� �ٽ� ȣ��� ���
        {
            isReseting = false;
            StopCoroutine(setOpaqueCoroutine); //���� �ڷ�ƾ ����
        }
        
        isTransparent = true;
        StartCoroutine(Co_SetTransparent()); //�ش� �Լ� ȣ�� ���ʿ� �� ���� �ڷ�ƾ ����
    }

    private void SetMaterialsRenderingMode(float mode, int renderQueue) //��� ��Ƽ������ ������ ��带 Transparent�� ���� (���� ó��)
    {
        foreach (Material material in renderer.materials)
        {
            SetMaterialRenderingMode(material, mode, renderQueue);
        }
    }
    // ��Ÿ�� �߿� ������ ��带 �ٲٴ� ����. �Ʒ� ��� ������ �����̶�� ���� �ǰ�, �Ű������� �޴� mode, renderQueue���� �޶�����.
    // mode : 0 = Opaque, 1 = Cutout, 2 = Fade, 3 = Transparent
    // renderQueue : ������Ʈ�� �׷����� ����. �̹����� sortingOrder��� �����ϸ� �ȴ�. �������� ������Ʈ�� ������ ������Ʈ�� ������ ó���� �ַ� ���ȴ�.
    // �� ������ ��庰 ä�� ����
    // 1. Opaque : none (ī�޶� ��� ������Ʈ�� �����ǿ� ���� �������ϱ� ������ ���� ä���� �ʿ� ����)
    // 2. Cutout : 2450 ~ 2500
    // 3. Fade : 2501 ~ 3999
    // 4. Transparent : 2501 ~ 3999
    private void SetMaterialRenderingMode(Material material, float mode, int renderQueue)
    {
        material.SetFloat("_Mode", mode);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = renderQueue;
    }
    private IEnumerator Co_SetTransparent() //��Ƽ������ �����ϰ� ����� �ڷ�ƾ
    {
        SetMaterialsRenderingMode(3, 3000);  //��Ƽ���� ������ ��� Transparent�� ���� (���� ó�� �ϱ� ����)

        while (true)
        {
            if (renderer.material.color.a > targetAlphaValue) //��ǥ �������� ���� ���߱�
            {
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    Color color = renderer.materials[i].color;
                    color.a -= Time.deltaTime;
                    renderer.materials[i].color = color;
                }
            }
            else //��ǥ �������� ���� ��� Ÿ�̸� �۵�
            {
                CheckTime();
                break;
            }
            yield return null;
        }
    }
    private void CheckTime() //�������� �� �ٽ� ������������ ������ �ð� üũ
    {
        if (timeCheckCoroutine != null) //�̹� �ð� üũ �ڷ�ƾ�� �۵� ���̾��ٸ� �����ϰ�, ���� �����Ŵ
        {
            StopCoroutine(timeCheckCoroutine);
        }

        timeCheckCoroutine = StartCoroutine(CheckTimerCouroutine()); //�ð� üũ �ڷ�ƾ ����
    }

    private IEnumerator CheckTimerCouroutine() //�������� �� �ٽ� ������������ ������ �ð� üũ �ڷ�ƾ
    {
        timer = 0f; //�ڷ�ƾ�� ���� �� ������ Ÿ�̸Ӹ� 0���� �ʱ�ȭ����

        while (true)
        {
            timer += Time.deltaTime;

            if (timer > applyTime) //applyTime��ŭ ����� ���
            {
                isReseting = true;
                setOpaqueCoroutine = StartCoroutine(Co_SetOpaque()); //���������� ���� �ڷ�ƾ ����
                break;
            }

            yield return null;
        }

    }
    private IEnumerator Co_SetOpaque()
    {
        isTransparent = false; //���� ���� �ٽ� ����ó���� �� �� �ֵ��� ó��

        while (true)
        {
            if(renderer.material.color.a < 1f) //���� 1�� ���󺹱�
            {
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    Color color = renderer.materials[i].color;
                    color.a += Time.deltaTime;
                    renderer.materials[i].color = color;
                }
            }
            else //���� ���� �Ϸ�� ������ ��带 Opaque�� ����. ���� ���� ������ �����ϸ� �׷����� ������ ���� ������ �߻��Ѵ�.
            {
                isReseting = false;
                SetMaterialsRenderingMode(0, -1);
                break;
            }
            yield return null;
        }
    }
}
