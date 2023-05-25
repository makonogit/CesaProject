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

    [Header("[�f�o�b�N��ON�AOFF]")]
    public bool debugging = false;

    int flameCnt;    // �t���[���J�E���g�p

    //---------------------------------------------
    // ��Ԋ֘A
    //---------------------------------------------

    public enum StateID// ���ID
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

    [Header("[�j�А����̐ݒ�]")]
    [Header("�E�����J�n�ʒu�i���̃I�u�W�F�N�g����_�j")]
    public Vector2 start = new Vector2(6.0f,2.0f);
    [Header("�E��������Ԋu�i�����j")]
    public Vector2 distance = new Vector2(0.02f, 0.5f);
    [Header("�E�j�Ђ̑傫��")]
    public float size = 0.02f;
    [Header("�E���t���[�����ɐ������邩")]
    public float flame = 2;
    [Header("�E�j�Ђ̈ړ����x")]
    public Vector2 move_speed = new Vector2(0.002f, 0.002f);
    [Header("�E�W�܂鎞�̔j�Ђ̉����x")]
    public float acceleration = 0.0002f;
    [Header("�E�j�Ђ̃I�u�W�F�N�g")]
    public GameObject[] debris = new GameObject[3];// �j�Зp�I�u�W�F�N�g
   
    bool isMoveFlg;                         // �j�Ђ̏W�܂�t���O

    //---------------------------------------------
    // �N���A�e�L�X�g�֘A
    //---------------------------------------------

    [Header("[�N���A�e�L�X�g�̐ݒ�]")]
    [Header("�E�e�L�X�g�̏o�����@�i0�ʏ�A1�����j")]
    int type = 1;// �e�L�X�g�̏o�����@

    // ��������p�ϐ�
    Vector3 textScale;// �����T�C�Y
    float scaleX;     // ���Z����傫��
    bool flg;

    //---------------------------------------------
    // ���U���g�֘A
    //---------------------------------------------

    [Header("[���U���g�̐ݒ�]")]
    [Header("�E���o�I����̑ҋ@�t���[����")]
    public int standbyTim = 1000;// ���o�I����̑ҋ@�t���[����

    //=============================================
    // *** ���������� ***
    //=============================================

    void Start()
    {
        //-----------------------------------------
        // �f�o�b�N��ON�Ȃ牉�K���J�n
        //-----------------------------------------

        if (debugging == true)
          PlayResult();

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
        isMoveFlg = false;

        flg = false;
        scaleX = 0.0f;

        //----------------------------------------------
        // �e�L�X�g�̉�����0�ɂ���
        //----------------------------------------------

        Transform textTransform = this.transform;
        Vector3 scale = textTransform.localScale;
        scale.x = 0.0f;
        textTransform.localScale = scale;

        //----------------------------------------------
        // ��Ԃ�RESULT_UPDATE�ɕύX
        //----------------------------------------------

        nextState = StateID.RESULT_UPDATE;
    }

    //=============================================
    // *** ���U���g���o�@���������� ***
    //=============================================

    void ResultUpdate()
    {
        //----------------------------------------------
        // ���݂̃t���[�������X�V
        //----------------------------------------------

        //flameCnt++;

        //----------------------------------------------
        // �j�Ђ𐶐�
        //----------------------------------------------

        if (start.x * 2 > distance.x * flameCnt)
        {
            //if(flameCnt % flame == 0)
            //{
            //    // �����_���Ɍ`�A���W�A�傫���A��]�����擾����
            //    int rndDebris = Random.Range(0, 3);
            //    int rndX = Random.Range(0, 10);
            //    int rndY = Random.Range(0, 10);
            //    int rndSizeX = Random.Range(1, 10);
            //    int rndSizeY = Random.Range(1, 10);
            //    int rndRot = Random.Range(1, 360);

            //    // �j�Ђ𐶐�
            //    GameObject debrisRist = Instantiate(debris[rndDebris], transform.position, Quaternion.identity);

            //    // ���������j�Ђ�transform���擾
            //    Transform objTransform = debrisRist.transform;

            //    // ���W��ύX
            //    Vector3 pos0;
            //    pos0.x = objTransform.position.x + start.x - distance.x * flameCnt;
            //    pos0.y = objTransform.position.y + start.y - distance.y * rndY;
            //    pos0.z = -0.9f;

            //    // �傫����ύX
            //    Vector3 scale;
            //    scale.x = size * rndSizeX;
            //    scale.y = size * rndSizeY;
            //    scale.z = 1.0f;

            //    // ��]��ύX
            //    Vector3 rot;
            //    rot.x = 1.0f;
            //    rot.y = 1.0f;
            //    rot.z = 1.0f * rndRot;

            //    // �ύX��G�p����
            //    objTransform.position = pos0;   // ���W
            //    objTransform.localScale = scale;// �傫��
            //    objTransform.eulerAngles = rot; // ��]

            //    // �j�Ђ̕ϐ���������
            //    debrisRist.GetComponent<ResultDebris>().move_speed = move_speed;
            //    debrisRist.GetComponent<ResultDebris>().acceleration = acceleration;
            //}


        }

        //----------------------------------------------
        // �������I��������RESULT_END�ɂ���
        //----------------------------------------------

        else
        {
            nextState = StateID.RESULT_END;
            flameCnt = 0;
        }

        //----------------------------------------------
        // �N���A�e�L�X�g�̋����𐧌䂷��
        //----------------------------------------------

        if ( start.x > flameCnt)
        {
            // ������textScale�܂ő傫������
            Transform textTransform = this.transform;  
            Vector3 scale = textTransform.localScale;

            // �e�L�X�g�̏o�����@�ɂ���ď����𕪊�
            switch (type)
            {
                case 0:
                    if (textTransform.localScale.x < textScale.x)
                    {
                        scale.x += textScale.x * 0.01f;
                    }
                    break;

                case 1:

                    if (textTransform.localScale.x < textScale.x + 2.0f)
                    {
                        scale.x += 0.03f + 0.00002f * flameCnt;
                    }
                    else
                    {
                        flg = true;
                    }

                    if (flg == true)
                    {
                        if (textTransform.localScale.x > textScale.x)
                        {
                            scale.x -= 0.06f + 0.00002f * flameCnt;
                        }
                        else
                        {
                            nextState = StateID.RESULT_END;
                        }
                    }
                    break;

                default:
                    if (textTransform.localScale.x < textScale.x)
                    {
                        scale.x += textScale.x * 0.01f;
                    }
                    break;
            }

            textTransform.localScale = scale;
        }
    }

    //=============================================
    // *** ���U���g���o�@�I������ ***
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

        if (standbyTim / 2 < flameCnt)
        {
            isMoveFlg = true;
        }

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

    public bool GetMoveFlg()
    {
        return isMoveFlg;
    }
}
