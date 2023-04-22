//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�p�C�v�ɂЂт�����������true��Ԃ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeHitCrack : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -
    private string CrackTag = "Crack";

    // �O���擾
    private CrackCreater order = null;
    public GameObject EnemyObj; // plantenemymove�����I�u�W�F�N�g;
    private PlantEnemyMove enemyMove; // PlantEnemyMove�X�N���v�g�擾�p�ϐ�
    private void Start()
    {
        // �G�̊�{AI�����X�N���v�g�擾
        enemyMove = EnemyObj.GetComponent<PlantEnemyMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        //Debug.Log(collision.gameObject.tag);

        // �����������̂��ЂтȂ�
        if (collision.gameObject.tag == CrackTag)
        {
            // ���������Ђт�CrackOrder���擾
            order = collision.gameObject.GetComponent<CrackCreater>();

             //�������Ȃ�
             if (order.State == CrackCreater.CrackCreaterState.CREATING || order.State == CrackCreater.CrackCreaterState.ADD_CREATING)
             {
                Destroy(collision.gameObject);

                // �Ђт������������𑗂�
                enemyMove.CrackInPipe = true;

                // �Ђтɓ����������̃p�C�v�̏��𑗂�
                enemyMove.CrackInObject = this.gameObject;
            }
        }
    }
}
