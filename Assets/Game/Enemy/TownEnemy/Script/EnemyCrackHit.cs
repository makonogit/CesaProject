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
    private GameObject ParentEnemy; // �e�I�u�W�F�N�g�̓G;
    private EnemyMove enemyMove; // EnemyMove�X�N���v�g�擾�p�ϐ�

    private void Start()
    {
        ParentEnemy = transform.parent.gameObject;  //�@�e�I�u�W�F�N�g���擾
        //Debug.Log(ParentEnemy);
        // �G�̊�{AI�����X�N���v�g�擾
        enemyMove = ParentEnemy.GetComponent<EnemyMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        //Debug.Log(collision.gameObject.tag);

        // �����������̂��ЂтȂ�
        if (collision.gameObject.tag == CrackTag)
        {
            if (enemyMove != null)
            {
                // ���߂̈��̂ݓ���
                if (enemyMove.EnemyAI != EnemyMove.AIState.DEATH)
                {
                    // ���������Ђт�CrackOrder���擾
                    order = collision.gameObject.GetComponent<CrackCreater>();

                    //�������Ȃ�
                    if (order != null)
                    {
                        //if (order.State == CrackCreater.CrackCreaterState.CREATING || order.State == CrackCreater.CrackCreaterState.ADD_CREATING)
                        //{
                        //    // ���S��Ԃɂ���
                        //    enemyMove.EnemyAI = EnemyMove.AIState.DEATH;
                        //}

                        if(order.State != CrackCreater.CrackCreaterState.CRAETED)
                        {
                            // ���S��Ԃɂ���
                            enemyMove.EnemyAI = EnemyMove.AIState.DEATH;
                        }
                    }
                }
            }
        }
    }
}
