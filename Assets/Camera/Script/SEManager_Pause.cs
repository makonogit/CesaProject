//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�|�[�Y�֌W��SE���Ǘ�����
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager_Pause : MonoBehaviour
{
    //---------------------------------------------------------------------
    // - �ϐ��錾 -

    public AudioClip se_select;
    public AudioClip se_cansel;
    public AudioClip se_ok;

    private AudioSource audioSource; // �I�u�W�F�N�g������AudioSource���擾����ϐ�

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySE_Select()
    {
        // �{�����[������
        audioSource.volume = 0.5f;

        audioSource.PlayOneShot(se_select);
    }

    public void PlaySE_Cansel()
    {
        // �{�����[������
        audioSource.volume = 0.5f;

        audioSource.PlayOneShot(se_cansel);
    }

    public void PlaySE_OK()
    {
        // �{�����[������
        audioSource.volume = 0.5f;

        audioSource.PlayOneShot(se_ok);
    }
}
