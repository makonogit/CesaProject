//-------------------------------------------
// �S���ҁF�����V�S
// ���e�@�FPlayResult�֐��Ń��U���g���o������
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    //=============================================
    // *** �ϐ��錾 ***
    //=============================================

    //---------------------------------------------
    // �ėp
    //---------------------------------------------

    int flameCnt;    // �t���[���J�E���g�p
    float screenWide;// ��ʂ̉���

    //---------------------------------------------
    // ��Ԋ֘A
    //---------------------------------------------

    enum StateID// ���ID
    {
        NULL,          // ��ԂȂ�
        RESULT_INIT,   // ���U���g���o����
        RESULT_UPDATE, // ���U���g���o��
        RESULT_END,    // ���U���g���o�I��
    }
    StateID oldState = StateID.NULL; // �O�̏��
    StateID nowState = StateID.NULL; // ���݂̏��
    StateID nextState = StateID.NULL;// ���̏��

    //---------------------------------------------
    // �j�Њ֘A
    //---------------------------------------------

    [Header("�j�Ђ̃I�u�W�F�N�g")]
    public GameObject[] debris = new GameObject[3];// �j�Зp�I�u�W�F�N�g

    //---------------------------------------------
    // �N���A�e�L�X�g�֘A
    //---------------------------------------------

    Vector3 textScale;// �����T�C�Y

    //---------------------------------------------
    // ���U���g�֘A
    //---------------------------------------------

    [Header("���o�I����̑ҋ@�t���[����")]
    public int standbyTim = 360;// ���o�I����̑ҋ@�t���[����

    //=============================================
    // *** ���������� ***
    //=============================================

    void Start()
    {
        //-----------------------------------------
        // �N���A�e�L�X�g�̑傫����ۑ�����
        //-----------------------------------------

        textScale = this.transform.localScale;


        //----------------------------------------------
        // �e�L�X�g�̉�����0�ɂ���
        //----------------------------------------------

        Transform textTransform = this.transform;
        Vector3 scale = textTransform.localScale;
        scale.x = 0.0f;
        textTransform.localScale = scale;

        //-----------------------------------------
        // ��ʕ������[���h���W�p�ɕϊ�����
        //-----------------------------------------

        screenWide = 0.03f * Screen.width;

        //PlayResult();
    }

    //=============================================
    // *** �X�V���� ***
    //=============================================

    void Update()
    {
        //-----------------------------------------
        // ���݂̏�Ԃɂ���ď����𕪊�
        //-----------------------------------------

        if (nextState != StateID.NULL)
        {
            oldState = nowState;
            nowState = nextState;
            nextState = StateID.NULL;
        }

        switch (nowState)
        {

            // ���U���g���o����
            case StateID.RESULT_INIT:
                ResultInit();
                break;
            // ���U���g���o��
            case StateID.RESULT_UPDATE:
                ResultUpdate();
                break;
            // ���U���g���o�I��
            case StateID.RESULT_END:
                ResultEnd();
                break;
        }
    }

    //=============================================
    // *** ���U���g���o�����@���� ***
    //=============================================

    void ResultInit()
    {
        //-----------------------------------------
        // �ύX�̏�����
        //-----------------------------------------

        flameCnt = 0;

        //----------------------------------------------
        // ��Ԃ�RESULT_UPDATE�ɕύX
        //----------------------------------------------

        nextState = StateID.RESULT_UPDATE;
    }

    //=============================================
    // *** ���U���g���o���@���� ***
    //=============================================

    void ResultUpdate()
    {
        //----------------------------------------------
        // ���݂̃t���[�������X�V
        //----------------------------------------------

        flameCnt++;

        //----------------------------------------------
        // �j�Ђ���ʂ̒[����[�܂Ő���
        //----------------------------------------------

        if (screenWide / 2 > 0.1f * flameCnt)
        {
            // �����_���Ɍ`�A���W�A�傫���A��]�����擾����
            int rndDebris = Random.Range(0, 3);
            int rndX = Random.Range(0, 21);
            int rndY = Random.Range(0, 21);
            int rndSizeX = Random.Range(1, 10);
            int rndSizeY = Random.Range(1, 10);
            int rndRot = Random.Range(1, 360);

            // �j�Ђ𐶐�
            GameObject debrisRist = Instantiate(debris[rndDebris], transform.position, Quaternion.identity);

            // ���������j�Ђ���transform���擾
            Transform objTransform = debrisRist.transform;

            // ���W��ύX
            Vector3 pos0 = objTransform.position;
            pos0.x = objTransform.position.x + screenWide / 4 - 0.1f * flameCnt;
            pos0.y = objTransform.position.y + 1.0f - 3.0f + 0.2f * rndY;
            pos0.z = 1.0f;//-0.9f;

            // �傫����ύX
            Vector3 scale;
            scale.x = 0.05f * rndSizeX;
            scale.y = 0.05f * rndSizeY;
            scale.z = 1.0f;

            // ��]��ύX
            Vector3 rot;
            rot.x = 1.0f;
            rot.y = 1.0f;
            rot.z = 1.0f * rndRot;

            // �ύX��G�p����
            objTransform.position = pos0;   // ���W
            objTransform.localScale = scale;// �傫��
            objTransform.eulerAngles = rot; // ��]
        }

        //----------------------------------------------
        // �[�܂ōs�������Ԃ�RESULT_END�ɂ���
        //----------------------------------------------

        else
        {
            // ������textScale�܂ő傫������
            Transform textTransform = this.transform;
            Vector3 scale = textTransform.localScale;
            if (textTransform.localScale.x < textScale.x)
            {
                scale.x += 6 * Time.deltaTime;
            }
            else
            {
                nextState = StateID.RESULT_END;
                flameCnt = 0;
            }

            textTransform.localScale = scale;

            //nextState = StateID.RESULT_END;
            //flameCnt = 0;
        }

        //----------------------------------------------
        // �N���A�e�L�X�g�̋����𐧌䂷��
        //----------------------------------------------

        if (screenWide <= 0.1f * flameCnt)
        {
            //Debug.Log(0.1f * flameCnt);
        }
        

    }

    //=============================================
    // *** ���U���g���o�I���@���� ***
    //=============================================

    void ResultEnd()
    {
        //----------------------------------------------
        // ���݂̃t���[�������X�V
        //----------------------------------------------

        flameCnt++;

        //----------------------------------------------
        // �ҋ@���Ԃ��o�߂�����Z���N�g��ʂɑJ��
        //----------------------------------------------

        if (standbyTim < flameCnt)
        {
            SceneManager.LoadScene("newSelectScene");
            nowState = StateID.NULL;
        }
    }

    //=============================================
    // *** ���U���g���o���J�n����֐� ***
    //
    // ���̊֐����Ăяo���Ɖ��o���J�n����
    //=============================================

    public void PlayResult()
    {
        //----------------------------------------------
        // ���̏�Ԃ�RESULT_INIT�ɃZ�b�g
        //----------------------------------------------

        if (nowState == StateID.NULL)
        {
            nextState = StateID.RESULT_INIT;
        }
    
    }
}
