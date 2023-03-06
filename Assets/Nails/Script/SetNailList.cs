//----------------------------------------
//�S���F�����S
//���e�F�Ђт̐������X�g��ǉ�
//----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SetNailList : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> HitList;   //�Փ˂����I�u�W�F�N�g�̃��X�g 

    public float NearDistance;         //1�ԋ߂�����
    public int NearNailNum;            //1�ԋ߂��B�̔ԍ�

    public int ThisPointNum = -1;   //���g�̃|�C���g�ԍ�(�ݒ肳��Ă��Ȃ�������-1)  

    GetCrackPoint _getCrackPoint;   //�Ђт̃|�C���g���쐬����X�N���v�g

    GameObject CrackCreateArea;     //�Ђт��쐬����p�̃I�u�W�F�N�g
    SetNailList _nextSet;           //����Point�Z�b�g�p

    GameObject AreaObj;
    CircleCollider2D thiscol;       //���̃I�u�W�F�N�g�̃R���C�_�[

    public int OldNailNum;          //1�O�̓B�ԍ�

    public bool ChainFlg = false;   //�Ȃ������炠���蔻������

    bool AddPointFlg = false;       //�|�C���g�ǉ��t���O

    // Start is called before the first frame update
    void Start()
    {
        //-------------------------------------------------------
        //�Ђт̃|�C���g�����擾
        CrackCreateArea = GameObject.Find("CrackCreateArea");
        _getCrackPoint = CrackCreateArea.GetComponent<GetCrackPoint>();

        //���̃I�u�W�F�N�g�̃R���C�_�[���擾
        thiscol = GetComponentInChildren<CircleCollider2D>();
        AreaObj = transform.FindChild("NailArea").gameObject;

        NearDistance = 10000;

    }

    // Update is called once per frame
    void Update()
    {

        if (ThisPointNum != -1)
        {
            if (_nextSet != null)
            {
                _nextSet.ChainFlg = true;
            }

            if (!AreaObj.GetComponent<CircleCollider2D>())
            {
                AreaObj.AddComponent<CircleCollider2D>();
                thiscol = GetComponentInChildren<CircleCollider2D>();
                thiscol.isTrigger = true;
            }

            //thiscol.radius = 0.5f;
            //thiscol.enabled = true;

        }
        else
        {
            HitList.Clear();
            if (_nextSet != null)
            {
                _nextSet.ChainFlg = false;
                _nextSet.ThisPointNum = -1;
                _nextSet.OldNailNum = -1;

            }

            if (AreaObj.GetComponent<CircleCollider2D>())
            {
                Destroy(thiscol);
            }
            
            //thiscol.radius = 0.0f;
            
            //thiscol.enabled = false;

        }

        //�Ȃ���Ȃ��Ȃ����玩�g��PointList����폜
        if (ThisPointNum == -1)
        {
          //  Debug.Log("�I�I�I�I");

           
        }

        //�������Ă���B�������
        if (HitList.Count > 0 && AddPointFlg)
        {

            for (int i = 0; i < HitList.Count; i++)
            {
                //------------------------------------------------------
                //���X�g�ɑ��݂��ĂȂ��B�̒�����1�ԋ߂��B���擾
                //���������߂�
                if (!_getCrackPoint.GetPointLest().Contains(HitList[i].transform.position))
                {
                    float Distance = Vector3.Magnitude(transform.position - HitList[i].transform.position);

                    //1�ԋ߂��������߂���������X�V
                    if (NearDistance > Distance)
                    {
                        NearDistance = Distance;
                        NearNailNum = i;
                    }
                }

                Debug.Log(NearDistance);
                //Debug.Log(NearNailNum);
            }

            _nextSet = HitList[NearNailNum].GetComponent<SetNailList>();
            //1�ԋ߂�Point�Z�b�g�X�N���v�g���Ă�Ōq����    
            //_nextSet.ChainFlg = true;
           
            if (ThisPointNum != -1)
            {
                _nextSet.OldNailNum = ThisPointNum;
                _nextSet.ThisPointNum = ThisPointNum + 1;
            }
            
            Debug.Log(ThisPointNum);

            //�����B�����݂��Ă��Ȃ�������|�C���g��ǉ�
            if (!_getCrackPoint.GetPointLest().Contains(HitList[NearNailNum].transform.position))
            {
                _getCrackPoint.SetPoint(HitList[NearNailNum].transform.position);
            }


            AddPointFlg = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log(collision.gameObject + "Enter");

        if (collision.gameObject.tag == "UsedNail")
        {
            Debug.Log(collision.gameObject + "Enter");

            //���X�g�ɓ����f�[�^���Ȃ����
            if (ChainFlg && !HitList.Contains(collision.gameObject))
            {
                //-------------------------------------------
                //�͈͓��ɓB����������HitList��ǉ�
                HitList.Add(collision.gameObject);

                AddPointFlg = true;   //PointList�̒ǉ�������

            }

        }

    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //   // Debug.Log(collision.gameObject);
    //    //if(collision.gameObject.tag == "UsedNail" && ChainFlg)
    //    //{
    //    //    for(int i = 0; i < HitList.Count; i++)
    //    //    {
    //    //        //HitList�ɓ����Q�[���I�u�W�F�N�g���Ȃ�
    //    //        if(HitList[i].transform.position != collision.gameObject.transform.position)
    //    //        {
    //    //            //-------------------------------------------
    //    //            //�͈͓��ɓB����������HitList��ǉ�
    //    //            HitList.Add(collision.gameObject);
    //    //        }
    //    //    }

    //    //    AddPointFlg = true;   //PointList�̒ǉ�������
    //    //    ChainFlg = false;     //1�񂾂������蔻������
    //    //}
        
    //}

}
