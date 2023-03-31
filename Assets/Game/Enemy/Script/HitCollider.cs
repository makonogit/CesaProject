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

    public float NoDamageTime = 1.0f; //���G����
    [SerializeField] float HitTime = 0.0f; // �G�ƐڐG���Ă��鎞��

    [Header("�G�ɂ��_���[�W")]
    public int Damage = 1; // �ڐG���ɂ��炤�_���[�W

    // �O���擾
    private GameObject player;
    private GameOver gameOver; // �Q�[���I�[�o�[��ʑJ�ڗp�X�N���v�g�擾�p�ϐ�

    private void Start()
    {
        player = GameObject.Find("player");
        gameOver = player.GetComponent<GameOver>();
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
        // Player�^�O���ǂ���
        if (collision.gameObject.tag == playerTag)
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
}
