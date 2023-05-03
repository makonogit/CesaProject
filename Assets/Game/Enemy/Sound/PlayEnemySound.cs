//--------------------------------
//  担当:菅眞心
//　内容：敵のSE再生
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEnemySound : MonoBehaviour
{
    public enum EnemySoundList
    {
        Attack,
        Destroy,
        PlantAttack
    }

    [SerializeField,Header("SoundList確認用")]
    private EnemySoundList EnemySound;  //敵のSEリスト

    [SerializeField, Header("敵のSEリスト ※EnemySoundListと同じ順")]
    //private List<AudioClip> EnemySE;
    private List<SE> EnemySE;

    private AudioSource Source;
    private void Start()
    {
        Source = GetComponent<AudioSource>();
    }

    //-----------------------------
    //　敵のSE再生
    public void PlayEnemySE(EnemySoundList enemySound)
    {
        // AudioClipとVolumeを設定
        Source.clip = EnemySE[(int)enemySound]._SE;
        Source.volume = EnemySE[(int)enemySound].Volume;

        //再生
        Source.Play();
    }

}

[System.Serializable]
public class SE
{
    public AudioClip _SE;   // SE管理
    public float Volume;   // ボリューム管理
 
    public SE(AudioClip _se,float Vol)
    {
        _SE = _se;
        Volume = Vol;
    }

}
