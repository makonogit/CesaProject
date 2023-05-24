//----------------------------
//�@�S���F�����S
//�@���e�F�G���A���Ƃ�BGM
//----------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIntroBGM : MonoBehaviour
{
    [SerializeField, Header("�X�e�[�W���Ƃ�IntroBGM")]
    private List<AudioClip> Intro_Bgm;

    [SerializeField] private BGMFadeManager _BGMfadeMana;

    // Start is called before the first frame update
    void Start()
    {
        SetStage stage = new SetStage();
        GetComponent<AudioSource>().clip = Intro_Bgm[stage.GetAreaNum()];
        GetComponent<AudioSource>().Play();

        _BGMfadeMana.BigStageBGM();
    }
}
