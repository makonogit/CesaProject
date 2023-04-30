//--------------------------------------------
//�@�S���F�����S
//�@���e�F�v���C���[���G���A�ɓ��������̏���
//--------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateThrough : MonoBehaviour
{
    [SerializeField, Header("��������u���b�N")]
    private GameObject GateBlock;

    [SerializeField, Header("�u���b�N��Transform")]
    private Transform Blocktransform;

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �v���C���[�����蔲�����琶��
        if(collision.tag == "Player")
        {
            Destroy(GetComponent<BoxCollider2D>());
            GameObject obj = Instantiate(GateBlock, Blocktransform);
            obj.transform.parent = null;
        }
    }
}
