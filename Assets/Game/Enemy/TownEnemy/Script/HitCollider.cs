//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�G�̓����蔻��֌W
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    private string playerTag = "Player"; // ������Enemy�����ϐ�

    // �O���擾
    private GameObject player;
    //private GameOver gameOver; // �Q�[���I�[�o�[��ʑJ�ڗp�X�N���v�g�擾�p�ϐ�
    //private KnockBack knocback; // �m�b�N�o�b�N�X�N���v�g�擾�p�ϐ�
    //private RenderOnOff _renderer; // �_�ŃX�N���v�g�擾�p�ϐ�
    private HitEnemy _hitEnemy; // ���G���Ԋ֌W�X�N���v�g

    private Transform thisTransform; // ���g�̃g�����X�t�H�[��

    //�ǉ���
    //private PlayEnemySound enemyse; //�G��SE

    private void Start()
    {
        player = GameObject.Find("player");

        thisTransform = GetComponent<Transform>();

        //// �Q�[���I�[�o�[�X�N���v�g�擾
        //// �v���C���[�̔�_���[�W�����p
        //gameOver = player.GetComponent<GameOver>();

        //// �m�b�N�o�b�N�X�N���v�g�擾
        //knocback = player.GetComponent<KnockBack>();

        //// �_�ŃX�N���v�g�擾
        //_renderer = player.GetComponent<RenderOnOff>();

        //// �G��SE�Đ��p�X�N���v�g�擾�@�ǉ�:��
        //enemyse = GameObject.Find("EnemySE").GetComponent<PlayEnemySound>();

        // �v���C���[��HitEnemy�擾
        _hitEnemy = player.GetComponent<HitEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Player�^�O���ǂ���
        if (collision.gameObject.tag == playerTag)
        {
            // �G�ƃv���C���[���ڐG�����Ƃ��̏����֐��Ăяo��
            _hitEnemy.HitPlayer(thisTransform);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Player�^�O���ǂ���
        if (collision.gameObject.tag == playerTag)
        {
            // �G�ƃv���C���[���ڐG�����Ƃ��̏����֐��Ăяo��
            _hitEnemy.HitPlayer(thisTransform);

            //// �ڐG���Ԃ����G���Ԃ��傫���Ȃ�HP���炷
            //if (_hitEnemy.HitTime > _hitEnemy.NoDamageTime)
            //{
            //    //---------------------------------------------------------
            //    //�@SE���Đ�
            //    enemyse.PlayEnemySE(PlayEnemySound.EnemySoundList.Attack);

            //    //---------------------------------------------------------
            //    // HP���炷���߂̏���
            //    gameOver.StartHPUIAnimation();

            //    // �m�b�N�o�b�N
            //    knocback.KnockBack_Func(transform.parent.transform);

            //    // �_��
            //    _renderer.SetFlash(true);

            //    //---------------------------------------------------------
            //    // �ڐG���ԃ��Z�b�g
            //    _hitEnemy.HitTime = 0.0f;
            //}
        }
    }
}
