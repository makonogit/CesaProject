//----------------------------------
//  �S���F��
//�@���e�F�|�[�Y����snapshot�ؑ�
//----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseSnap : MonoBehaviour
{
  
    [SerializeField]
    private AudioMixerSnapshot _PauseSnap;   //�|�[�Y�p

    [SerializeField]
    private AudioMixerSnapshot _StartSnap;   //�J�n��


    // �|�[�Y�p�ɕύX
    public void PauseSnapChange()
    {
        _PauseSnap.TransitionTo(0.1f);
    }

    // �ʏ�ɕύX
    public void NormalSnapChange()
    {
        _StartSnap.TransitionTo(0.1f);
    }

}
