//---------------------------------
//�@�S���F�����S
//�@�v�����g��{�X�̃_���[�W����
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�@�Ђт��������������
        if(collision.tag == "Crack")
        {
            Destroy(gameObject);
        }
    }
}
