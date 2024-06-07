using UnityEngine;
/// <summary>
/// ���ڿ��� �����ϴ� ���(�ִϸ��̼� Ŭ��, ������Ʈ �̸� ��)�� ������ �� ������� ����
/// </summary>
public class ConstDefine //��� ��� ����.
{
    //�ִϸ����� �Ķ����
    public const string BOOL_ISMOVE = "IsMove";
    public const string TRIGGER_MELEE_ATTACK = "MeleeAttack";
    public const string TRIGGER_DIE = "Die";
    public const string TRIGGER_DIEEND = "DieEnd";
    public const string TRIGGER_ATTACK = "Attack";
    public const string TRIGGER_Dodge = "Dodge";
    public const string FLOAT_ATTACK_SPEED = "AttackSpeed";

    //���ӿ�����Ʈ �̸�
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

    //�±�
    public const string TAG_MONSTER = "Monster";
    public const string TAG_PLAYER = "Player";
    public const string TAG_PARTICLE = "Particle";
    public const string TAG_FLOOR = "Floor";

    //Ÿ�� ������
    public const int TARGET_FRAME = 60;
    //�߷°��ӵ�
    public const float GRAVITY_SCALE = 9.81f;

    //��ų ����
    public const int SKILL_MAX_LEVEL = 4;
    public const int SKILL_MAX_HAVE_COUNT = 6;
    public const int SKILL_SELECT_COUNT = 3;

    //���⸦ ������ ��� ����
    public const int ALL_PASSIVE_COUNT = 9;
    public const int ALL_ACTIVE_COUNT = 8;//12;
    public const int ALL_GIMMICK_COUNT = 16;//21;

    public const int PLAYER_PASSIVE_COUNT_MAX = 6;
    public const int PLAYER_ACTIVE_COUNT_MAX = 6;

    //���� ���ġ ���� ���
    public const float REPOSITION_DISTANCE = 30f; //���͸� ���ġ �� �÷��̾�� ���Ͱ��� �Ÿ�
    public const float REPOSITION_VALUE = 10f; //���� ���ġ ��. (�÷��̾� �������� +��)
}
