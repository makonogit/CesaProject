//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�v���C���[�𔭌���������𑗂�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottedPlayer : MonoBehaviour
{
    // �ϐ��錾
    private string playerTag = "Player"; // ������Enemy�����ϐ�

    // �O���擾
    private PlantEnemyMove plantEnemyMove;
    private GameObject Parent; // �e�I�u�W�F�N�g
    private GameObject PlantEnemy;

    // Start is called before the first frame update
    void Start()
    {
        // �e�I�u�W�F�N�g�擾
        Parent = transform.parent.gameObject;

        // �q�I�u�W�F�N�g�擾
        PlantEnemy = Parent.transform.GetChild(2).gameObject;

        plantEnemyMove = PlantEnemy.GetComponent<PlantEnemyMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Player�^�O���ǂ���
        if (collision.gameObject.tag == playerTag)
        {
            // �������Ă�����𑗂�
            plantEnemyMove.PlayerHit = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Player�^�O���ǂ���
        if (collision.gameObject.tag == playerTag)
        {
            // �������Ă�����𑗂�
            plantEnemyMove.PlayerHit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Player�^�O���ǂ���
        if (collision.gameObject.tag == playerTag)
        {
            // ���G�͈͂���O�ꂽ���𑗂�
            plantEnemyMove.PlayerHit = false;
        }
    }
}
