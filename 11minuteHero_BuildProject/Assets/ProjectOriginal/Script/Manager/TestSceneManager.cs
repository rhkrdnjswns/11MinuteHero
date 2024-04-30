using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using UnityEngine.SceneManagement;

namespace ForTest
{
    public class TestSceneManager : MonoBehaviour
    {
        public GameObject dummyPrefab;
        private Queue<Monster> dummyPool = new Queue<Monster>();

        private LayerMask layer;
        public bool bSummonDummy { get; set; }
        public bool bDeleteDummy { get; set; }
        private void CreateDummy()
        {
            GameObject obj = Instantiate(new GameObject());
            obj.transform.position = Vector3.zero;
            for (int i = 0; i < 100; i++)
            {
                Monster m = Instantiate(dummyPrefab).GetComponent<Monster>();
                m.gameObject.SetActive(false);
                m.transform.SetParent(obj.transform);
                m.InitDamageUIContainer();
                m.name = m.name + i;

                dummyPool.Enqueue(m);
            }
        }
        private void Awake()
        {
            EditorApplication.playModeStateChanged += ShowLog;
            CreateDummy();
        }
        private void Update()
        {
            if(bSummonDummy)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePos = Input.mousePosition;

                    Ray ray = Camera.main.ScreenPointToRay(mousePos);

                    RaycastHit hit;

                    layer = LayerMask.GetMask("Floor");

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
                    {
                        Monster m = dummyPool.Dequeue();
                        m.transform.position = hit.point;
                        m.gameObject.SetActive(true);
                    }
                    bSummonDummy = false;
                }
            }
            if(bDeleteDummy)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePos = Input.mousePosition;

                    Ray ray = Camera.main.ScreenPointToRay(mousePos);

                    RaycastHit hit;

                    layer = LayerMask.GetMask("Monster");

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
                    {
                        if(!hit.transform.GetComponent<NormalMonster>() || !hit.transform.GetComponent<Boss>())
                        {
                            Monster m = hit.transform.GetComponent<Monster>();
                            m.gameObject.SetActive(false);
                            m.transform.position = Vector3.zero;
                            m.CurrentHp = m.MaxHp;
                            dummyPool.Enqueue(m);
                        }
                    }
                    bDeleteDummy = false;
                }
            }
        }
        public void BtnEvt_InitSkill(int index)
        {
            FindObjectOfType<SkillManager>().Test_SkillLevelUp(index + 1);
        }
        public void BtnEvt_Weapon()
        {
            FindObjectOfType<SkillManager>().Test_SkillLevelUp(0);
        }
        public void BtnEvt_ActiveObject(GameObject obj)
        {
            obj.SetActive(!obj.activeSelf);
        }
        public void BtnEvt_SummonDummy()
        {
            bSummonDummy = true;
        }
        public void BtnEvt_DeleteDummy()
        {
            bDeleteDummy = true;
        }
        private void ShowLog(PlayModeStateChange state)
        {
            if(state == PlayModeStateChange.ExitingPlayMode)
            {
                Debug.Log($"���� �ð� : {(int)InGameManager.Instance.Timer / 60}��{(int)InGameManager.Instance.Timer % 60}��");
            }
        }

        public void BtnEvt_SummonMonster(int index)
        {
            Vector3 RotDir = Quaternion.Euler(0, Random.Range(0, 361), 0) * Vector3.forward;

            InGameManager.Instance.MonsterPool.GetMonster(InGameManager.Instance.Player.transform.position + RotDir.normalized * 25, index);
        }
    }
    public class DPSLogWindow : EditorWindow
    {
        private List<ActiveSkill> activatedSkillList = new List<ActiveSkill>();

        StringBuilder sb = new StringBuilder();

        [MenuItem("Window/DPS Log")]//DPS Log = �޴� �̸�, �ش� �޴� ���� �� �Ʒ� �Լ� ȣ��
        public static void ShowWindow()
        {
            //DPSLogWindow Ÿ���� �����츦 �����´�. DPS Log = ������ Ÿ��Ʋ
            GetWindow(typeof(DPSLogWindow), false, "DPS Log");
        }
        private void OnGUI()
        {
            if (SceneManager.GetActiveScene().name != "Scene05_InGameTest")
            {
                GUILayout.Label("DPS Log�� 'Scene05_InGameTest' �������� Ȯ�� �����մϴ�.");
                return;
            }
            if (activatedSkillList.Count == 0)
            {
                GUILayout.Label("Ȱ��ȭ�� ��ų�� �����ϴ�.");
                return;
            }
            for(int i = 0; i < activatedSkillList.Count; i++)
            {
                AddString("��ų �̸� : ", activatedSkillList[i].Name,
                    "\nȰ��ȭ �ð� : ", activatedSkillList[i].ActivateTime,
                    "\n��ų ���� : ", activatedSkillList[i].Level.ToString(),
                    "\n��ų ���ط� :", activatedSkillList[i].Damage.ToString(),
                    "\n����(���) ȸ�� : ", activatedSkillList[i].AttackCount.ToString(),
                    "\n���� ���ط� : ", activatedSkillList[i].TotalDamage.ToString(), "\n");
                GUILayout.Label(sb.ToString());
                sb.Clear();
            }

            if(GUILayout.Button("�α� �����ϱ�"))
            {

            }
        }
        public void AddSkill(ActiveSkill skill)
        {
            activatedSkillList.Add(skill);
        }
        private void AddString(params string[] values)
        {
            foreach (var item in values)
            {
                sb.Append(item);
            }
        }
    }

}

