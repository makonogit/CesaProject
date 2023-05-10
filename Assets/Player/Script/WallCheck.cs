//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�v���C���[���ǂɓ������Ă��邩�𒲂ׂ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    // �ϐ��錾
    private bool isLeft = false;
    private bool isRight = false;

    public float AdjustY = 0.03f; // �摜�̋󔒕����𖳎����邽�߂̒����p�ϐ�
    public float AdjustX = 0.41f; // �摜�̋󔒕����𖳎����邽�߂̒����p�ϐ�
    public float AdjustCenterX = 0.15f; // �������낦�p�ϐ�
    public float AdjustCenterY = 0.15f; // �������낦�p�ϐ�

    [SerializeField, Tooltip("���C�̒����𒲐����܂��B")] private float _length = 0.01f;

    private LayerMask layerMask = 1 << 10 | 1 << 14 | 1 << 17 | 1 << 18 | 1 << 21 | 1 << 22;

    private Transform thisTransform;

    PlayerInputManager.DIRECTION oldDire; // �O�t���[���̌��������Ă������߂̕ϐ�
    public PlayerInputManager _playerInputManager;

    private void Start()
    {
        thisTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (oldDire != _playerInputManager.Direction)
        {
            AdjustCenterX = -AdjustCenterX;
        }

        // �O�t���[���̌����Ƃ��ĕۑ�
        oldDire = _playerInputManager.Direction;
    }

    public bool LeftCheck()
    {
        //----------------------------------------------------------------------------------------------------------
        // ���C�L���X�g�ɕK�v�Ȉ����̏���

        // x:left y:bottom
        Vector2 origin_leftUp = new Vector2(thisTransform.position.x - AdjustX + AdjustCenterX,
            thisTransform.position.y + AdjustY + AdjustCenterY);

        // x: y;bottom
        Vector2 origin_leftDown = new Vector2(thisTransform.position.x - AdjustX + AdjustCenterX,
            thisTransform.position.y - AdjustY + AdjustCenterY);

        // ����
        Vector2 direction = Vector2.left;
        // ����
        float length = _length;// �ύX�ҁF����
        // ����
        Vector2 distance = direction * length;

        //----------------------------------------------------------------------------------------------------------
        // ���C��΂��ĉ����ƂԂ������琶����߂�
        // ��
        RaycastHit2D hit_up = Physics2D.Raycast(origin_leftUp, direction, length, layerMask); // ��O���� ���C���� �A��l���� ���C���[ -1�͑S�Ẵ��C���[

        // ��
        RaycastHit2D hit_down = Physics2D.Raycast(origin_leftDown, direction, length, layerMask); // ��O���� ���C���� �A��l���� ���C���[ -1�͑S�Ẵ��C���[

        //----------------------------------------------------------------------------------------------------------
        // ���C��`��
        Debug.DrawRay(origin_leftUp, distance, Color.white);
        Debug.DrawRay(origin_leftDown, distance, Color.white);

        // �Е��ł��G��Ă�����
        if(hit_down || hit_up)
        {
            isLeft = true;
        }
        else
        {
            isLeft = false;
        }

        return isLeft;
    }

    public bool RightCheck()
    {
        //----------------------------------------------------------------------------------------------------------
        // ���C�L���X�g�ɕK�v�Ȉ����̏���

        // x:left y:bottom
        Vector2 origin_RightUp = new Vector2(thisTransform.position.x + AdjustX + AdjustCenterX,
            thisTransform.position.y + AdjustY + AdjustCenterY);

        // x: y;bottom
        Vector2 origin_RightDown = new Vector2(thisTransform.position.x + AdjustX + AdjustCenterX,
            thisTransform.position.y - AdjustY + AdjustCenterY);

        // ����
        Vector2 direction = Vector2.right;
        // ����
        float length = _length;// �ύX�ҁF����
        // ����
        Vector2 distance = direction * length;

        //----------------------------------------------------------------------------------------------------------
        // ���C��΂��ĉ����ƂԂ������琶����߂�
        // ��
        RaycastHit2D hit_up = Physics2D.Raycast(origin_RightUp, direction, length, layerMask); // ��O���� ���C���� �A��l���� ���C���[ -1�͑S�Ẵ��C���[

        // ��
        RaycastHit2D hit_down = Physics2D.Raycast(origin_RightDown, direction, length, layerMask); // ��O���� ���C���� �A��l���� ���C���[ -1�͑S�Ẵ��C���[

        //----------------------------------------------------------------------------------------------------------
        // ���C��`��
        Debug.DrawRay(origin_RightUp, distance, Color.green);
        Debug.DrawRay(origin_RightDown, distance, Color.green);

        // �Е��ł��G��Ă�����
        if (hit_down || hit_up)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }

        return isRight;
    }
}
