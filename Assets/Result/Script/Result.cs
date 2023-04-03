//------------------------------------------------------------------------------
// �S���F�����V�S
// ���e�F���U���g
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    //--------------------------------------------------------------------------
    // - �ϐ��錾 -

    bool isFadeFlg = false;// ��ʂ����鏈����ON�AOFF

    // �t�F�[�h�֘A
    [Header("�t�F�[�h�A�E�g���鑬�x")]
    public float fadeSpeed = 0.001f;// �t�F�[�h�A�E�g���鑬�x
    float alpha = 0.0f;             // �p�l���̓����x

    // ��ʑJ�ڊ֘A
    int fadeDelay;// ��ʑJ�ڂ���܂ł̑ҋ@����

    // �j�Њ֘A
    [Header("�j�Ђ̃I�u�W�F�N�g")]
    public GameObject[] debris = new GameObject[3];// �j�Зp�I�u�W�F�N�g
    [Header("�j�Ђ̐�����")]
    public int amount = 95;  // �j�Ђ̐�����
    GameObject[] debrisRist = new GameObject[95];

    // ���֘A
    [Header("���ʉ�")]
    public AudioClip sound1;// �����t�@�C��
    AudioSource audioSource;// �擾����AudioSource�R���|�[�l���g

    // �N���A�e�L�X�g�֘A
    int[] clear = new int[]{

          1,1,1,0,1,0,0,0,1,1,1,0,0,1,0,0,1,1,0 ,
          1,0,0,0,1,0,0,0,1,0,0,0,1,0,1,0,1,0,1 ,
          1,0,0,0,1,0,0,0,1,1,1,0,1,1,1,0,1,1,1 ,
          1,0,0,0,1,0,0,0,1,0,0,0,1,0,1,0,1,0,1 ,
          1,1,1,0,1,1,1,0,1,1,1,0,1,0,1,0,1,0,1 ,
    };

    //============================================================
    // - ���������� -

    void Start()
    {
        // AudioSource�R���|�[�l���g���擾
        audioSource = GetComponent<AudioSource>();

        // ���̊֐��ŉ��o��ON�AOFF����
        //SetFadeFlg(true);
    }

    //============================================================
    // - �X�V���� -

    void Update()
    {
        //--------------------------------------------------------
        // ��ʂ������Ȃ��Ȃ�܂Ńt�F�[�h�A�E�g��A�j�Ђ𐶐�����

        if (isFadeFlg == true)
        {
            if (alpha >= 1.0f)
            {
                //--------------------------------------------------------
                // ���ʉ����Đ�

                audioSource.PlayOneShot(sound1);

                //--------------------------------------------------------
                // �j�Ђ𐶐�

                for (int i = 0; i < amount; i++)
                {

                    // �����_���Ɍ`�A���W�A�傫���A��]�����擾����
                    int rndDebris = Random.Range(0, 3);
                    int rndX = Random.Range(0, 21);
                    int rndY = Random.Range(0, 21);
                    int rndSizeX = Random.Range(1, 10);
                    int rndSizeY = Random.Range(1, 10);
                    int rndRot = Random.Range(1, 360);

                    // �j�Ђ𐶐�
                    debrisRist[i] = Instantiate(debris[rndDebris], transform.position, Quaternion.identity);

                    // �j�Ђ̃N���A��̍��W������
                    if (clear[i] == 1)
                    {
                        ResultDebris resultDebris = debrisRist[i].GetComponent<ResultDebris>();
                        resultDebris.clearPos.x = transform.position.x + -7.0f + 0.8f * (i % 19);
                        resultDebris.clearPos.y = 4.0f - 0.8f * (i / 19);
                        resultDebris.clearPos.z = 2.0f;
                    }

                    Transform objTransform = debrisRist[i].transform;

                    // ���W��ύX
                    Vector3 pos = objTransform.position;
                    pos.x = transform.position.x + -8.0f + 0.8f * rndX;
                    pos.y = -8.0f + 0.8f * rndY;
                    pos.z = 0.0f;

                    // �傫����ύX
                    Vector3 scale;
                    scale.x = 0.5f * rndSizeX;
                    scale.y = 0.5f * rndSizeY;
                    scale.z = 1.0f;

                    // ��]��ύX
                    Vector3 rot;
                    rot.x = 1.0f;
                    rot.y = 1.0f;
                    rot.z = 1.0f * rndRot;

                    // �ύX��G�p����
                    objTransform.position = pos;    // ���W
                    objTransform.localScale = scale;// �傫��
                    objTransform.eulerAngles = rot; // ��]

                }

                //--------------------------------------------------------
                // �t�F�[�h������OFF�ɂ���

                isFadeFlg = false;
                alpha = 0.0f;
                
            }
            else
            {

                //--------------------------------------------------------
                // ��ʂ������Ȃ��Ȃ�܂Ńt�F�[�h�A�E�g����

                // �����x�����Z
                alpha += fadeSpeed;
            }

            // alpha�̒l��G�p����
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);
        }

        //--------------------------------------------------------
        // ���o��t�F�[�h�A�E�g���ăZ���N�g��ʂɑJ�ڂ���

        fadeDelay++;

        if(fadeDelay > 3500)
        {
            if(alpha < 1.0f)
            {
                //--------------------------------------------------------
                // ��ʂ������Ȃ��Ȃ�܂Ńt�F�[�h�A�E�g����

                // �����x�����Z
                alpha += fadeSpeed;
                // alpha�̒l��G�p����
                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);
            }
            else
            {
                SceneManager.LoadScene("SelectScene");
            }
        }
    }

    //============================================================
    // - isScreenBreakFlg�p�Z�b�^�[ -

    public void SetFadeFlg(bool _isSet)
    {
        isFadeFlg = _isSet;
    }
    //============================================================
}
