//-------------------------------------
// �S���F�����S
// ���e�F�Ђт̕���
//-------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchCreater : MonoBehaviour
{
    //--------------------------
    //�@�ϐ��錾
    [SerializeField,Header("�������镪��Ђ�")]
    private GameObject BranchObj;       //�������镪��Ђ�
    private CrackCreater branchcreater; //����Ђт�CrackCreater
    private CrackCreater creater;       //���̃I�u�W�F�N�g��CrackCreater


    [SerializeField,Header("�Ђт̃X�v���C�g")]
    private Sprite CrackSprite;

    [SerializeField, Header("�Ђт̐�[�X�v���C�g")]
    private Sprite CrackEndSprite;

    private Hammer hammer;      //Hammer�X�N���v�g

    private bool Branch = false;    //1�񔻒�p

    private List<Vector2> PointList;    //�Ђт�PointList

    private int StartBranch = 1;    //����Ђт̐����J�n�l

    // Start is called before the first frame update
    void Start()
    {
        //Creater�̎擾
        creater = GetComponent<CrackCreater>();
        branchcreater = BranchObj.GetComponent<CrackCreater>();

        //Hammer�X�N���v�g�̎擾
        hammer = GameObject.Find("player").GetComponent<Hammer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(creater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATE)
        {
            //�@��[�̃X�v���C�g��ύX
            transform.GetChild(transform.childCount - 1).GetComponent<SpriteRenderer>().sprite = CrackSprite;

        }

        if (creater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATING)
        {
            if (Branch)
            {
                StartBranch++;
                Branch = false;
            }
        }

        //�@�������I�����Ă�����
        if (creater.GetState() == CrackCreater.CrackCreaterState.CRAETED)
        {

            if (!Branch)
            {
                //�@��[�̃X�v���C�g��ύX
                //transform.GetChild(transform.childCount - 1).GetComponent<SpriteRenderer>().sprite = CrackEndSprite;
                //�@����Ђѐ���
                //StartBranch = hammer.CreateBranch(BranchObj,gameObject,branchcreater,StartBranch);

                Branch = true;
            }
        }
    }

    public void SetPointList(List<Vector2> pointlist)
    {
        PointList = pointlist;
    }
}