using UnityEngine;
/// <summary>
/// 문자열로 기입하는 경로(애니메이션 클립, 오브젝트 이름 등)와 프레임 등 상수값의 모임
/// </summary>
public class ConstDefine //모든 상수 정의.
{
    //애니메이터 파라메터
    public const string BOOL_ISMOVE = "IsMove";
    public const string TRIGGER_MELEE_ATTACK = "MeleeAttack";
    public const string TRIGGER_DIE = "Die";
    public const string TRIGGER_DIEEND = "DieEnd";
    public const string TRIGGER_ATTACK = "Attack";
    public const string TRIGGER_Dodge = "Dodge";
    public const string FLOAT_ATTACK_SPEED = "AttackSpeed";

    //게임오브젝트 이름
    public const string NAME_DAMAGEUI_CANVAS = "Canvas_DamageUI";
    public const string NAME_PROJECTILE_SPAWN_POINT = "ProjectileSpawnPoint";
    public const string NAME_FIELD = "Field";
    public const string NAME_ITEMGAINER = "ItemGainer";
    public const string NAME_OVERLAPPINGAVOIDER = "OverlappingAvoider";
    public const string NAME_PLAYER_UI_HP = "Image_HpFill";
    public const string NAME_PLAYER_UI_EXP = "Image_ExpBarFill";
    public const string NAME_UICAMERA = "UICamera";

    public static LayerMask LAYER_MONSTER = LayerMask.GetMask("Monster");
    public static LayerMask LAYER_PLAYER = LayerMask.GetMask("Player");

    //태그
    public const string TAG_MONSTER = "Monster";
    public const string TAG_PLAYER = "Player";
    public const string TAG_PARTICLE = "Particle";
    public const string TAG_FLOOR = "Floor";

    //타겟 프레임
    public const int TARGET_FRAME = 60;
    //중력가속도
    public const float GRAVITY_SCALE = 9.81f;

    //스킬 관련
    public const int SKILL_MAX_LEVEL = 4;
    public const int SKILL_MAX_HAVE_COUNT = 6;
    public const int SKILL_SELECT_COUNT = 3;

    //무기를 포함한 기믹 갯수
    public const int ALL_PASSIVE_COUNT = 9;
    public const int ALL_ACTIVE_COUNT = 8;//12;
    public const int ALL_GIMMICK_COUNT = 16;//21;

    public const int PLAYER_PASSIVE_COUNT_MAX = 6;
    public const int PLAYER_ACTIVE_COUNT_MAX = 6;

    //몬스터 재배치 관련 상수
    public const float REPOSITION_DISTANCE = 30f; //몬스터를 재배치 할 플레이어와 몬스터간의 거리
    public const float REPOSITION_VALUE = 10f; //몬스터 재배치 값. (플레이어 방향으로 +로)
}
