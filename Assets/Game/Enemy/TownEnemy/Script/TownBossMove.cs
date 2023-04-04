//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F1�ʃ{�XAI
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBossMove : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    // �G�������Ă������
    enum EnemyDirection
    {
        LEFT,
        RIGHT
    }

    private EnemyDirection Direction = EnemyDirection.LEFT;

    private float sizeX; // ���[�J���T�C�Y�ۑ�

    // �G�̃v���C���[�T�[�`�ϐ�
    private float SubX;    // ���߂�X���W�̍���ێ�����ϐ�
    private float SubY;    // ���߂�Y���W�̍���ێ�����ϐ�
    private float Distance; // ���߂�������ێ�����ϐ�

    // ���C�̏Փ˔��茋�ʗp�ϐ�
    RaycastHit2D hit;
    // ���C������l�F���@�E�ɂ��炷,�@���@���ɂ��炷
    private float halfScale; 
    // ���C�̈ʒu����
    float AdjustX;

    // �{�X�̃A�N�V������̌�����
    private float waitTimer;

    // �ːi�p�ϐ�
    private float RammingSpeed = 6.0f; // �ːi���̈ړ����x
    private float PreRammingTimer = 0f; // �ːi��������
    private int SwitchBack; // �؂�Ԃ���

    public enum AIState
    {
        Wait, // ��
        RammingInit, // �ːi����
        Ramming, // �ːi
        RammingWait, // �ːi��̌�
        ThrowShards, // �������΂�
        Death, // ���j
    }

    // �G�s�����
    public AIState EnemyAI = AIState.Ramming;

    // �O���擾
    private Transform thisTransform; // ���̃I�u�W�F�N�g�̍��W�����ϐ�
    private GameObject player; // �v���C���[�̃Q�[���I�u�W�F�N�g�T���p
    private Transform playerTransform;
    private GameObject child;
    private TownBossHealth BossHealth;

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // �I�u�W�F�N�g��Transform���擾
        thisTransform = GetComponent<Transform>();

        // �v���C���[�I�u�W�F�N�g�T��
        player = GameObject.Find("player");
        playerTransform = player.GetComponent<Transform>();

        // ���g�̎q�I�u�W�F�N�g�擾
        child = transform.Find("HitCollider").gameObject;
        // �{�X�̗̑̓X�N���v�g�擾
        BossHealth = child.GetComponent<TownBossHealth>();

        // ���C�ʒu����
        halfScale = thisTransform.localScale.x / 2.0f;
        AdjustX = halfScale;

        // �T�C�Y��ۑ�
        sizeX = thisTransform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        // ���͈͓��Ƀv���C���[���N�����Ă�����X�e�[�^�X�ω�
        // �v���C���[�Ƃ̋��������Ƃ߂�
        SubX = thisTransform.position.x - playerTransform.position.x; // x��
        SubY = thisTransform.position.y - playerTransform.position.y; // y��

        // �O�����̒藝
        Distance = SubX * SubX + SubY * SubY; // �v���C���[�Ƃ̋��������܂���

        // �{�X�̌����Ă���������Z�b�g
        SetDirection();

        // �i�s�����Ƀ��C���΂�
        CreateRay();

        switch (EnemyAI)
        {
            case AIState.Wait:
                Wait();
                break;

            case AIState.Ramming:
                Ramming();
                break;

            case AIState.ThrowShards:
                ThrowShards();
                break;
        }
    }

    private void Wait()
    {
        // �U����̌㌄
    }

    private void RammingInit()
    {
        // �ːi����
        // ���G

        // �؂�Ԃ��񐔂����߂�
        SwitchBack = BossHealth.MaxBossHealth - BossHealth.BossHealth;

    }

    private void Ramming()
    {
        // �ːi�U��
        // ���G

        float sign = 0.0f; // ����

        // �����Ă�������ɂ���ĕ������ς��
        if(Direction == EnemyDirection.RIGHT)
        {
            sign = 1f;
        }
        else
        {
            sign = -1f;
        }

        // �ړ����������߂�
        float MoveDistance = sign * RammingSpeed * Time.deltaTime; // ��b�Ԃ�RammmingSpeed���ړ�

        // �ǂɂԂ���܂œːi
        if (hit == false)
        {
            // �{�X�̌��݈ʒu�ɉ��Z
            thisTransform.Translate(MoveDistance, 0.0f, 0.0f);
        }
        // �ǂɂԂ���
        else
        {
            // �؂�Ԃ��񐔂�1�ȏ゠��ΐ؂�Ԃ��ċt�����ɓːi
            if (SwitchBack > 0) 
            {
                if(Direction == EnemyDirection.LEFT)
                {
                    Direction = EnemyDirection.RIGHT;
                }
                else
                {
                    Direction = EnemyDirection.LEFT;
                }
            }
            else
            {
                EnemyAI = AIState.RammingWait;
            }
        }
    }

    private void RammingWait()
    {

    }

    private void ThrowShards()
    {
        // �������΂�
    }

    private void CreateRay()
    {
        // �i�s�����Ƀ��C���΂��ĕǂɂԂ�������i�s������ς���
        Vector2 origin = new Vector2(
            thisTransform.position.x + AdjustX,
            thisTransform.position.y
            );

        // ���C���΂�����
        Vector2 RayDirection = Vector2.zero;

        // �{�X�̌����ɂ���ă��C���Ƃ΂��������ω�
        switch (Direction)
        {
            case EnemyDirection.LEFT:
                RayDirection = new Vector2(-1, 0); // ������
                break;

            case EnemyDirection.RIGHT:
                RayDirection = new Vector2(1, 0); // �E����
                break;
        }

        // ����
        float length = 0.1f;
        // ����
        Vector2 distance = RayDirection * length;
        // ����̃��C���[�̃��m�Ƃ����Փ˔�����Ƃ�
        // ���C���[�}�X�N�͓�i���𗘗p
        // ��:layerMask = 1 << 2 �͓�i���\����100�B�ォ��O�ڂ̃��C���[�Ƃ����Ƃ����Ӗ�
        LayerMask layerMask = 1 << 10; // ���V�t�g���Z�A1��<<�̉E�̐��������ɃV�t�g

        // ���C��΂��ăX�e�[�W�ƂԂ������琶����߂�
        hit = Physics2D.Raycast(origin, RayDirection, length, layerMask); // ��O���� ���C���� �A��l���� ���C���[ -1�͑S�Ẵ��C���[

        // �`��
        Debug.DrawRay(origin, distance, Color.red);
        //-----------------------------------------------------------------------------------------------
    }

    private void SetDirection()
    {
        if (Direction == EnemyDirection.LEFT) {
            // ������
            thisTransform.localScale = new Vector3(-sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            // ���C����
            AdjustX = -halfScale;
        }
        else
        {
            // �E����
            thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            // ���C����
            AdjustX = halfScale;
        }
    }
}
