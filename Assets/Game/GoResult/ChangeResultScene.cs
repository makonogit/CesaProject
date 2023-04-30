//---------------------------------
//�S���F��{��
//���e�F�����𖞂������烊�U���g�V�[���Ɉڍs
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ChangeResultScene : MonoBehaviour
{
    // - �ϐ��錾 -
    public float time = 0.0f; // �N���X�^����S���󂵂Ă���̌o�ߎ���
    private float WaitTime = 2.0f; // �V�[���J�ڂ���܂ł̑҂�����

    // �O���擾
    private GameObject Stage;
    private StageStatas Stagestatus;
    private GameObject Resultobj;   // ���U���g���o�p�̃I�u�W�F�N�g
    private ResultManager resultmanager;
    private bool Firstcheck = false;
    public bool BossStage = false; // �{�X�X�e�[�W�p
    public float WaitFlame = 0.0f;

    //----�ǉ��ҁF���쒼�o----
    private Clear clear;// �N���A�������ǂ������Z���N�g�Ɏ����Ă���
    //------------------------
    
    // Start is called before the first frame update
    void Start()
    {
        Stage = GameObject.Find("StageData");
       
        //------------------------------------------------
        // ���U���g���o�p�̃V�X�e���擾
        Resultobj = GameObject.Find("Result_StageClear");
        resultmanager = Resultobj.GetComponent<ResultManager>();

        //----�ǉ��ҁF���쒼�o----
        clear = new Clear();
        //------------------------
    }

    // Update is called once per frame
    void Update()
    {
        

        if (!Firstcheck)
        {
            Stagestatus = Stage.transform.GetChild(0).GetComponent<StageStatas>();

            if (GameObject.Find("BossEnemy")) {
                BossStage = true;
            }
            Firstcheck = true;
        }

        // �{�X�X�e�[�W�p
        if (BossStage)
        {
            //�{�X��������Ȃ��Ȃ�����
            if (GameObject.Find("BossEnemy").transform.childCount == 0)
            {
                //�R�A��j�󂵂ă��U���g
                if (Stagestatus.GetStageCrystal() == 0 && WaitFlame > 0.2f)
                { 
                    Result();
                }
                //zoom���Ă��܂��̂őҋ@
                WaitFlame += Time.deltaTime;
            }

        }
        else
        {
            // �S�ăN���X�^�����󂵂��烊�U���g��ʂɈړ�
            if (Stagestatus.GetStageCrystal() == 0)
            {
                Result();
            }

        }
    }

    private void Result()
    {
        var gamepad = Gamepad.current;
        GameObject stage = GameObject.Find("StageData").transform.GetChild(0).gameObject;
        CameraZoom zoom = stage.GetComponent<CameraZoom>();
        GameObject.Find("player").GetComponent<PlayerMove>().SetMovement(false);
        //playerStatus.AddBreakCrystal();
        //���o�J�n
        //result.SetFadeFlg(true);

        if (gamepad != null)
        {
            //�@�U����~
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }
        if (zoom.ZoomEnd)
        {
            resultmanager.PlayResult();
        }
        //----�ǉ��ҁF���쒼�o----
        clear.GameClear();// �N���A����
                          //------------------------
    }

}
