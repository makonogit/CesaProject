//-----------------------------------
//�S���F�����S
//���e�F�`���[�g���A���pUI�̕\��
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    //--------------------------------------
    //�@�ϐ��錾
    
    //--------------------------------------
    //�@�O���擾
    private GameObject Player;      // �v���C���[�̃I�u�W�F�N�g
    private Transform PlayerTrans;  // �v���C���[��Transform

    private Transform thisTrans;    // ���g��Transform

    [SerializeField,Header("�ǂ̂��炢�̋����ŕ\������邩")]
    private float OpenDistance;

    [SerializeField,Header("UI�̃T�C�Y")]
    private Vector3 UIsize;

    [SerializeField, Header("�g��k���X�s�[�h")]
    private float MoveSpeed;

    private bool OpenFlg = false;   //�\���t���O

    // Start is called before the first frame update
    void Start()
    {
        //------------------------------------
        // �v���C���[�̃I�u�W�F�N�g���擾
        Player = GameObject.Find("player");
        PlayerTrans = Player.transform;

        //�@���g��Transform
        thisTrans = transform;
    }

    // Update is called once per frame
    void Update()
    {

        // �v���C���[��UI�̋��������߂�
        float Distance = Vector3.Magnitude(PlayerTrans.position - thisTrans.position);
        
        //--------------------------------------------
        //�@�\�������܂ŋ߂Â�����UI�̕\���A�j���[�V�������Đ�
        if(Distance < OpenDistance)
        {
            OpenFlg = true;
        }

        if (OpenFlg)
        {
            OpenAnim();
        }

    }

    //-------------------------------
    // UI�̕\���A�j���[�V����
    private void OpenAnim()
    {
        //---------------------------------------------------
        // UI���g�傷��
        if (thisTrans.localScale.x < UIsize.x)
        {
            thisTrans.localScale = new Vector3(thisTrans.localScale.x + (MoveSpeed + 1) * Time.deltaTime, thisTrans.localScale.y, 1.0f);
        }
        if (thisTrans.localScale.y < UIsize.y)
        {
            thisTrans.localScale = new Vector3(thisTrans.localScale.x, thisTrans.localScale.y + MoveSpeed * Time.deltaTime, 1.0f);
        }
    }

}
