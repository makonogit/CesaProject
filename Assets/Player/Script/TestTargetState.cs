//------------------------------------
//�S���F�����S
//���e�F�Ə��̏�ԊǗ�(��)
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTargetState : MonoBehaviour
{
    public bool CheeckGround = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            CheeckGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            CheeckGround = false;
        }
    }
}
