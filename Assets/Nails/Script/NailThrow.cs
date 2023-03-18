//-------------------------------------
//�S���F�����S
//���e�F�B�𓊂���A�j���[�V����
//---------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailThrow : MonoBehaviour
{

    GameObject Player;      // �v���C���[�̃I�u�W�F�N�g
    HammerNail hammerNail;  // �B��łX�N���v�g

    Vector3 TargetPos;      // �B�̖ړI�n
    Vector3 NowNailPos;     // ���݂̓B�̈ʒu

    [Header("�B�𓊂���X�s�[�h")]
    public float ThrowSpeed; 

    [Header("�B�̖ړI�n�Ƃ̋���")]
    public float NailDistance;


    // Start is called before the first frame update
    void Start()
    {
        //------------------------------------------
        // �B��łX�N���v�g�̎擾
        Player = GameObject.Find("player");
        hammerNail = Player.GetComponent<HammerNail>();

        //------------------------------------------
        // �B�̍��W��ݒ�
        TargetPos = hammerNail.NailsTrans.position;
        NowNailPos = Player.transform.position;

        NailDistance = 100000;
        transform.position = NowNailPos;

    }

    // Update is called once per frame
    void Update()
    {

        //-----------------------------------------------
        //�ړI�n�ɍs���܂ō��W�ړ�
        NailDistance = Vector3.Magnitude(NowNailPos - TargetPos);
        if (NailDistance > 0)
        {
            //Debug.Log("�ړ�")
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, ThrowSpeed * Time.deltaTime);
            NowNailPos = transform.position;
        }
       
    }
}
