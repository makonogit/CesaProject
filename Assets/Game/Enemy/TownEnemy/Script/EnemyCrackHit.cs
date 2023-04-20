//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�������̂Ђт�G�ɓ��Ă����ɓG������
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrackHit : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -
    private string CrackTag = "Crack";

    // �O���擾
    private CrackCreater order = null;
    private GameObject ParentEnemy; // �e�I�u�W�F�N�g�̓G
    private EnemyMove enemyMove; // EnemyMove�X�N���v�g�擾�p�ϐ�

    private void Start()
    {
        // �e�I�u�W�F�N�g�擾
        ParentEnemy = transform.root.gameObject;

        // �G�̊�{AI�����X�N���v�g�擾
        enemyMove = ParentEnemy.GetComponent<EnemyMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        //Debug.Log(collision.gameObject.tag);

        // ���߂̈��̂ݓ���
        if (enemyMove.EnemyAI != EnemyMove.AIState.DEATH)
        {
            // �����������̂��ЂтȂ�
            if (collision.gameObject.tag == CrackTag)
            {
                // ���������Ђт�CrackOrder���擾
                order = collision.gameObject.GetComponent<CrackCreater>();

                //�������Ȃ�
                if (order.State == CrackCreater.CrackCreaterState.CREATING)
                {
                    // ���S��Ԃɂ���
                    enemyMove.EnemyAI = EnemyMove.AIState.DEATH;
                }
            }
        }
    }
}
