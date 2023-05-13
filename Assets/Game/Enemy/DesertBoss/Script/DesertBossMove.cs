//-----------------------------------------
//�@�S���F�����S
//�@���e�F�����̃{�X�̍s��
//-----------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossMove : MonoBehaviour
{
    //--------------------------------------
    // �ϐ��錾
    
    //--------------------------------------
    // �O���擾
    [SerializeField,Header("��������s���~�b�h�I�u�W�F�N�g")]
    private GameObject PyramidObj;

    [SerializeField, Header("�s���~�b�h�̐�")]
    private int PyramidNum;

    [SerializeField, Header("�s���~�b�h�̐����ʒu�I�u�W�F�N�g")]
    private List<Transform> PyramidPos;

    private GameObject[] Pyramid_parent;  // �s���~�b�h�����e�I�u�W�F�N�g

    private int CoreNum;            // �R�A�̔ԍ�

    [SerializeField, Header("�s���~�b�h���o�Ă���܂ł̎���")]
    private float WaitTime; 
    private float TimeMeasure;      // ���Ԍv���p

    [SerializeField,Header("�R�A���I�o���Ă���s���~�b�h���~���܂ł̎���")]
    private float EndTime;       

    public GameObject PyramidList;  // �s���~�b�h�Ǘ��I�u�W�F�N�g
    
    [SerializeField]
    private List<int> Appearance;       // �o������s���~�b�h�ԍ�

    public bool Breaking = false;   // �s���~�b�h����ꂽ�� 

    private VibrationCamera vibration;  //�@�U���p
    public bool Setvibratioin = false;

    private float Appearance_probability;        //�@�o���m��

    [SerializeField,Header("�Q�[�g")]
    private GateThrough gate;

    public enum DesertBossState
    {
        NONE,   // �������Ă��Ȃ�
        IDLE,   // �ҋ@
        ATTACK, // �U��
        CLEAN,  // �s���~�b�h��Еt����
        END     // �U���I��
    }

    public DesertBossState BossState;  // �{�X�̏�ԊǗ��p�ϐ�

    private Animator anim;             //�@�{�X�̃A�j���[�V����

    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------------------
        //�@��������s���~�b�h�̐����Q�[���I�u�W�F�N�g��ǉ�
        PyramidList = GameObject.Find("PyramidList");

        Pyramid_parent = new GameObject[3];
        //----------------------------------------------------
        //�@�s���~�b�h�̐e�擾
        for (int i = 0; i < 3; i++)
        {
            Pyramid_parent[i] = GameObject.Find("Pyramid" + (i + 1));
        }

        //--------------------------------------------
        //�@�s���~�b�h����
        for (int i = 0; i < PyramidNum; i++)
        {
            GameObject obj = Instantiate(PyramidObj);
            obj.transform.parent = PyramidList.transform;
            SpriteRenderer objrender = obj.GetComponent<SpriteRenderer>();
            objrender.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

        // �R�A�������Ă���s���~�b�h�����߂�
        CoreNum = Random.Range(0, PyramidNum - 1);
        PyramidData Data = PyramidList.transform.GetChild(CoreNum).gameObject.GetComponent<PyramidData>();
        Data.InsideNum = 1;

        // �������m��
        Appearance.Add(0);
        Appearance.Add(0);
        Appearance.Add(0);

        // animator���擾
        anim = transform.GetChild(0).GetComponent<Animator>();

        vibration = GameObject.Find("Main Camera").GetComponent<VibrationCamera>();

        Appearance_probability = 20.0f; //20���̊m���ɐݒ�

        // �������Ă��Ȃ���Ԃɂ���
        BossState = DesertBossState.NONE;

    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[���Q�[�g�𒴂�����
        if(gate.Throgh && BossState==DesertBossState.NONE)
        {
            // �G���o�Ă��鉉�o
            BossState = DesertBossState.IDLE;

        }

        // �������s���~�b�h�����݂��Ă��邩
        if(PyramidPos[0].childCount > 0 && PyramidPos[1].childCount > 0 && PyramidPos[2].childCount > 0)
        {
            //�@����ΐ��������擾
            bool Pyramid1 = PyramidPos[0].GetChild(0).GetComponent<PyramidData>().MoveFlg;
            bool Pyramid2 = PyramidPos[1].GetChild(0).GetComponent<PyramidData>().MoveFlg;
            bool Pyramid3 = PyramidPos[2].GetChild(0).GetComponent<PyramidData>().MoveFlg;

            //�@����ΕЕt�������擾
            bool Pyramid1_c = PyramidPos[0].GetChild(0).GetComponent<PyramidData>().Clean;
            bool Pyramid2_c = PyramidPos[1].GetChild(0).GetComponent<PyramidData>().Clean;
            bool Pyramid3_c = PyramidPos[2].GetChild(0).GetComponent<PyramidData>().Clean;


            //�@�S�Đ�������������U���A�j���[�V�����ɂ���
            anim.SetBool("Attack", Pyramid1 && Pyramid2 && Pyramid3);

            // �ǂꂩ��������or�Еt�����ŐU�����Ă��Ȃ�������
            if(((Pyramid1 && Pyramid2 && Pyramid3) || (Pyramid1_c && Pyramid2_c && Pyramid3_c)) && !Setvibratioin)
            {
                vibration.SetVibration(2.0f);
                Setvibratioin = true;
            }

        }


        if (BossState == DesertBossState.IDLE)
        {

            if (PyramidList.transform.childCount == PyramidNum)
            {
                // ���Ԍv��
                TimeMeasure += Time.deltaTime;

                //------------------------------------
                //�@���Ԍo�߂ōU���J�n
                if (TimeMeasure > WaitTime)
                {
                    BossState = DesertBossState.ATTACK;
                    TimeMeasure = 0.0f;
                }
            }
           
            //�@�ǂꂩ1����ꂽ��ЂÂ���
            if(Breaking)
            {
                Setvibratioin = false;
                BossState = DesertBossState.CLEAN;
            }

            
        }

        //---------------------------------------
        //�@�U�����
        if (BossState == DesertBossState.ATTACK)
        {
            BossAttack();
        }

        //---------------------------------------
        //�@�Еt������
        if(BossState == DesertBossState.CLEAN)
        {
            PyramidClean();

        }

        //---------------------------------------
        //�@�R�A���I�o�A�U���I��
        if(BossState == DesertBossState.END)
        {
            Debug.Log("�I��");
            //------------------------------------------
            //�@�w�莞�Ԍo�߂őS�ĕЕt����
            if (TimeMeasure > EndTime)
            {
                PyramidClean();
                //Destroy(gameObject);
            }
            else
            {
                TimeMeasure += Time.deltaTime;
            }

        }
    }

    //------------------------------------
    //�@�{�X�̍U������
    private void BossAttack()
    {
        //-----------------------------------------
        //�@�o������s���~�b�h��ݒ�
        do {
            for (int i = 0; i < 3; i++)
            {
                //-----------------------------
                //�@�d���`�F�b�N
                int num = Random.Range(0, PyramidNum - 1);
                if (!Appearance.Contains(num))
                {
                    Appearance[i] = num;
                }
                else
                {
                    i--;
                }
            }

            //  3�Ƃ����Ă����������x�ݒ�
        } while ( (PyramidList.transform.GetChild(Appearance[0]).gameObject.GetComponent<PyramidData>().Breaked && 
                   PyramidList.transform.GetChild(Appearance[1]).gameObject.GetComponent<PyramidData>().Breaked &&
                   PyramidList.transform.GetChild(Appearance[2]).gameObject.GetComponent<PyramidData>().Breaked));


        //-----------------------------------------------------------
        // �s���~�b�h���Ȃ���΃s���~�b�h�𐶐�(�q�I�u�W�F�N�g�ɂ���)
        GameObject obj = PyramidList.transform.GetChild(Appearance[0]).gameObject;    // ��
        obj.transform.position = Pyramid_parent[0].transform.position;
        SpriteRenderer objrender = obj.GetComponent<SpriteRenderer>();
        objrender.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
      
        GameObject obj2 = PyramidList.transform.GetChild(Appearance[1]).gameObject;   // �^��
        obj2.transform.position = Pyramid_parent[1].transform.position;
        SpriteRenderer obj2render = obj2.GetComponent<SpriteRenderer>();
        obj2render.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
     
        GameObject obj3 = PyramidList.transform.GetChild(Appearance[2]).gameObject;   // �E
        obj3.transform.position = Pyramid_parent[2].transform.position;
        SpriteRenderer obj3render = obj3.GetComponent<SpriteRenderer>();
        obj3render.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        
        obj.transform.parent = Pyramid_parent[0].transform;
        obj2.transform.parent = Pyramid_parent[1].transform;
        obj3.transform.parent = Pyramid_parent[2].transform;


        BossState = DesertBossState.IDLE;
    }


    //---------------------------------
    //�@�s���~�b�h��Еt���鏈��
    private void PyramidClean()
    {
        Pyramid_parent[0].transform.GetChild(0).gameObject.GetComponent<PyramidData>().Clean = true;
        Pyramid_parent[1].transform.GetChild(0).gameObject.GetComponent<PyramidData>().Clean = true;
        Pyramid_parent[2].transform.GetChild(0).gameObject.GetComponent<PyramidData>().Clean = true;
    }

}
