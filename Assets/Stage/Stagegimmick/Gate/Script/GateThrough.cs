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

    //[SerializeField, Header("�u���b�N��Transform")]
    //private Transform Blocktransform;

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        // �v���C���[�����蔲�����琶���ABGM�̍Đ�
        if(collision.tag == "Player" && collision.gameObject.transform.position.x > transform.position.x)
        {
            Destroy(GetComponent<BoxCollider2D>());
            GameObject.Find("BGM(Loop)").GetComponent<AudioSource>().Stop();
            GameObject.Find("BossBGM").GetComponent<AudioSource>().Play();
            GameObject obj = Instantiate(GateBlock, transform);
            obj.transform.parent = null;
        }
    }
}
