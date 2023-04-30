//----------------------------------------------------
//�@���e�F�`���[�g���A���p�̃A�j���[�V������I������
//�@�S���F�����S
//----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSelect : MonoBehaviour
{
    //------------------------------
    //�@�ϐ��錾

    // �Đ�����A�j���[�V�����̎��
    public enum PlayAnim{
        Move,
        Hammer = 5,
        Jump = 4
    }

    [SerializeField,Header("�Đ�����A�j���[�V����")]
    private PlayAnim playanim;

    //--------------------------------
    //�@�O���擾
    private Animator anim;  //�@�`���[�g���A���p��Animator


    // Start is called before the first frame update
    void Start()
    {
        //�@���̃I�u�W�F�N�g��Animator���擾
        anim = GetComponent<Animator>();

        //---------------------------
        //�@�Đ�����A�j���[�V������I��
        anim.SetInteger("Select", (int)(playanim));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
