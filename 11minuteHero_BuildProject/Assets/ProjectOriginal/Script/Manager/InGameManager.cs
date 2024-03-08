using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Test

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

    public WaitForSeconds FrameDelay;
    public WaitForFixedUpdate waitForFixedUpdate;

    [SerializeField] private GameObject[] playerCharacterArray; //�÷��̾� ĳ���� �迭. �÷��� �� ���� �� �ε����� ���޹޾� ������ ĳ���͸� Ȱ��ȭ
    [SerializeField] private int characterIndex;
    [SerializeField] private List<Monster> monsterList = new List<Monster>(); //���� �ʵ� ���� �����ϴ� ���� ����Ʈ
                                                                              //[SerializeField] private CustomPriorityQueue<Monster> monster
    private SkillManager skillManager;
    private int killCount;

    #region --Test--
    public Text timerText;
    public Text killCountText;
    private float timer;

    public Image fade;
    #endregion


    private EGameState gameState; //���� ���� ����

    private MonsterPool monsterPool; //���� Ǯ�� ������ �ʿ���ϴ� ��ü���� ������ �� �ֵ��� �̱��� Ŭ�������� �ν��Ͻ�ȭ��
    
    public static InGameManager Instance { get => instance; }
    public CPlayer Player { get => player; }
    public List<Monster> MonsterList { get => monsterList; set => monsterList = value; }
    public EGameState GameState { //���� ������Ʈ�� GameOver�� �Ǹ� gameOver ��������Ʈ ȣ��. GameOver�� �Ǵ� ���� �÷��̾� hp�� 0�� �Ǵ� ���ۿ� ����
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

    public delegate void LevelUpEvent(); // �÷��̾� ������ �� �߻��ϴ� �̺�Ʈ���� Ʈ���� �ۿ��� �� ��������Ʈ ����

    public delegate void StopAllCoroutinesWhenGameOver(); //���ӿ��� �� ��� �ڷ�ƾ ������ ���� ��������Ʈ ����

    private StopAllCoroutinesWhenGameOver dGameOver; //�ڷ�ƾ�� ����ϴ� �ΰ��� Ŭ�������� ��������Ʈ ü���� ���� �ش� ��������Ʈ�� StopAllCoroutines �Լ� �߰�.
    private LevelUpEvent dLevelUp; //������ �� �߻��ϴ� ��� �̺�Ʈ �Լ��� �ش� ��������Ʈ�� �߰�
    public StopAllCoroutinesWhenGameOver DGameOver { get => dGameOver; set => dGameOver = value; }
    public LevelUpEvent DLevelUp { get => dLevelUp; set => dLevelUp = value; }
    public MonsterPool MonsterPool { get => monsterPool; }

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

    private void Awake() //�̱������� �ν��Ͻ�ȭ. �ΰ��� �������� ����ȯ�� �Ͼ�� �ʱ� ������ �ΰ��� �� �ε� �ÿ��� this�� �ʱ�ȭ ���ָ� ��.
    {
        instance = this;
        //characterIndex = 0;//GameTest.Instance.index;
        Application.targetFrameRate = ConstDefine.TARGET_FRAME; //������ ����
        Screen.SetResolution(1280, 720, true);
        FrameDelay = new WaitForSeconds(ConstDefine.FRAME_DELAY);

        GameObject obj = Instantiate(playerCharacterArray[characterIndex]); //ĳ���� ���� �� ������ ������
        player = obj.GetComponent<CPlayer>();
        monsterPool = FindObjectOfType<MonsterPool>();
        skillManager = FindObjectOfType<SkillManager>();


        StartCoroutine(TweeningUtility.FadeOut(2f, fade));
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
        while(gameState != EGameState.GameOver)
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
        //if(gameState != EGameState.GameOver)
        //timer += Time.deltaTime;
    }
    public void IncreaseKillCount()
    {
        killCount++;
    }
}
