//----------------------------------
// �S���F��
// �q�r����N���X�^���Ǘ��p
//----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCrackManager : MonoBehaviour
{
    [SerializeField,Header("�Ђѓ���N���X�^��")]
    private List<CrackStart> crackcrystal;
    int breakcrystal = 0;   //�Ђѓ��ꒆ�̃N���X�^��

    public bool clear = false;  //�X�e�[�W�N���A������

    // Update is called once per frame
    void Update()
    {
        //�@�X�e�[�W�N���A������J�n
        if (clear)
        {
            // ���ԂɂЂѓ�����n�߂Ă���
            if (breakcrystal < crackcrystal.Count)
            {
                if (!crackcrystal[breakcrystal].StartFlg)
                {
                    crackcrystal[breakcrystal].StartCrack();    //�Ђѓ���J�n
                }


                if (crackcrystal[breakcrystal].EndFlg)
                {
                    breakcrystal++;
                }
            }
        }
    }
}
