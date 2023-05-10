//---------------------------------
//�S���F��{��
//���e�F�v���C���[�̃��X�|�[�����W�X�V
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRespawn : MonoBehaviour
{
    // �ϐ��錾

    private string PlayerTag = "Player";

    private bool Used = false; // ���ɃZ�b�g���Ă��邩

    [Header("�X�e�[�W�����琔�������X�n�̏���")]public int RespawnNumber;

    private GameObject Child;
    private Vector3 RespawnPos;

    SetStage setstage = new SetStage();

    private bool Init = false;

    // �O���擾

    private StageManager stageManager;

    private PlayerStatas playerStatus; // ���X�|�[�����W����������

    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[���擾
        playerStatus = GameObject.Find("player").GetComponent<PlayerStatas>();

        Child = transform.GetChild(0).gameObject;

        // ���X�|�[����������W
        RespawnPos = Child.transform.position;
    }

    private void Update()
    {
        if(Init == false)
        {
            // �������X�|�[���n�_�I�u�W�F�N�g�Ȃ�X�e�[�W�f�[�^�̃X�e�[�W�v���n�u�̏����ʒu���擾
            stageManager = GameObject.Find("StageData").GetComponent<StageManager>();
            if (this.gameObject.name == "RespawnArea")
            {
                RespawnPos = stageManager.GetInitPlayerPos(setstage.GetAreaNum(), setstage.GetStageNum());

                //Debug.Log(RespawnPos);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������̂��v���C���[�Ȃ�
        if(collision.gameObject.tag == PlayerTag)
        {
            // ��x�����X�ݒ肵�Ă��Ȃ�
            if(Used == false)
            {
                if (playerStatus != null)
                {
                    // ���X�|�[�����̃X�e�[�^�X���擾
                    //RespawnStatus _respawnSta = playerStatus.GetRespawnStatus();

                    // ���g����ɂ��郊�X�|�[�����W��ݒ肵�Ă��Ȃ����
                    if (RespawnNumber >= playerStatus.respawnStatus.NowRespawnNumber)
                    {
                        if (playerStatus.GetCrystal() > playerStatus.respawnStatus.RespawnCrystalNum)
                        {
                            playerStatus.UpdateCrystalNum = true;
                        }

                        // ���X�|�[���ݒ�
                        playerStatus.SetRespawnNum(RespawnNumber); // �����牽�ڂ̃��X�n��
                        playerStatus.SetRespawn(RespawnPos); // ���X�|�[���n�_
                        playerStatus.SetRespawnCrystalNum(); // �����N���X�^����

                        //Debug.Log("���X�|�[���n�_�X�V");
                        //Debug.Log(playerStatus.respawnStatus.RespawnCrystalNum);

                        Used = true;
                    }
                    else
                    {
                        //Debug.Log("��̃��X�|�[���n�_���ݒ肳��Ă��܂�");
                    }
                }
            }
            else
            {
                //Debug.Log("�g�p�ς�");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // ���������̂��v���C���[�Ȃ�
        if (collision.gameObject.tag == PlayerTag)
        {
            // ��x�����X�ݒ肵�Ă��Ȃ�
            if (Used == false)
            {
                // ���X�|�[�����̃X�e�[�^�X���擾
                //RespawnStatus _respawnSta = playerStatus.GetRespawnStatus();

                if (playerStatus.respawnStatus != null)
                {
                    // ���g����ɂ��郊�X�|�[�����W��ݒ肵�Ă��Ȃ����
                    if (RespawnNumber > playerStatus.respawnStatus.NowRespawnNumber)
                    {
                        // ���X�|�[���ݒ�
                        playerStatus.SetRespawnNum(RespawnNumber);
                        playerStatus.SetRespawn(RespawnPos);

                        Used = true;
                    }
                }
            }
        }
    }
}
