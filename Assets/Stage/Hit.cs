//---------------------------------------------
//�S���ҁF���Ԑ^���q
//���e�F�����蔻��i�S�[���j
//---------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hit : MonoBehaviour
{
   
    //---------------------------------------------------------
    //* ���������� *
    //---------------------------------------------------------
    private void Start()
    {

    }


    //---------------------------------------------------------
    //* �X�V���� *
    //---------------------------------------------------------
    void Update()
    {

    }


    //----------------------------------------------------------
    // * �����蔻��̏��� *
    //----------------------------------------------------------
    void OnTriggerEnter2D(Collider2D collider)
    {
      
        if (collider.gameObject.CompareTag("Goal"))
        {
            collider.gameObject.SetActive(false);

            // �S�[���I�u�W�F�N�g�ɓ���������N���A��ʂ�`�悷��
            SceneManager.LoadScene("ClearScene");
        }
    }
}
