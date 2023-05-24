//---------------------------------------
//  �S���F��
//�@���e�F�N���A��ɂЂт������Ă������o
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCrack : MonoBehaviour
{
    [SerializeField, Header("����ЂтȂ̂�")]
    private bool Branch;

    [SerializeField,Header("1��̂Ђ�")]
    private SpriteRenderer ParentSprite;

    [SerializeField,Header("�Ђт̃��X�g")]
    private List<SpriteRenderer> CrackRender;

    [SerializeField, Header("�\�������X�s�[�h")]
    private float AnimSpeed;

    private float TimeMasure = 0.0f;   //���Ԍv���p

    private int NowCrack = 0;          //���ݕ\�����Ă���q�r�̔ԍ�

    // Update is called once per frame
    void Update()
    {
        // ����Ђт���Ȃ���΂��̂܂ܕ\��
        if (!Branch)
        {
            //�@���Ԍo�߂�����Ђт�\��
            if (TimeMasure > AnimSpeed && NowCrack < CrackRender.Count)
            {
                CrackRender[NowCrack].enabled = true;
                NowCrack++;         //���̂Ђт�ݒ�
                TimeMasure = 0.0f;  //���Ԃ̏�����
            }
            else
            {
                TimeMasure += Time.deltaTime;
            }
        }
        else
        {
            // �e���\�����ꂽ��\���J�n
            if (ParentSprite.enabled)
            {
                Branch = false;
            }   
        }

    }


}
