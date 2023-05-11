//-----------------------------------
//  �S���F�����S
//�@���e�F�^�C�g���̉��o
//-----------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
public class TitleDirection : MonoBehaviour
{
    Transform thistrans;    // ���̃I�u�W�F�N�g��Transform

    [SerializeField, Header("�ړI�n")]
    private Vector3 TargetPos;

    [SerializeField,Header("�҂�����")]
    private float WaitTime;     // �ړI�n�ɓ����������莞�ԑ҂�

    // �����p
    private Bloom bloom;
    private Volume volume;

    [SerializeField, Header("�N���X�^���p�l��")]
    private GameObject CrystalPanel;

    [SerializeField, Header("�^�C�g���̉��o�p")]
    private TitleMove _titlemove;

    [SerializeField, Header("TITLELOGO")]
    private Animator Titleanim;             //TitleLogo�̃A�j���[�V����

    [SerializeField, Header("PushA_Renderer")]
    private SpriteRenderer PushA;               //PushA
    [SerializeField, Header("PushA_Script")]
    private Button PushA_Script;

    private Animator anim;          //�A�j���[�V�����p

    private bool FlashFlg = false;  //�t���b�V���p�t���O
    private int FlashNum = 0;       //�t���b�V����

    private float TimeMasure;   // ���Ԍv���p

    private bool start = false;

    // Start is called before the first frame update
    void Start()
    {
        thistrans = transform;
        // Volume�֌W�擾
        volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        volume.profile.TryGet(out bloom);

        anim = GetComponent<Animator>();    //Animator���擾
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            start = true;
        }
        if (start)
        {
            // �G���ړ�������
            if (thistrans.position.x > TargetPos.x && TimeMasure < WaitTime)
            {
                thistrans.position = Vector3.MoveTowards(thistrans.position, TargetPos, 2.0f * Time.deltaTime);
            }
            else
            {

                TimeMasure += Time.deltaTime;   //�@���Ԍv���p

                // ��莞�ԑ҂�����t���b�V��������
                if (TimeMasure > WaitTime)
                {
                    // �t���b�V�����I�������
                    if (!Flash())
                    {
                        if (FlashNum < 1)
                        {
                            FlashNum++;
                            FlashFlg = false;

                            // ���o�p�̐ݒ�
                            CrystalPanel.SetActive(true);
                            _titlemove.enabled = true;
                            thistrans.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                            anim.enabled = true;
                            PushA.color = new Color(1.0f, 1.0f, 1.0f);
                            PushA_Script.enabled = true;
                        }
                        else
                        {
                            Titleanim.enabled = true;
                            thistrans.position = Vector3.MoveTowards(thistrans.position, new Vector3(9.78f, -3.1f, 0.0f), 2.0f * Time.deltaTime);
                        }

                    }
                }
                else
                {
                    anim.enabled = false;
                }
            }
        }
        
    }

    //------------------------------
    //�@��u���鉉�o�p�֐�
    private bool Flash()
    {
        if (!FlashFlg && bloom.intensity.value < 40.0f)
        {
            bloom.intensity.value += 50.0f * Time.deltaTime;
        }
        else
        {
            FlashFlg = true;
        }

        if (FlashFlg)
        {
            if (bloom.intensity.value <= 0.0f)
            {
               return false;
            }

            bloom.intensity.value -= 50.0f * Time.deltaTime;
        }

        return true;
    }
}
