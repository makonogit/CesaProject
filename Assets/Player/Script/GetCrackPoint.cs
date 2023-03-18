//-----------------------------------
//�S���F�����S
//���e�F�Ђт̏����쐬����
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCrackPoint : MonoBehaviour
{
    public List<GameObject> HitList;   //���������R���C�_�[�̏��̃��X�g

    float NearDistance;         //1�ԋ߂�����
    public int NearNailNum;            //1�ԋ߂��B�̔ԍ�

    Transform Playertransform;      //�v���C���[�̍��W
    CrackAutoMove _crackAutoMove;   //�Ђт̈ړ�����

    NailStateManager nailStateManager;  //�B�̏�Ԃ��擾

    Vector2 OldFirstPoint;  //1�O�̎n�_�B

    SetNailList _Setlist;               //�B�̏����擾
    public List<Vector2> PointList;     //�Ђт̐����p�|�C���g���X�g
    public List<GameObject> objectList; //�I�u�W�F�N�g���X�g

    // Start is called before the first frame update
    void Start()
    {
        //�v���C���[�̍��W���擾
        Playertransform = GetComponentInParent<Transform>();

        _crackAutoMove = GetComponentInParent<CrackAutoMove>();
    
        NearDistance = 10000;

        /*
        PointList = new List<Vector2>(2);   //���X�g���T�C�Y�m�ۂ��ď��������������ǂȂ񂩂ł��ւ�
        */

        objectList.Add(gameObject);
        PointList.Add(transform.position);
        //  PointList.Add(transform.position);

    }

    // Update is called once per frame
    void Update()
    {

        //���W�𓯊�
        transform.localPosition = Vector3.zero;

        PointList[0] = new Vector2(Playertransform.position.x, Playertransform.position.y);  //�Ђт̎n�_�͏�Ƀv���C���[�̍��W�ɐݒ�
      
        //-----------------------------------------
        //�Փ˂��Ă���I�u�W�F�N�g��1�ȏ゠���
        if (HitList.Count > 0)
        {
            OldFirstPoint = HitList[NearNailNum].transform.position;

            //----------------------------------
            //1�ԋ߂��B���擾
            for (int i = 0;i<HitList.Count; i++)
            { 
                 //���������߂�
                 float Distance = Vector3.Magnitude(transform.position - HitList[i].transform.position);


                //1�ԋ߂��������߂���������X�V
                if (NearDistance > Distance)
                {
                    NearDistance = Distance;
                    NearNailNum = i;
                }
              
            }

            //�O��̎n�_�ƈႤ�n�_�ɂȂ����烊�X�g��������
            if (PointList.Count > 1)
            {
                if (OldFirstPoint != PointList[1])
                {
                     Debug.Log("�n�_�̈ړ�");
                   
                    _Setlist.ChainFlg = false;
                    _Setlist.ThisPointNum = -1;
                    _Setlist.OldNailNum = -1;
                    for (int i = 1; i < PointList.Count; i++)
                    {
                        objectList.RemoveAt(i);
                        PointList.RemoveAt(i);
                    }

                }
            }
            
            //����Point��1�ԋ߂��B�ɐݒ�
            //PointList[1] = HitList[NearNailNum].transform.position;
            if (!PointList.Contains(HitList[NearNailNum].transform.position) &&  PointList.Count == 1)
            {
                PointList.Add(HitList[NearNailNum].transform.position);
                objectList.Add(HitList[NearNailNum].transform.root.gameObject);
            }

            //1�ԋ߂��B���Ȃ���
            _Setlist = HitList[NearNailNum].GetComponentInChildren<SetNailList>();
            _Setlist.ThisPointNum = 1;
            _Setlist.OldNailNum = 0;
            _Setlist.ChainFlg = true;
            //Debug.Log("Point[1]�ݒ芮��");

            //������������(�A�N�Z�X�ᔽ���N����A���ɕ��@������낤���ǎv������1:30�̂܂����)
            NearDistance = 10000;
            NearNailNum = 0;   

        }

    }

    //-------------------------------------------
    //���������u��HitList���X�V
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        //Debug.Log(collision.gameObject.transform.GetChild(0).gameObject.tag);
        if (collision.gameObject.tag == "UsedNail")
        {

            //���������R���C�_�[�����X�g��
            HitList.Add(collision.gameObject);

        }
    }

    //----------------------------------------
    //�͈͂���o����I�u�W�F�N�g������
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "UsedNail")
        {
            for (int i = 0; i < HitList.Count; i++)
            {
                //HitList�ɓ����Q�[���I�u�W�F�N�g����������
                if (HitList[i].transform.position == collision.gameObject.transform.position)
                {
                    //�I�u�W�F�N�g�����X�g�������
                    HitList.RemoveAt(i);
                }
            }

            if (_Setlist != null)
            {
                _Setlist.ChainFlg = false;
                _Setlist.ThisPointNum = -1;
                _Setlist.OldNailNum = -1;
            }
            for (int i = 1; i < PointList.Count; i++)
            {
                objectList.RemoveAt(i);
                PointList.RemoveAt(i);
            }

        }
    }


    //----------------------------------
    //�Ђт̃|�C���g�쐬�p�֐�
    //�����F�|�C���g���W
    //�߂�l�F�Ȃ�
    //----------------------------------
    public void SetPoint(Vector2 point)
    {
        //�|�C���g��ǉ�
        PointList.Add(point);
    }

    //----------------------------------
    //�Ђт̃|�C���g���X�g�擾�֐�
    //�����F�Ȃ�
    //�߂�l�FPointList
    //----------------------------------
    public List<Vector2> GetPointLest()
    {
        return PointList;
    }


    //----------------------------------
    //�Ђт̃|�C���g���X�g�폜�֐�
    //�����F�폜����|�C���g�ԍ�
    //�߂�l�F�Ȃ�
    //----------------------------------
    public void RemovePoint(int Pointnum)
    {
        PointList.RemoveAt(Pointnum);
    }



}
