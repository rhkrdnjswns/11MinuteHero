using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleUtility : MonoBehaviour
{
    private bool isTransparent; //현재 투명 상태인지 체크

    private Renderer renderer; //오브젝트의 렌더러 컴포넌트
    private float targetAlphaValue = 0.25f; //투명 처리시 적용할 목표 투명도 수치
    private float applyTime = 0.5f; //투명에서 불투명으로 돌아가기 위한 경과 시간 (투명 상태에서 0.5초 지나면 불투명으로 다시 돌아감)

    private bool isReseting;
    private float timer = 0f;


    private Coroutine setOpaqueCoroutine;
    private Coroutine timeCheckCoroutine;


    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        if(GetComponent<BoxCollider>().isTrigger) //오브젝트에 맞는 레이어 설정
        {
            gameObject.layer = LayerMask.NameToLayer("PenetrateObstacle");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Obstacle");
        }
    }

    public void SetTransparent() //오브젝트의 투명도를 현재 상태에 맞게 조절
    {
        if (isTransparent) //이미 투명 상태인 경우 timer를 초기화하여 투명 상태 끝나지 않게 유지
        {
            timer = 0f;
            return;
        }

        if (isReseting) //불투명으로 복구 중 다시 호출된 경우
        {
            isReseting = false;
            StopCoroutine(setOpaqueCoroutine); //복구 코루틴 중지
        }
        
        isTransparent = true;
        StartCoroutine(Co_SetTransparent()); //해당 함수 호출 최초에 한 번만 코루틴 실행
    }

    private void SetMaterialsRenderingMode(float mode, int renderQueue) //모든 머티리얼의 렌더링 모드를 Transparent로 변경 (투명 처리)
    {
        foreach (Material material in renderer.materials)
        {
            SetMaterialRenderingMode(material, mode, renderQueue);
        }
    }
    // 런타임 중에 렌더링 모드를 바꾸는 로직. 아래 모든 로직이 고정이라고 보면 되고, 매개변수로 받는 mode, renderQueue값만 달라진다.
    // mode : 0 = Opaque, 1 = Cutout, 2 = Fade, 3 = Transparent
    // renderQueue : 오브젝트가 그려지는 순서. 이미지의 sortingOrder라고 생각하면 된다. 불투명한 오브젝트와 반투명 오브젝트의 렌더링 처리에 주로 사용된다.
    // 각 렌더링 모드별 채널 범위
    // 1. Opaque : none (카메라 뷰와 오브젝트의 포지션에 따라 렌더링하기 때문에 여러 채널이 필요 없음)
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
    private IEnumerator Co_SetTransparent() //머티리얼을 투명하게 만드는 코루틴
    {
        SetMaterialsRenderingMode(3, 3000);  //머티리얼 렌더링 모드 Transparent로 변경 (투명 처리 하기 위해)

        while (true)
        {
            if (renderer.material.color.a > targetAlphaValue) //목표 투명도까지 투명도 낮추기
            {
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    Color color = renderer.materials[i].color;
                    color.a -= Time.deltaTime;
                    renderer.materials[i].color = color;
                }
            }
            else //목표 투명도까지 낮춘 경우 타이머 작동
            {
                CheckTime();
                break;
            }
            yield return null;
        }
    }
    private void CheckTime() //투명해진 후 다시 불투명해지기 까지의 시간 체크
    {
        if (timeCheckCoroutine != null) //이미 시간 체크 코루틴이 작동 중이었다면 중지하고, 새로 실행시킴
        {
            StopCoroutine(timeCheckCoroutine);
        }

        timeCheckCoroutine = StartCoroutine(CheckTimerCouroutine()); //시간 체크 코루틴 실행
    }

    private IEnumerator CheckTimerCouroutine() //투명해진 후 다시 불투명해지기 까지의 시간 체크 코루틴
    {
        timer = 0f; //코루틴이 실행 될 때마다 타이머를 0으로 초기화해줌

        while (true)
        {
            timer += Time.deltaTime;

            if (timer > applyTime) //applyTime만큼 경과한 경우
            {
                isReseting = true;
                setOpaqueCoroutine = StartCoroutine(Co_SetOpaque()); //불투명으로 복구 코루틴 실행
                break;
            }

            yield return null;
        }

    }
    private IEnumerator Co_SetOpaque()
    {
        isTransparent = false; //리셋 도중 다시 투명처리를 할 수 있도록 처리

        while (true)
        {
            if(renderer.material.color.a < 1f) //투명도 1로 원상복구
            {
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    Color color = renderer.materials[i].color;
                    color.a += Time.deltaTime;
                    renderer.materials[i].color = color;
                }
            }
            else //투명도 복구 완료시 렌더링 모드를 Opaque로 변경. 투명도 복구 이전에 변경하면 그래픽이 깨지는 듯한 현상이 발생한다.
            {
                isReseting = false;
                SetMaterialsRenderingMode(0, -1);
                break;
            }
            yield return null;
        }
    }
}
