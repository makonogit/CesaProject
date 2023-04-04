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

    private int CoreNum;            // �R�A�̔ԍ�

    [SerializeField, Header("�s���~�b�h���o�Ă���܂ł̎���")]
    private float WaitTime; 
    private float TimeMeasure;      // ���Ԍv���p

    public GameObject PyramidList;  // �s���~�b�h�Ǘ��I�u�W�F�N�g
    
    [SerializeField]
    private List<int> Appearance;       // �o������s���~�b�h�ԍ�

    public bool Breaking = false;   // �s���~�b�h����ꂽ�� 

    public enum DesertBossState
    {
        NONE,   // �������Ă��Ȃ�
        ATTACK, // �U��
        CLEAN,  // �s���~�b�h��Еt����
    }

    public DesertBossState BossState;  // �{�X�̏�ԊǗ��p�ϐ�

    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------------------
        //�@��������s���~�b�h�̐����Q�[���I�u�W�F�N�g��ǉ�
        PyramidList = GameObject.Find("PyramidList");

        for(int i = 0; i < PyramidNum; i++)
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

        // �������Ă��Ȃ���Ԃɂ���
        BossState = DesertBossState.NONE;

    }

    // Update is called once per frame
    void Update()
    {
        if (BossState == DesertBossState.NONE)
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
                BossState = DesertBossState.CLEAN;
            }

            
        }

        //---------------------------------------
        //�@�U�����
        if(BossState == DesertBossState.ATTACK)
        {
            BossAttack();
        }

        //---------------------------------------
        //�@�Еt������
        if(BossState == DesertBossState.CLEAN)
        {
            PyramidClean();

        }
    }

    //------------------------------------
    //�@�{�X�̍U������
    private void BossAttack()
    {
        //-----------------------------------------
        //�@�o������s���~�b�h��ݒ�
        for(int i = 0; i < 3; i++)
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


        //-----------------------------------------------------------
        // �s���~�b�h���Ȃ���΃s���~�b�h�𐶐�(�q�I�u�W�F�N�g�ɂ���)
        GameObject Pyramid1 = GameObject.Find("Pyramid1");
        GameObject obj = PyramidList.transform.GetChild(Appearance[0]).gameObject;    // ��
        obj.transform.position = Pyramid1.transform.position;
        SpriteRenderer objrender = obj.GetComponent<SpriteRenderer>();
        objrender.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
      
        GameObject Pyramid2 = GameObject.Find("Pyramid2");
        GameObject obj2 = PyramidList.transform.GetChild(Appearance[1]).gameObject;   // �^��
        obj2.transform.position = Pyramid2.transform.position;
        SpriteRenderer obj2render = obj2.GetComponent<SpriteRenderer>();
        obj2render.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
     
        GameObject Pyramid3 = GameObject.Find("Pyramid3");
        GameObject obj3 = PyramidList.transform.GetChild(Appearance[2]).gameObject;   // �E
        obj3.transform.position = Pyramid3.transform.position;
        SpriteRenderer obj3render = obj3.GetComponent<SpriteRenderer>();
        obj3render.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        
        obj.transform.parent = Pyramid1.transform;
        obj2.transform.parent = Pyramid2.transform;
        obj3.transform.parent = Pyramid3.transform;


        BossState = DesertBossState.NONE;
    }


    //---------------------------------
    //�@�s���~�b�h��Еt���鏈��
    private void PyramidClean()
    {
        GameObject Pyramid1 = GameObject.Find("Pyramid1");
        GameObject Pyramid2 = GameObject.Find("Pyramid2");
        GameObject Pyramid3 = GameObject.Find("Pyramid3");

        Pyramid1.transform.GetChild(0).gameObject.GetComponent<PyramidData>().Clean = true;
        Pyramid2.transform.GetChild(0).gameObject.GetComponent<PyramidData>().Clean = true;
        Pyramid3.transform.GetChild(0).gameObject.GetComponent<PyramidData>().Clean = true;

    }

}
