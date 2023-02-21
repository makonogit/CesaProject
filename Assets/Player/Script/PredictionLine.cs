//------------------------------------------------------------------------------
// �S���ҁF���쒼�o
// ���e  �F�Ђт̗\����
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionLine : MonoBehaviour
{
    //------------------------------------------------------------------------------
    //�`�ϐ��錾�`
    private float _rate =0.8f;//�\�����̃T�C�Y����
    private Crack _crack;
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�

    //--------���S��-------------
    private Vector3 CrackPower; //�Ђт�����͂�ێ�����ϐ�
    private GameObject Player;  //�v���C���[�I�u�W�F�N�g

    private bool CrackLine = false;

    //------------------------------------------------------------------------------
    //�`�����������`
    void Start()
    {
        Player = GameObject.Find("player");
        CrackPower = Vector3.zero;
        _crack = Player.GetComponent<Crack>();
        ScriptPIManager =_crack.PlayerInputManager.GetComponent<PlayerInputManager>();
    }

    //------------------------------------------------------------------------------
    //�`�X�V�����`
    void Update()
    {
        //--------------------------------------------------------
        //�E�X�e�B�b�N���͂����邩�̔���
        if (ScriptPIManager.GetCarackPower().x == 1 || ScriptPIManager.GetCarackPower().x == -1 ||
            ScriptPIManager.GetCarackPower().y == 1 || ScriptPIManager.GetCarackPower().y == -1 )
        {
            CrackPower = ScriptPIManager.GetRmove();
            CrackLine = true;
        }
        else
        {
            CrackLine = false;
        }

        //---------------------------------------------------------
        //�I�u�W�F�N�g��Y���̃T�C�Y��ύX���ĕ\�����鏈��

        // ���͂̊p�x
        float angle = Mathf.Atan2(CrackPower.y, CrackPower.x) * Mathf.Rad2Deg;
        float radius = _crack.CrackPower / 2 * _rate;
        float radian = ((angle) / 180.0f) * Mathf.PI;

        this.transform.localPosition = new Vector3(Player.transform.localPosition.x + radius * Mathf.Cos(radian), Player.transform.localPosition.y + radius * Mathf.Sin(radian), 0);
        this.transform.localEulerAngles = new Vector3(0, 0, angle - 90);
        this.transform.localScale = new Vector3(this.transform.localScale.x, _crack.CrackPower * _rate, this.transform.localScale.z);
        
        //--------------------------------------------
        //���͊O�ňړ�������L�����Z������
        if (!CrackLine)
        {
            if(ScriptPIManager.GetMovement().x > 0 || ScriptPIManager.GetMovement().x < 0)
            {
                CrackPower = Vector3.zero;
                _crack.CrackPower = 0;
            }
        }



    }

}
