using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState //�ΰ��� ���� üũ. GameOver�� ��� �ڷ�ƾ �۵� ������.
{
    Start,
    Play,
    GameOver
}
public class InGameManager : MonoBehaviour
{
    private static InGameManager instance; //�̱��� �ν��Ͻ�
    private CPlayer player; //�÷��̾� ����. ��� �÷��̾� ������ �ΰ��ӸŴ����� �÷��̾ ������.

    [SerializeField] private GameObject[] playerCharacterArray; //�÷��̾� ĳ���� �迭. �÷��� �� ���� �� �ε����� ���޹޾� ������ ĳ���͸� Ȱ��ȭ
    [SerializeField] private int characterIndex;
    [SerializeField] private List<NormalMonster> monsterList = new List<NormalMonster>(); //���� �ʵ� ���� �����ϴ� ���� ����Ʈ
                                                                              //[SerializeField] private CustomPriorityQueue<Monster> monster
    private SkillManager skillManager;
    private BossUIManager bossUIManager;
    private InGameBasicUIManager inGameBasicUIManager;
    private ItemManager itemManager;
    private CSVManager csvManager;

    [SerializeField] private GameObject[] bossPrefabArray;

    private Boss currentBoss;
    private CameraUtility cameraUtility;
    private GrayscaleUtility grayscaleUtility;

    private int killCount;

    public int CharacterIndex { get => characterIndex; }
    public float Timer { get; private set; }
    public bool bTimeStop { get; set; }
    public bool bAppearBoss { get; private set; }

    private EGameState gameState; //���� ���� ����

    private MonsterPool monsterPool; //���� Ǯ�� ������ �ʿ���ϴ� ��ü���� ������ �� �ֵ��� �̱��� Ŭ�������� �ν��Ͻ�ȭ��

    public static InGameManager Instance { get => instance; }
    public CPlayer Player { get => player; }
    public List<NormalMonster> MonsterList { get => monsterList; set => monsterList = value; }
    public EGameState GameState { get; }

    public delegate void LevelUpEvent(); // �÷��̾� ������ �� �߻��ϴ� �̺�Ʈ���� Ʈ���� �ۿ��� �� ��������Ʈ ����

    public delegate void GameOverEvent(); //���ӿ��� �� ��� �ڷ�ƾ ������ ���� ��������Ʈ ����

    private GameOverEvent dGameOver; //�ڷ�ƾ�� ����ϴ� �ΰ��� Ŭ�������� ��������Ʈ ü���� ���� �ش� ��������Ʈ�� StopAllCoroutines �Լ� �߰�.
    private LevelUpEvent dLevelUp; //������ �� �߻��ϴ� ��� �̺�Ʈ �Լ��� �ش� ��������Ʈ�� �߰�
    public GameOverEvent DGameOver { get => dGameOver; set => dGameOver = value; }
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
            inGameBasicUIManager.KillCountText.text = killCount.ToString();
        }
    }
    public SkillManager SkillManager { get => skillManager; }
    public BossUIManager BossUIManager { get => bossUIManager; }
    public InGameBasicUIManager InGameBasicUIManager { get => inGameBasicUIManager; }
    public ItemManager ItemManager { get => itemManager; }
    public CSVManager CSVManager { get => csvManager; }
    public GrayscaleUtility GrayscaleUtility { get => grayscaleUtility; }


    private void Awake() //�̱������� �ν��Ͻ�ȭ. �ΰ��� �������� ����ȯ�� �Ͼ�� �ʱ� ������ �ΰ��� �� �ε� �ÿ��� this�� �ʱ�ȭ ���ָ� ��.
    {
        instance = this;
        //characterIndex = 0;//GameTest.Instance.index;
        Application.targetFrameRate = 120;//ConstDefine.TARGET_FRAME; //������ ����
        Screen.SetResolution(1280, 720, true);

        if (GameManager.instance == null) characterIndex = 1;
        else
        {
            characterIndex = GameManager.instance.characterIndex;
        }

        GameObject obj = Instantiate(playerCharacterArray[characterIndex]); //ĳ���� ���� �� ������ ������
        player = obj.GetComponent<CPlayer>();
        monsterPool = FindObjectOfType<MonsterPool>();

        skillManager = FindObjectOfType<SkillManager>();
        bossUIManager = FindObjectOfType<BossUIManager>();
        inGameBasicUIManager = FindObjectOfType<InGameBasicUIManager>();
        itemManager = FindObjectOfType<ItemManager>();
        csvManager = FindObjectOfType<CSVManager>();

        // * ������������ �ε��� �ٸ��� �����ؾ���
        GameObject boss = Instantiate(bossPrefabArray[0]);
        boss.transform.position = Vector3.zero;
        currentBoss = boss.GetComponent<Boss>();
        boss.SetActive(false);

        cameraUtility = Camera.main.GetComponent<CameraUtility>();
        grayscaleUtility = Camera.main.GetComponent<GrayscaleUtility>();
    }
    //�Ʒ� ������ ���� �׽�Ʈ (09/25)
    private void Start()
    {
        dGameOver += GameOverDebug;
        dLevelUp += LevelUpDebug;
        StartCoroutine(Co_Timer());
    }
    private IEnumerator Co_Timer()
    {
        while (gameState != EGameState.GameOver || !bAppearBoss)
        {
            Timer += Time.deltaTime;
            inGameBasicUIManager.SetTimer((int)Timer / 60, (int)Timer % 60);
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
        Transform t = GetComponent<Transform>();
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(Co_AppearBoss());
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            dLevelUp();
        }
    }
    public void IncreaseKillCount()
    {
        killCount++;
    }
    public IEnumerator Co_AppearBoss()
    {
        bAppearBoss = true;

        //���� UI ��� -> ����UI�Ŵ���
        StartCoroutine(bossUIManager.Co_AppearBoss());

        //���� ���� ���� ���� -> ����

        //ī�޶� ���� -> ī�޶��ȷο�
        currentBoss.transform.position = randomBossPos(); //���� �Ÿ��� ������ ��ġ ����
        yield return StartCoroutine(cameraUtility.Co_FocusCam(2.5f, player.transform.position, currentBoss.transform.position)); //ī�޶� �̵�

        //���� ü�¹� Ȱ��ȭ, ���� ������Ʈ Ȱ��ȭ
        inGameBasicUIManager.InActiveExpBar();
        bossUIManager.ActiveBossHpBar();
        currentBoss.gameObject.SetActive(true);

        //���� ���� ����
        currentBoss.CreateBossArea();

        //���� ���� �ִϸ��̼Ǹ�ŭ ���
        yield return new WaitForSeconds(currentBoss.GetStartAnimPlayTime());

        //���� ��ġ ȭ��ǥ UI ���� -> ����UI�Ŵ���
        bossUIManager.SetBossDirectionArrow();

        //ī�޶� ��ġ �÷��̾�� �̵�
        yield return StartCoroutine(cameraUtility.Co_FocusCam(0.5f, currentBoss.transform.position, player.transform.position));
        cameraUtility.UnFocus();

        //���� ���� ����
        currentBoss.InitBoss();
    }
    private Vector3 randomBossPos()
    {
        float x = Random.Range(0f, 20f);
        float z = x < 15 ? Random.Range(15f, 20f) : Random.Range(0f, 20f); //������ x��ġ���� ���� z��ġ���� ����

        int xOperation = Random.Range(0, 2) * 2 - 1; // -1 or 1 �� �ϳ� ����
        x *= xOperation;

        int zOperation = Random.Range(0, 2) * 2 - 1;
        z *= zOperation;

        Vector3 pos = new Vector3(player.transform.position.x + x, 0, player.transform.position.z + z); //�÷��̾� ��ġ �������� ��ȯ ��ġ�� ���� 

        return pos;
    }
}
