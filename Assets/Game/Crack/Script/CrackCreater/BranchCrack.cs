//---------------------------------
//�@�S��:�����S
//�@���e�F����Ђт̐����㏈��
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchCrack : MonoBehaviour
{
    GameObject ParentCrack; //�{�؂̂Ђ�
    CrackCreater ParentCrackCreater;    //�{�؂̂Ђт�CarckCreater
    CrackCreater creater;   //���̃I�u�W�F�N�g��CrackCreater

    [SerializeField, Header("�������镪��Ђ�")]
    private GameObject BranchObj;
    private CrackCreater branchcreater;

    [SerializeField, Header("�Ђт�Texture")]
    private Texture Crack;

    [SerializeField, Header("�Ђт̐�[Texture")]
    private Texture Crackend;

    private Hammer hammer;  //Hammer�X�N���v�g�A�Ђѐ����p

    private bool Create = false;    //�����t���O
    private int RandomCrack = 0;

    int StartBranch = 0;

    // Start is called before the first frame update
    void Start()
    {
        ParentCrack = transform.parent.gameObject;  //�{�؂̃q�r�̃I�u�W�F�N�g���擾
        ParentCrackCreater = ParentCrack.GetComponent<CrackCreater>();

        //Hammer�X�N���v�g�̎擾
        hammer = GameObject.Find("player").GetComponent<Hammer>();

        creater = GetComponent<CrackCreater>();
        branchcreater = BranchObj.GetComponent<CrackCreater>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (creater.GetState() == CrackCreater.CrackCreaterState.CRAETED)
        //{
        //    if (Create)
        //    {
        //        RandomCrack = Random.Range(0, 100); //0�`100�̗����擾
        //        //�����I��������R���C�_�[�𖳌���
        //        GetComponent<EdgeCollider2D>().enabled = false;
        //        Create = false;
        //    }
        //}

        ////�@�e�̂Ђт��ǉ��������ꂽ����̊m���ŕ���Ђт��L�т�
        //if (ParentCrackCreater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATING)
        //{
        //    if (!Create)
        //    {
        //        if (RandomCrack < 2)
        //        {
        //            if (creater.GetState() == CrackCreater.CrackCreaterState.CRAETED)
        //            {
        //                GetComponent<EdgeCollider2D>().enabled = true;
        //                //StartBranch = hammer.CreateBranch(BranchObj, gameObject, branchcreater, StartBranch);
        //                //creater.SetState(CrackCreater.CrackCreaterState.ADD_CREATEBACK);
        //            }
        //        }
        //        Create = true;
        //    }
        //}

        // �����I��������X�v���C�g�̕ύX
        if (creater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATE)
        {
            transform.GetChild(transform.childCount - 1).GetComponent<PointMatControl>().
                NormalMat.SetTexture("_MainTexture", Crack);
            GetComponent<EdgeCollider2D>().enabled = true;
        }

        // �����I��������X�v���C�g�̕ύX
        if (creater.GetState() == CrackCreater.CrackCreaterState.CRAETED)
        {
            transform.GetChild(transform.childCount - 1).GetComponent<PointMatControl>().
                NormalMat.SetTexture("_MainTexture",Crackend);
            GetComponent<EdgeCollider2D>().enabled = false;
        }
    }
}
