//---------------------------------------------------------
//担当者：二宮怜
//内容　：ポーズ関係のSEを管理する
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager_Pause : MonoBehaviour
{
    //---------------------------------------------------------------------
    // - 変数宣言 -

    public AudioClip se_select;
    public AudioClip se_cansel;
    public AudioClip se_ok;

    private AudioSource audioSource; // オブジェクトがもつAudioSourceを取得する変数

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
        // ボリューム調整
        audioSource.volume = 0.5f;

        audioSource.PlayOneShot(se_select);
    }

    public void PlaySE_Cansel()
    {
        // ボリューム調整
        audioSource.volume = 0.5f;

        audioSource.PlayOneShot(se_cansel);
    }

    public void PlaySE_OK()
    {
        // ボリューム調整
        audioSource.volume = 0.5f;

        audioSource.PlayOneShot(se_ok);
    }
}
