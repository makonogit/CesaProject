//-----------------------------
//�S���F�����S
//���e�F�ǂ�����Ă�������(HP)
//-----------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallWall : MonoBehaviour
{
    //----------------------------
    //�ϐ��錾

    [System.NonSerialized]
    public GameObject NowCrackObj;     //�ŐV�̂ЂуI�u�W�F�N�g
    private CrackCreater Creater;      //�Ђт̐����V�X�e��
    private EdgeCollider2D CrackEdge;  //�Ђт̃R���C�_�[��ۑ�
  
    [System.NonSerialized]
    public bool CreateCrackFlg = false;     //�Ђт���������Ă��邩

    private float Cracklength;          //�Ђт̒���
    [SerializeField,Header("�ǂ̑ϋv�l")]
    private float WallEndress;          //�ǂ̑ϋv�l

    private RectTransform thisrect;     //���̃I�u�W�F�N�g��RectTrans
    private float UIsizeraito;          //UI�̃T�C�Y������

    // Start is called before the first frame update
    void Start()
    {
        Cracklength = 0.0f;     //������������
        WallEndress = 50.0f;   //�ǂ̑ϋv�ő�l
       
        thisrect = GetComponent<RectTransform>();

        UIsizeraito = thisrect.sizeDelta.x / WallEndress;   //UI�̃T�C�Y������

    }

    // Update is called once per frame
    void Update()
    {
        //--------------------------
        //�Ђт��������ꂽ�u��
        if (CreateCrackFlg)
        {
            //-----------------------------
            //�ŐV�̂Ђт̏����擾
            Creater = NowCrackObj.GetComponent<CrackCreater>();
            CreateCrackFlg = false;
    
        }

        //---------------------------------------
        //�Ђт̐������I�������璷�������߂�
        if (Creater != null && Creater.GetState() == CrackCreater.CrackCreaterState.CRAETED)
        {
            CracklengthAdd();

            //--------------------------------
            //��������ϋv�l�Q�[�W���k��������
            WallEndress -= Cracklength;
            thisrect.sizeDelta = new Vector2(thisrect.sizeDelta.x - (UIsizeraito * Cracklength), thisrect.sizeDelta.y);
            thisrect.position = new Vector3(thisrect.position.x - (UIsizeraito * Cracklength) / 2, thisrect.position.y);
            Cracklength = 0;

        }

    }

    //------------------------------
    //�ŐV�̂Ђт̒��������߂�
    private void CracklengthAdd()
    {
         //�v�Z�p�ɃG�b�W�R���C�_�[�̏����擾
         CrackEdge = Creater.Edge2D;

         //----------------------------------------
         //�Ђт̒������R���C�_�[�̒������狁�߂�
         if (CrackEdge != null)
         {
             //-------------------------------
             //�Ђт̃|�C���g�̐�����HP�v�Z
             for (int i = 0; i < Creater.Edge2D.pointCount - 1; i++)
             {
                 //�Ђт̒��������߂�
                 Cracklength += Vector3.SqrMagnitude(CrackEdge.points[i] - CrackEdge.points[i + 1]);
             }

         }
         Creater = null;
        
    }

}
