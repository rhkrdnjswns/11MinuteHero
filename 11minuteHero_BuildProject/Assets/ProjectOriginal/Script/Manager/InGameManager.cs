using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI; //Test

public enum EGameState //인게임 상태 체크. GameOver면 모든 코루틴 작동 정지함.
{
    Start,
    Play,
    GameOver
}
public class InGameManager : MonoBehaviour
{
    private static InGameManager instance; //싱글턴 인스턴스
    private CPlayer player; //플레이어 참조. 모든 플레이어 참조는 인게임매니저의 플레이어를 참조함.

    [SerializeField] private GameObject[] playerCharacterArray; //플레이어 캐릭터 배열. 플레이 씬 입장 시 인덱스를 전달받아 선택한 캐릭터를 활성화
    [SerializeField] private int characterIndex;
    [SerializeField] private List<Monster> monsterList = new List<Monster>(); //현재 필드 위에 존재하는 몬스터 리스트
                                                                              //[SerializeField] private CustomPriorityQueue<Monster> monster
    private SkillManager skillManager;
    private BossUIManager bossUIManager;

    [SerializeField] private GameObject[] bossPrefabArray;

    private Boss currentBoss;
    private CameraUtility cameraUtility;

    private int killCount;

    #region --Test--
    public Text timerText;
    public Text killCountText;

    public Image fade;
    #endregion

    public bool bAppearBoss { get; private set; }

    private EGameState gameState; //현재 게임 상태

    private MonsterPool monsterPool; //몬스터 풀의 참조를 필요로하는 객체들이 접근할 수 있도록 싱글턴 클래스에서 인스턴스화함

    public static InGameManager Instance { get => instance; }
    public CPlayer Player { get => player; }
    public List<Monster> MonsterList { get => monsterList; set => monsterList = value; }
    public EGameState GameState
    { //게임 스테이트가 GameOver가 되면 gameOver 델리게이트 호출. GameOver가 되는 경우는 플레이어 hp가 0이 되는 경우밖에 없음
        get
        {
            return gameState;
        }
        set
        {
            gameState = value;
            if (gameState == EGameState.GameOver) dGameOver();
        }
    }

    public delegate void LevelUpEvent(); // 플레이어 레벨업 시 발생하는 이벤트들의 트리거 작용을 할 델리게이트 정의

    public delegate void StopAllCoroutinesWhenGameOver(); //게임오버 시 모든 코루틴 정지를 위한 델리게이트 정의

    private StopAllCoroutinesWhenGameOver dGameOver; //코루틴을 사용하는 인게임 클래스에서 델리게이트 체인을 통해 해당 델리게이트에 StopAllCoroutines 함수 추가.
    private LevelUpEvent dLevelUp; //레벨업 시 발생하는 모든 이벤트 함수를 해당 델리게이트에 추가
    public StopAllCoroutinesWhenGameOver DGameOver { get => dGameOver; set => dGameOver = value; }
    public LevelUpEvent DLevelUp { get => dLevelUp; set => dLevelUp = value; }
    public MonsterPool MonsterPool { get => monsterPool; }

    public Boss CurrentBoss { get => currentBoss; }
    public int KillCount
    {
        get
        {
            return killCount;
        }
        set
        {
            killCount = value;
            killCountText.text = killCount.ToString();
        }
    }

    public SkillManager SkillManager { get => skillManager; }

    public BossUIManager BossUIManager { get => bossUIManager; }
    private void Awake() //싱글턴으로 인스턴스화. 인게임 내에서는 씬전환이 일어나지 않기 때문에 인게임 씬 로드 시에만 this로 초기화 해주면 됨.
    {
        instance = this;
        //characterIndex = 0;//GameTest.Instance.index;
        Application.targetFrameRate = 120;//ConstDefine.TARGET_FRAME; //프레임 조정
        Screen.SetResolution(1280, 720, true);

        GameObject obj = Instantiate(playerCharacterArray[characterIndex]); //캐릭터 생성 후 참조를 가져옴
        player = obj.GetComponent<CPlayer>();
        monsterPool = FindObjectOfType<MonsterPool>();
        skillManager = FindObjectOfType<SkillManager>();

        bossUIManager = FindObjectOfType<BossUIManager>();

        // * 스테이지마다 인덱스 다르게 적용해야함
        GameObject boss = Instantiate(bossPrefabArray[0]);
        boss.transform.position = Vector3.zero;
        currentBoss = boss.GetComponent<Boss>();
        boss.SetActive(false);

        cameraUtility = Camera.main.GetComponent<CameraUtility>();

        StartCoroutine(TweeningUtility.FadeOut(2f, fade));
    }
    //아래 로직은 전부 테스트 (09/25)
    private void Start()
    {
        dGameOver += GameOverDebug;
        dLevelUp += LevelUpDebug;
        StartCoroutine(Co_Timer());
    }
    private IEnumerator Co_Timer()
    {
        float timer = 0;

        while (gameState != EGameState.GameOver)
        {
            timer += Time.deltaTime;
            timerText.text = ((int)timer / 60).ToString("00") + ":" + ((int)timer % 60).ToString("00");
            yield return null;
        }
    }
    private void GameOverDebug()
    {
        Debug.Log("Game Over!");
    }
    private void LevelUpDebug()
    {
        Debug.Log("Level Up!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(Co_AppearBoss());
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(cameraUtility.Co_ShakeCam(0.2f, 1, 0.1f));
        }
    }
    public void IncreaseKillCount()
    {
        killCount++;
    }
    public IEnumerator Co_AppearBoss()
    {
        bAppearBoss = true;
        //몹 생성 정지 -> 몬스터스포너

        //타이머 정지 -> 인게임매니저

        //보스 UI 출력 -> 보스UI매니저
        StartCoroutine(bossUIManager.Co_AppearBoss());

        //보스 출현 사운드 실행 -> 보스

        //카메라 연출 -> 카메라팔로우
        currentBoss.transform.position = randomBossPos(); //일정 거리의 랜덤한 위치 지정
        yield return StartCoroutine(cameraUtility.Co_FocusCam(2.5f, player.transform.position, currentBoss.transform.position)); //카메라 이동

        //보스 체력바 활성화, 보스 오브젝트 활성화
        player.InActiveExpBar();
        bossUIManager.ActiveBossHpBar();
        currentBoss.gameObject.SetActive(true);

        //보스 구역 생성
        currentBoss.CreateBossArea();

        //보스 등장 애니메이션만큼 대기
        yield return new WaitForSeconds(currentBoss.GetStartAnimPlayTime());

        //보스 위치 화살표 UI 생성 -> 보스UI매니저
        bossUIManager.SetBossDirectionArrow();

        //카메라 위치 플레이어로 이동
        yield return StartCoroutine(cameraUtility.Co_FocusCam(0.5f, currentBoss.transform.position, player.transform.position));
        cameraUtility.UnFocus();

        //보스 동작 시작
        currentBoss.InitBoss();
    }
    private Vector3 randomBossPos()
    {
        float x = Random.Range(0f, 20f);
        float z = x < 15 ? Random.Range(15f, 20f) : Random.Range(0f, 20f); //결정된 x위치값에 따라 z위치값을 결정

        int xOperation = Random.Range(0, 2) * 2 - 1; // -1 or 1 중 하나 선택
        x *= xOperation;

        int zOperation = Random.Range(0, 2) * 2 - 1;
        z *= zOperation;

        Vector3 pos = new Vector3(player.transform.position.x + x, 0, player.transform.position.z + z); //플레이어 위치 기준으로 소환 위치를 잡음 

        return pos;
    }
}
