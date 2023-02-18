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
    

    //------------------------------------------------------------------------------
    //�`�����������`
    void Start()
    {
        _crack = GetComponentInParent<Crack>();
        ScriptPIManager =_crack.PlayerInputManager.GetComponent<PlayerInputManager>();
    }

    //------------------------------------------------------------------------------
    //�`�X�V�����`
    void Update()
    {
        
        //---------------------------------------------------------
        //�I�u�W�F�N�g��Y���̃T�C�Y��ύX���ĕ\�����鏈��

        // ���͂̊p�x
        float angle = Mathf.Atan2(ScriptPIManager.GetRmove().y, ScriptPIManager.GetRmove().x) * Mathf.Rad2Deg;
        float radius = _crack.CrackPower / 2 * _rate;
        float radian = ((angle) / 180.0f) * Mathf.PI;

        this.transform.localPosition = new Vector3(radius * Mathf.Cos(radian), radius * Mathf.Sin(radian), 0);
        this.transform.localEulerAngles = new Vector3(0, 0, angle - 90);
        this.transform.localScale = new Vector3(this.transform.localScale.x, _crack.CrackPower * _rate, this.transform.localScale.z);
    }
}
