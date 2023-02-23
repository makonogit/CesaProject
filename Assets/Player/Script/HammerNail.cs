//-----------------------------------------
//�S���ҁF�����S
//����  �F�B��ł�
//-----------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class HammerNail : MonoBehaviour
{
    private HaveNails HaveNails;          //�B�̏��������擾

    private int HammerNails;              //�B��ǂɑł�����

    [Header("�B�̃v���n�u�I�u�W�F�N�g")]
    public GameObject NailPrehubObj;      //�B�̃v���n�u

    Transform NailsTrans;                 // �B��TransForm

    // �O���擾
    private GameObject PlayerInputManager;      // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    public GameObject CrackPrefab;              // �Ђѐ����p�v���n�u�I�u�W�F�N�g
    private CrackOrder Order;                   // �Ђѐ����p�X�N���v�g

    [SerializeField,Header("�B�������\�ȋ���")]
    private float NailsCreateDistance = 2.5f;   //�B�������\�ȋ���
    public float NailsDistance;                 //�O��̓B�Ƃ̋���
    [SerializeField,Header("�B�������\��")]
    private bool NailsCreateFlg = true;         //�B�������\��
    public bool MousePointaHit = false;         //�}�E�X�|�C���^���ǂɓ������Ă��邩

    bool CreateCrack = false;                   //�Ђт��������ꂽ��
    public List<Vector2> NailsPoint;            //�B�̍��W���擾

    private GameObject MousePointa;             //�}�E�X�|�C���^�p�I�u�W�F�N�g
    private SpriteRenderer MousePointaRender;   //�}�E�X�|�C���^�̃X�v���C�g�����_�[

    // Start is called before the first frame update
    void Start()
    {
        HammerNails = 0;
        NailsDistance = 0.0f;

        //--------------------------------------------
        // �B�̊Ǘ��X�N���v�g���擾
        HaveNails = GetComponent<HaveNails>();
        
        NailsTrans = NailPrehubObj.transform;

        //--------------------------------------------
        //�@�Ђѐ����p�X�N���v�g���擾
        Order = CrackPrefab.GetComponent<CrackOrder>();

        //--------------------------------------------
        //�}�E�X�|�C���^�p�I�u�W�F�N�g���擾
        MousePointa = GameObject.Find("MousePointa");
        MousePointaRender = MousePointa.GetComponent<SpriteRenderer>();

        //--------------------------------------------
        //InputManagr���擾
        PlayerInputManager = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();

    }

    // Update is called once per frame
    void Update()
    {
        //�}�E�X�|�C���^�̍��W����ɍX�V
        Vector3 MousePos = (Vector2)Camera.main.ScreenToWorldPoint(ScriptPIManager.GetMousePos());
        MousePointa.transform.position = MousePos;

        //----------------------------------------
        //�B�������Ă�����B����ʂɑł�
        if (HaveNails.NailsNum > 0)
        {
            //---------------------------------------------------------------------------
            //�O��ł����B�Ƃ̋��������߂�(��������Ă��Ȃ�������v���C���[�Ƃ̋���)
            if (HammerNails > 0)
            {
                NailsDistance = Vector3.Distance(MousePos, NailsPoint[HammerNails - 1]);
            }
            else
            {
                NailsDistance = Vector3.Distance(MousePos, this.transform.position);
            }

            //----------------------------------------------------------------
            //�����������\�ȋ�����������Ă��邩�Ő����\�t���O���w��
            if(NailsDistance < NailsCreateDistance && !MousePointaHit)
            {
                NailsCreateFlg = true;
            }
            else
            {
                NailsCreateFlg = false;
            }
           
            //----------------------------------------------------------------
            //�B�����\���ǂ����Ń}�E�X�|�C���^�̐F��ύX
            MousePointaRender.color = NailsCreateFlg == true ? Color.white : Color.red;

            //---------------------------------------------------------------------------
            //���N���b�N���m+�Ђѐ���������Ȃ�+�B��łĂ�͈͂Ȃ�}�E�X���W�ɓB�𐶐�
            if (Mouse.current.leftButton.wasPressedThisFrame && !CreateCrack && NailsCreateFlg)
            {
                NailsTrans.position = new Vector3(ScriptPIManager.GetMousePos().x, ScriptPIManager.GetMousePos().y, 0);
                // ���[���h���W�ɕϊ�Z�̃J�������W�����������Ȃ�̂�Vector2�^�ɃL���X�g�ϊ����đΏ�
                NailsTrans.position = (Vector2)Camera.main.ScreenToWorldPoint(NailsTrans.position);

                //�|�C���g���W��ǉ�
                NailsPoint.Add(item:NailsTrans.position);

                Instantiate(NailPrehubObj,NailsTrans.position,Quaternion.identity);

                HammerNails++;
                HaveNails.NailsNum--;
            }
           
        }

        //----------------------------------------
        //�ǂɑł����B��1�ȏ゠��΂Ђт𐶐�
        if (HammerNails > 0)
        {
            //-------------------------------------
            //�E�N���b�N���m�łЂт𐶐�
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                CreateCrack = true;
            }

            //------------------------------------
            //�Ђѐ���
            if (CreateCrack)
            {
                Order.numSummon = HammerNails;
                Order.CrackPos = NailsPoint[NailsPoint.Count - 1];
                Order.CrackFlg = CreateCrack;
                //Quaternion.LookRotation()
                HammerNails = 0;
                Instantiate(CrackPrefab, NailsPoint[NailsPoint.Count - 1], Quaternion.identity);
                CreateCrack = false;
            }
        }



    }
}
