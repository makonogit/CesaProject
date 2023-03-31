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
    [SerializeField] float HitTime = 0.0f; // �G�ƐڐG���Ă��鎞��

    [Header("�G�ɂ��_���[�W")]
    public int Damage = 10; // �ڐG���ɂ��炤�_���[�W

    // �O���擾
    public GameOver gameOver; // �Q�[���I�[�o�[��ʑJ�ڗp�X�N���v�g�擾�p�ϐ�

    void Update()
    {
        HitTime += Time.deltaTime;
    }

    //---------------------------------------------------------
    // Enemy�^�O�����I�u�W�F�N�g�Ƃ̏Փ˂�����������
    // gameOver�X�N���v�g�����ϐ� HP ��-1����
    void OnTriggerEnter2D(Collider2D collision)
    {
        ////---------------------------------------------------------
        //// Enemy�^�O���ǂ���
        //if (collision.gameObject.tag == enemyTag)
        //{
        //    //---------------------------------------------------------
        //    // HP -Damage
        //    gameOver.DecreaseHP(Damage);

        //    //---------------------------------------------------------
        //    // ������
        //    HitTime = 0.0f;

        //    //---------------------------------------------------------
        //    // �A���t�@�l�ύX
        //    //alpha.SetAlpha(gameOver.HP,gameOver.maxHp);
        //}

        //---------------------------------------------------------
        // Enemy�^�O���ǂ���
        if (collision.gameObject.tag == enemyTag)
        {
            // �ڐG���Ԃ����G���Ԃ��傫���Ȃ�HP���炷
            if (HitTime > NoDamageTime)
            {
                //---------------------------------------------------------
                // HP -Damage
                gameOver.DecreaseHP(Damage);

                //---------------------------------------------------------
                // �ڐG���ԃ��Z�b�g
                HitTime = 0.0f;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Enemy�^�O���ǂ���
        if (collision.gameObject.tag == enemyTag)
        {
            // �ڐG���Ԃ����G���Ԃ��傫���Ȃ�HP���炷
            if (HitTime > NoDamageTime)
            {
                //---------------------------------------------------------
                // HP -Damage
                gameOver.DecreaseHP(Damage);

                //---------------------------------------------------------
                // �A���t�@�l�ύX
                //alpha.SetAlpha(gameOver.HP, gameOver.maxHp);

                //---------------------------------------------------------
                // �ڐG���ԃ��Z�b�g
                HitTime = 0.0f;
            }
        }
    }
}
