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

    [SerializeField] private PlantEnemyMove enemyMove; // EnemyMove�X�N���v�g�擾�p�ϐ�
    private GameObject EnemySE;
    private PlayEnemySound enemyse; //���񂾉��p

    private void Start()
    {
        //// �G�̊�{AI�����X�N���v�g�擾
        //enemyMove = ParentEnemy.GetComponent<PlantEnemyMove>();

        EnemySE = GameObject.Find("EnemySE");
        enemyse = EnemySE.GetComponent<PlayEnemySound>();
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
                if (order != null)
                {
                    if (order.State == CrackCreater.CrackCreaterState.CREATING || order.State == CrackCreater.CrackCreaterState.ADD_CREATING)
                    {
                        if (enemyMove.EnemyAI == PlantEnemyMove.AIState.Attack || enemyMove.EnemyAI == PlantEnemyMove.AIState.Confusion || enemyMove.EnemyAI == PlantEnemyMove.AIState.Rage)
                        {
                            //SE�Đ�
                            enemyse.PlayEnemySE(PlayEnemySound.EnemySoundList.Destroy);
                            // ���S��Ԃɂ���
                            enemyMove.EnemyAI = PlantEnemyMove.AIState.Death;
                        }
                    }
                }
            }
        }
    }
}
