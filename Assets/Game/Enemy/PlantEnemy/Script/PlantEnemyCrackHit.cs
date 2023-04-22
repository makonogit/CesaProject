//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�v�����g��̓G���Ђтɓ��������玀��
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemyCrackHit : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -
    private string CrackTag = "Crack";

    // �O���擾
    private CrackCreater order = null;
    public GameObject ParentEnemy; // �e�I�u�W�F�N�g�̓G;
    private PlantEnemyMove enemyMove; // PlantEnemyMove�X�N���v�g�擾�p�ϐ�

    private void Start()
    {
        // �G�̊�{AI�����X�N���v�g�擾
        enemyMove = ParentEnemy.GetComponent<PlantEnemyMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // �����������̂��ЂтȂ�
        if (collision.gameObject.tag == CrackTag)
        {
            // ���߂̈��̂ݓ���
            if (enemyMove.EnemyAI != PlantEnemyMove.AIState.Death)
            {
                // ���������Ђт�CrackOrder���擾
                order = collision.gameObject.GetComponent<CrackCreater>();

                //�������Ȃ�
                if (order != null && (order.State == CrackCreater.CrackCreaterState.CREATING || order.State == CrackCreater.CrackCreaterState.ADD_CREATING))
                {
                    if (enemyMove.EnemyAI == PlantEnemyMove.AIState.Attack || enemyMove.EnemyAI == PlantEnemyMove.AIState.Confusion)
                    {
                        // ���S��Ԃɂ���
                        enemyMove.EnemyAI = PlantEnemyMove.AIState.Death;
                    }
                }
            }
        }
    }
}
