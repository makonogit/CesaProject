//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�ݒ肵���^�O�Ɛڒn���Ă��邩RayCast�Ŕ���
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadCheck : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    private string groundTag = "Ground"; // Ground�^�O���𕶎���^�Ŏ����Ă���ϐ�
    private string iceTag = "Ice";

    public bool isOverhead = false; // �ŏI�I�ɓV��ƏՓ˂��Ă��邩���Ă��Ȃ����̏�������
    public float AdjustY = -0.15f; // �摜�̋󔒕����𖳎����邽�߂̒����p�ϐ�
    public float AdjustX = 0.41f; // �摜�̋󔒕����𖳎����邽�߂̒����p�ϐ�
    public float AdjustCenter = 0.15f; // �������낦�p�ϐ�

    PlayerInputManager.DIRECTION oldDire; // �O�t���[���̌��������Ă������߂̕ϐ�

    // �O���擾
    private GameObject PlayerInputMana; // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾�����
    private Transform thistransform; // ���C�ɂ�铖���蔻����Ƃ�I�u�W�F�N�g�̌��_���W

    private void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // ���W���擾
        thistransform = GetComponent<Transform>();

        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManager��T��
        PlayerInputMana = GameObject.Find("PlayerInputManager");

        //----------------------------------------------------------------------------------------------------------
        // �Q�[���I�u�W�F�N�gPlayerInputManager������PlayerInputManager�X�N���v�g���擾
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        oldDire = ScriptPIManager.Direction;
    }

    private void Update()
    {
        if (oldDire != ScriptPIManager.Direction)
        {
            AdjustX = -AdjustX;
            AdjustCenter = -AdjustCenter;
        }

        // �O�t���[���̌����Ƃ��ĕۑ�
        oldDire = ScriptPIManager.Direction;
    }

    //----------------------------------------------------------------------------------------------------------
    //�ڒn�����Ԃ����\�b�h
    //��������̍X�V���ɌĂԕK�v������
    public bool IsOverHead()
    {
        //----------------------------------------------------------------------------------------------------------
        // ���C�L���X�g�ɕK�v�Ȉ����̏���

        // x:left y:top
        Vector2 origin_left = new Vector2(thistransform.position.x - thistransform.localScale.x / 2.0f + AdjustX + AdjustCenter,
            thistransform.position.y + thistransform.localScale.y / 2.0f + AdjustY);
        // x:right y:top
        Vector2 origin_right = new Vector2(thistransform.position.x + thistransform.localScale.x / 2.0f - AdjustX + AdjustCenter,
            thistransform.position.y + thistransform.localScale.y / 2.0f + AdjustY);
        // ����
        Vector2 direction = new Vector2(0, 1);
        // ����
        float length = 0.1f;
        // ����
        Vector2 distance = direction * length;

        //----------------------------------------------------------------------------------------------------------
        // ���C��΂��ĉ����ƂԂ������琶����߂�
        RaycastHit2D hit_l = Physics2D.Raycast(origin_left, direction, length, 5); // ��O���� ���C���� �A��l���� ���C���[ -1�͑S�Ẵ��C���[
        RaycastHit2D hit_r = Physics2D.Raycast(origin_right, direction, length, 5); // ��O���� ���C���� �A��l���� ���C���[ -1�͑S�Ẵ��C���[

        //----------------------------------------------------------------------------------------------------------
        // ���C��`��
        Debug.DrawRay(origin_left, distance, Color.blue);
        Debug.DrawRay(origin_right, distance, Color.blue);

        // if�����₷�����邽�ߕϐ���`
        bool leftJudge = (hit_l && ((hit_l.collider.gameObject.tag == groundTag) || (hit_l.collider.gameObject.tag == iceTag)));
        bool rightJudge = (hit_r && ((hit_r.collider.gameObject.tag == groundTag) || (hit_r.collider.gameObject.tag == iceTag)));

        //----------------------------------------------------------------------------------------------------------
        // �������Ă��邩�A�^�O��Ground�Ȃ�isGround��true�ɂ���
        if (leftJudge || rightJudge)
        {
            isOverhead = true;
        }
        else
        {
            isOverhead = false;
        }

        //----------------------------------------------------------------------------------------------------------
        // �����Ԃ�
        return isOverhead;
    }
}
