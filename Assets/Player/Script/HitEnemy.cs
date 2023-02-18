//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�G�Ƃ̓����蔻��
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemy : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    private string enemyTag = "Enemy"; // ������Enemy�����ϐ�

    public float NoDamageTime = 100.0f; //���G����
    public float HitTime = 0.0f; // �G�ƐڐG���Ă��鎞��

    // �O���擾
    public GameOver gameOver; // �Q�[���I�[�o�[��ʑJ�ڗp�X�N���v�g�擾�p�ϐ�
    public DownAlpha alpha; // UI�̃X�N���v�g

    void Update()
    {
        HitTime += Time.deltaTime;
    }

    //---------------------------------------------------------
    // Enemy�^�O�����I�u�W�F�N�g�Ƃ̏Փ˂�����������
    // gameOver�X�N���v�g�����ϐ� HP ��-1����
    void OnCollisionEnter2D(Collision2D collision)
    {
        //---------------------------------------------------------
        // ������
        HitTime = 0.0f;
        //---------------------------------------------------------
        // Enemy�^�O���ǂ���
        if (collision.gameObject.tag == enemyTag)
        {
            //---------------------------------------------------------
            // HP -1
            gameOver.HP--;

            //---------------------------------------------------------
            // �A���t�@�l�ύX
            alpha.SetAlpha(gameOver.HP,gameOver.maxHp);

            Debug.Log("Hit");
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //---------------------------------------------------------
        // Enemy�^�O���ǂ���
        if (collision.gameObject.tag == enemyTag)
        {
            // �ڐG���Ԃ����G���Ԃ��傫���Ȃ�HP���炷
            if (HitTime > NoDamageTime)
            {
                //---------------------------------------------------------
                // HP -1
                gameOver.HP--;

                //---------------------------------------------------------
                // �A���t�@�l�ύX
                alpha.SetAlpha(gameOver.HP, gameOver.maxHp);

                //---------------------------------------------------------
                // �ڐG���ԃ��Z�b�g
                HitTime = 0.0f;

                Debug.Log("Hit");

            }
        }
    }
}
