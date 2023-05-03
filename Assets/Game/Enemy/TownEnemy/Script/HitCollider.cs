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
    private string BossEnemyTag = "BossEnemy";

    public float NoDamageTime = 1.0f; //���G����
    [SerializeField] float HitTime = 0.0f; // �G�ƐڐG���Ă��鎞��

    [Header("�G�ɂ��_���[�W")]
    public int Damage = 1; // �ڐG���ɂ��炤�_���[�W

    // �O���擾
    private GameObject player;
    private GameOver gameOver; // �Q�[���I�[�o�[��ʑJ�ڗp�X�N���v�g�擾�p�ϐ�
    private KnockBack knocback; // �m�b�N�o�b�N�X�N���v�g�擾�p�ϐ�
    private RenderOnOff renderer; // �_�ŃX�N���v�g�擾�p�ϐ�

    private void Start()
    {
        player = GameObject.Find("player");

        // �Q�[���I�[�o�[�X�N���v�g�擾
        // �v���C���[�̔�_���[�W�����p
        gameOver = player.GetComponent<GameOver>();

        // �m�b�N�o�b�N�X�N���v�g�擾
        knocback = player.GetComponent<KnockBack>();

        // �_�ŃX�N���v�g�擾
        renderer = player.GetComponent<RenderOnOff>();
    }

    void Update()
    {
        HitTime += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Player�^�O���ǂ���
        if (collision.gameObject.tag == playerTag)
        {
            // �ڐG���Ԃ����G���Ԃ��傫���Ȃ�HP���炷
            if (HitTime > NoDamageTime)
            {
                //---------------------------------------------------------
                // HP���炷���߂̏���
                gameOver.StartHPUIAnimation();

                // �m�b�N�o�b�N
                knocback.KnockBack_Func(transform.parent.transform);

                // �_��
                renderer.SetFlash(true);

                //---------------------------------------------------------
                // �ڐG���ԃ��Z�b�g
                HitTime = 0.0f;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Player�^�O���ǂ���
        if (collision.gameObject.tag == playerTag)
        {
            // �ڐG���Ԃ����G���Ԃ��傫���Ȃ�HP���炷
            if (HitTime > NoDamageTime)
            {
                //---------------------------------------------------------
                // HP���炷���߂̏���
                gameOver.StartHPUIAnimation();

                // �m�b�N�o�b�N
                knocback.KnockBack_Func(transform.parent.transform);

                // �_��
                renderer.SetFlash(true);

                //---------------------------------------------------------
                // �ڐG���ԃ��Z�b�g
                HitTime = 0.0f;
            }
        }
    }
}
