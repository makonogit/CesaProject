//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F���Ђ��ǂɓ������������
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakShard : MonoBehaviour
{
    // �ϐ��錾
    private string GroundTag = "Ground";

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������̂�Ground�^�O��������
        if(collision.gameObject.tag == GroundTag)
        {
            // ������I�u�W�F�N�g�i���g�j������
            Destroy(this.gameObject);
        }
    }
}
