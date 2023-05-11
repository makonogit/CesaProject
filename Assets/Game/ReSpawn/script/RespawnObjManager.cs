//---------------------------------
//�S���F��{��
//���e�F���X�|�[�������m���āA�e�e�I�u�W�F�N�g�ɏ��������߂��o��
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObjManager : MonoBehaviour
{
    // �ϐ��錾

    private bool Respawn = false;

    private CameraZoom _cameraZoom; // ���U���g�p
    public InitScoreCrystal _initScoreCrystal; // �X�R�A�N���X�^���������p�X�N���v�g

    // Start is called before the first frame update
    void Start()
    {
        _cameraZoom = GetComponent<CameraZoom>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���X�|�[�������Ƃ��Ɉ�x�������s
        if(Respawn == true)
        {
            Debug.Log("���X�|�[���}�l�[�W���[");

            // ���X�|�[�����Ƀ��Z�b�g�������I�u�W�F�N�g�̏������֐��Ăяo��
            _initScoreCrystal.ScoreCrystalInitStart(); // �X�R�A�N���X�^��
            _cameraZoom.RespawnInit();

            Respawn = false;
        }
    }

    public void RespawnInit()
    {
        Respawn = true;
    }
}
