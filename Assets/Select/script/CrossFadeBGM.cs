//--------------------------------
//担当：二宮怜
//内容：セレクトBGMクロスフェードさせる
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFadeBGM : MonoBehaviour
{
    // 変数宣言

    [SerializeField] private AudioSource BGM_1;
    [SerializeField] private AudioSource BGM_2;

    // 移動先のエリアを取得するため
    [SerializeField] private SelectArea _selectArea;

    public enum Sound
    {
        bgm_1,
        bgm_2,
    }

    // 現在なっているBGM
    Sound _sound = Sound.bgm_1;

    // true:クロスフェード開始
    private bool CrossFade = false;

    [SerializeField] private List<AudioClip> AudioClipList = new List<AudioClip>();

    [SerializeField] private float FadeSpeed = 0.002f;
    private float ElapsedTime = 0f; // 始まってからの経過時間
    [SerializeField] private float MaxVolume = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        BGM_1.volume = MaxVolume;
        BGM_2.volume = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // クロスフェード
        if(CrossFade == true)
        {
            if (_sound == Sound.bgm_1)
            {
                XFadeBGM(BGM_1,BGM_2);
            }
            else
            {
                XFadeBGM(BGM_2, BGM_1);
            }
        }

        //Debug.Log(BGM_1.volume);
    }

    public void PreXFadeBGM()
    {
        // この関数が呼び出された時になっていたAudioSourceがBGM_1なら
        if(_sound == Sound.bgm_1)
        {
            // 経過時間取得
            ElapsedTime = BGM_1.time;

            // BGM_2に移動先のエリアのAudioClipをセット
            BGM_2.clip = AudioClipList[_selectArea._nextArea];
            BGM_2.time = ElapsedTime;
            BGM_2.Play();
        }
        else
        {
            // 経過時間取得
            ElapsedTime = BGM_2.time;

            // BGM_1に移動先のエリアのAudioClipをセット
            BGM_1.clip = AudioClipList[_selectArea._nextArea];
            BGM_1.time = ElapsedTime;
            BGM_1.Play();
        }

        CrossFade = true;
    }

    // 第一引数:フェードアウトしていくAudioSource
    // 第二引数:フェードインしていくAudioSource
    private void XFadeBGM(AudioSource _audio1, AudioSource _audio2)
    {
        // フェードアウト
        if (_audio1.volume > 0)
        {
            _audio1.volume -= Time.unscaledTime * FadeSpeed;
        }
        else
        {
            _audio1.volume = 0;
        }

        // フェードイン
        if (_audio2.volume < MaxVolume)
        {
            _audio2.volume += Time.unscaledTime * FadeSpeed;
        }
        else
        {
            _audio2.volume = MaxVolume;
        }

        if(_sound == Sound.bgm_1)
        {
            BGM_1.volume = _audio1.volume;
            BGM_2.volume = _audio2.volume;
        }
        else
        {
            BGM_2.volume = _audio1.volume;
            BGM_1.volume = _audio2.volume;
        }

        // どちらも目標の値になったら
        if(_audio1.volume == 0 && _audio2.volume == MaxVolume)
        {
            // フェード終了
            CrossFade = false;

            // 今音を出しているAudioSourceを表す変数切り替え
            if(_sound == Sound.bgm_1)
            {
                _sound = Sound.bgm_2;
            }
            else
            {
                _sound = Sound.bgm_1;
            }
        }
    }
}
