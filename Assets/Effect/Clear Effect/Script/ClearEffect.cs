//------------------------------------------------------------------------------
// �S���F�����V�S
// ���e�F�N���A���o
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEffect : MonoBehaviour
{
    //--------------------------------------------------------------------------
    // - �ϐ��錾 -

    bool isFadeFlg = false;// ��ʂ����鏈����ON�AOFF

    // �t�F�[�h�֘A
    [Header("�t�F�[�h�A�E�g���鑬�x")]
    public float fadeSpeed = 0.001f;// �t�F�[�h�A�E�g���鑬�x
    float alpha = 0.0f;             // �p�l���̓����x

    // �j�Њ֘A
    [Header("�j�Ђ̃I�u�W�F�N�g")]
    public GameObject debris;// �j�Зp�I�u�W�F�N�g
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

        SetFadeFlg(true);
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

                    // �����_���ɍ��W�A�傫���A��]�����擾����
                    int rndX = Random.Range(0, 21);
                    int rndY = Random.Range(0, 21);
                    int rndSizeX = Random.Range(1, 10);
                    int rndSizeY = Random.Range(1, 10);
                    int rndRot = Random.Range(1, 360);

                    // �j�Ђ𐶐�
                    debrisRist[i] = Instantiate(debris, transform.position, Quaternion.identity);

                    // �j�Ђ̃N���A��̍��W������
                    if (clear[i] == 1)
                    {
                        ClearDebris clearDebris = debrisRist[i].GetComponent<ClearDebris>();
                        clearDebris.clearPos.x = -7.0f + 0.8f * (i % 19);
                        clearDebris.clearPos.y = 4.0f - 0.8f * (i / 19);
                        clearDebris.clearPos.z = 2.0f;
                    }

                    Transform objTransform = debrisRist[i].transform;

                    // ���W��ύX
                    Vector3 pos = objTransform.position;
                    pos.x = -8.0f + 0.8f * rndX;
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
    }

    //============================================================
    // - isScreenBreakFlg�p�Z�b�^�[ -

    public void SetFadeFlg(bool _isSet)
    {
        isFadeFlg = _isSet;
    }
    //============================================================
}