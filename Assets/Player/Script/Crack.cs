//-----------------------------------
//�S���F�����S
//���e�F�Ђт̊p�x�E���W�E�����w�肵�Đ���
//-----------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crack : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    // �Ђѐ����p
    [Header("�Ђт������(�f�o�b�O�p�ɕ\��)")]
    public float CrackPower;   //�Ђт������

    [Header("�Ђт�����͂̉��Z��")]
    public float AddCrackPower;

    [Header("�ő�Ђѐ�����")]
    public float MaxCrackNum;

    [System.NonSerialized]
    public Vector3 CrackPos;    //�Ђт̍��W

    [System.NonSerialized]
    public bool CreateCrackFlg = false;

    // �O���擾
    public GameObject PlayerInputManager;       // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    private Transform Trans;                    // Transform���擾����ϐ�
    public GameObject PrehubObject;             // �Ђт𐶐�����I�u�W�F�N�g

    CrackOrder Order;

    [System.NonSerialized]
    public bool RadialCrackFlg = false;   //���ː���ɂЂт𐶐�

    // ��{�ǉ�
    private PlayerDamaged playerDamaged; // HP���炷���߂̃Z�b�^�[�Ăяo���p


    //----------------------------------------------------------------------------------------------------------
    // - ���������� -
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // �Q�[���I�u�W�F�N�gPlayerInputManager������PlayerInputManager�X�N���v�g���擾
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();
        
        Order = PrehubObject.GetComponent<CrackOrder>();
        
        //----------------------------------------------------------------------------------------------------------
        // Player��Transform���擾����
        Trans = this.GetComponent<Transform>();

        CrackPower = 0.0f;

        // ��{�ǉ�
        playerDamaged = GetComponent<PlayerDamaged>();

    }


    // Update is called once per frame
    void Update()
    {
        //---------------------------------------------
        //�͂�������
        if((ScriptPIManager.GetCarackPower().x == 1 || ScriptPIManager.GetCarackPower().x == -1) ||
            ScriptPIManager.GetCarackPower().y == 1 || ScriptPIManager.GetCarackPower().y == -1)
        {
            //----------------------------------------------------------------------------------------------------------
            // �Ђт����鋭�������Z
            CrackPower += AddCrackPower * Time.deltaTime;

            //-----------------------------------------------------------
            //���͗ʂɂ���Ċp�x�����߂�
            Order.CrackAngle = Mathf.Atan2(ScriptPIManager.GetRmove().y, ScriptPIManager.GetRmove().x) * Mathf.Rad2Deg;

            //-----------------------------------------------------------
            //�Ђт̍��W���X�V
            CrackPos = transform.position;

        }
        else if (CrackPower > 0.0f)
        {
            //�p���[�ɂ���ĕ��ː���ɂЂт�����
            if (CrackPower < MaxCrackNum)
            {
                CreateCrackFlg = true;
            }
            else if(CrackPower > MaxCrackNum)
            {
                CrackPower = 0.0f;
               // RadialCrackFlg = true;
            }
        }

        //--------------------------
        //�Ђт̌����w�肵�Đ���
        if (CreateCrackFlg)
        {
            //-------------------------------
            // HP�������Z�b�g
            playerDamaged.SetCrackInfo(CrackPower);

            Order.CrackFlg = CreateCrackFlg;
            Order.numSummon = CrackPower;
            Order.CrackPos = CrackPos;
            //-----------------------------
            //�����w�肵���珉�������ďI��
            CrackPower = 0.0f;
            CreateCrackFlg = false;
            GameObject obj = Instantiate(PrehubObject, CrackPos, Quaternion.identity);
        }

        //-----------------------------------------
        //���ː���ɂЂт�����
        if (RadialCrackFlg)
        {
            //for(int i = 0; i< 3; i++)
            //{
            //    Order.CrackFlg = RadialCrackFlg;
            //    Order.numSummon = Random.Range(CrackPower - 5.0f,CrackPower);
            //    Order.CrackPos = new Vector3(CrackPos.x - i * 0.5f,CrackPos.y,CrackPos.z);
            //    //-----------------------------
            //    //�����w�肵���珉�������ďI��
            //    CrackPower = 0.0f;
            //    GameObject obj = Instantiate(PrehubObject, CrackPos, Quaternion.identity);
            //}
            RadialCrackFlg = false;
        }

        //if(CrackPower.x > 0.0f)
        //{
        //    Order.numSummon = 10;
        //    Order.CrackFlg = true;
        //}

    }
}
