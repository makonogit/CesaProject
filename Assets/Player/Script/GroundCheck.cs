//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�ݒ肵���^�O�Ɛڒn���Ă��邩RayCast�Ŕ���
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    private string groundTag = "Ground"; // Ground�^�O���𕶎���^�Ŏ����Ă���ϐ�
    public bool isGround = false; // �ŏI�I�ɐڒn���Ă��邩���Ă��Ȃ����̏�������
    private float AdjustY = 0.03f; // �摜�̋󔒕����𖳎����邽�߂̒����p�ϐ�
    private float AdjustX = 0.41f; // �摜�̋󔒕����𖳎����邽�߂̒����p�ϐ�
    public int touch; // �n�ʂƐG��Ă��郌�C�̖{��

    // �O���擾
    private Transform thistransform; // ���C�ɂ�铖���蔻����Ƃ�I�u�W�F�N�g�̌��_���W

    private void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // ���W���擾
        thistransform = GetComponent<Transform>();
    }

    //----------------------------------------------------------------------------------------------------------
    //�ڒn�����Ԃ����\�b�h
    //��������̍X�V���ɌĂԕK�v������
    public bool IsGround()
    {
        //----------------------------------------------------------------------------------------------------------
        // ���C�L���X�g�ɕK�v�Ȉ����̏���

        // x:left y:bottom
        Vector2 origin_left = new Vector2(thistransform.position.x - thistransform.localScale.x / 2.0f + AdjustX, 
            thistransform.position.y - thistransform.localScale.y / 2.0f + AdjustY);

        // x:���_ y;bottom
        Vector2 origin_middle = new Vector2(thistransform.position.x, 
            thistransform.position.y - thistransform.localScale.y / 2.0f + AdjustY);

        // x:right y:bottom
        Vector2 origin_right = new Vector2(thistransform.position.x + thistransform.localScale.x / 2.0f - AdjustX, 
            thistransform.position.y - thistransform.localScale.y / 2.0f + AdjustY);
        // ����
        Vector2 direction = new Vector2(0, -1);
        // ����
        float length = 0.1f;
        // ����
        Vector2 distance = direction * length;

        //----------------------------------------------------------------------------------------------------------
        // ���C��΂��ĉ����ƂԂ������琶����߂�
        // ����
        RaycastHit2D hit_l = Physics2D.Raycast(origin_left, direction, length,5); // ��O���� ���C���� �A��l���� ���C���[ -1�͑S�Ẵ��C���[

        // ����
        RaycastHit2D hit_m = Physics2D.Raycast(origin_middle, direction, length,5); // ��O���� ���C���� �A��l���� ���C���[ -1�͑S�Ẵ��C���[

        // �E��
        RaycastHit2D hit_r = Physics2D.Raycast(origin_right, direction, length,5); // ��O���� ���C���� �A��l���� ���C���[ -1�͑S�Ẵ��C���[

        //------�ǉ����� �S���F�����S Ray��LayerMask��ݒ�

        //----------------------------------------------------------------------------------------------------------
        // ���C��`��
        Debug.DrawRay(origin_left, distance, Color.red);
        Debug.DrawRay(origin_middle, distance, Color.red);
        Debug.DrawRay(origin_right, distance, Color.red);

        touch = 0;

        // �������Ă��邩�A�^�O��Ground�Ȃ�J�E���g�𑝂₷
        if (hit_l && hit_l.collider.gameObject.tag == groundTag)
        {
            touch++;
        }
        if (hit_m && hit_m.collider.gameObject.tag == groundTag)
        {
            touch++;
        }
        if (hit_r && hit_r.collider.gameObject.tag == groundTag)
        {
            touch++;
        }

       
        //----------------------------------------------------------------------------------------------------------
        // ��{�ȏ�n�ʂɐG��Ă�����
        if (touch >= 2)
        {
            isGround = true;
            // �������Ă���^�O����\��
            //Debug.Log(hit.collider.gameObject.tag);
        }
        else
        {
            isGround = false;
        }

        //----------------------------------------------------------------------------------------------------------
        // �����Ԃ�
        return isGround;
    }

    public int GetRayNum()
    {
        return touch;
    }
}
