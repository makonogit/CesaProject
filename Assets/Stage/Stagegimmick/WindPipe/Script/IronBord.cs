//---------------------------------
//�@�S��:�����S
//�@�{�[�h�̓����蔻��
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronBord : MonoBehaviour
{
    
    Transform playertrans;      //Player��Transform
    PolygonCollider2D thiscol;  //���̃I�u�W�F�N�g��Collider
    EdgeCollider2D walkedge;    //�����͈͗p��EdgeCollider

    // Start is called before the first frame update
    void Start()
    {
        // player��Transform�擾
        playertrans = GameObject.Find("player").transform;
        // ���̃I�u�W�F�N�g��collider���擾
        thiscol = GetComponent<PolygonCollider2D>();
        // �����p��EdgeCollider
        walkedge = transform.GetChild(0).GetComponent<EdgeCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�@�v���C���[���Փ˂����瓖���蔻���ON�ɂ���
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("������");
             // Player��Position�ɂ����Trigger��ON�ɂ���
            walkedge.isTrigger = playertrans.position.y > transform.GetChild(0).transform.position.y ? false : true;
        }
    }



}
