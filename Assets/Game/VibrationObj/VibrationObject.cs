//---------------------------------
//担当：二宮怜
//内容：パーリンノイズを使ってスクリプトを
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationObject : MonoBehaviour
{
    // - 変数宣言 -

    // 単一のパーリンノイズ情報を格納する構造体
    [System.Serializable]
    private struct NoiseParam
    {
        // 振幅
        public float amplitude;

        // 振動の速さ
        public float speed;

        // パーリンノイズのオフセット
        [System.NonSerialized] public float offset;

        // 乱数のオフセット値を指定する
        public void SetRandomOffset()
        {
            offset = UnityEngine.Random.Range(0f, 256f);
        }

        // 指定自国のパーリンノイズ値を取得する
        public float GetValue(float time)
        {
            // ノイズ位置を計算
            var noisePos = speed * time + offset;

            // -1から1の範囲のノイズ値を取得
            var noiseValue = 2 * (Mathf.PerlinNoise(noisePos, 0) - 0.5f);

            // 振幅を掛けた値を返す
            return amplitude * noiseValue;
        }
    }

    // パーリンノイズのXY情報
    [System.Serializable]
    private struct NoiseTransform
    {
        public NoiseParam x, y;

        // xy成分に乱数のオフセット値を指定する
        public void SetRandomOffset()
        {
            x.SetRandomOffset();
            y.SetRandomOffset();
        }

        // 指定時刻のパーリンノイズを取得する
        public Vector3 GetValue(float time)
        {
            return new Vector3(
                x.GetValue(time),
                y.GetValue(time),
                0
                );
        }
    }

    // 位置の揺れ情報
    [SerializeField] private NoiseTransform _noisePosition;

    // 自身の位置情報
    private Transform thisTransform;

    // Transformの初期状態
    private Vector3 initLocalPosition;

    // 振動命令
    private bool Vibration = false;

    // 振動させる時間
    private float VibrationTime = 0.0f;

    // 振動が始まった時間
    private float StartVibrationTime = 0.0f;

    void Awake()
    {
        thisTransform = transform;

        // Transformの初期値を保持
        initLocalPosition = thisTransform.localPosition;

        // パーリンノイズのオフセット初期化
        _noisePosition.SetRandomOffset();
    }

    // Update is called once per frame
    void Update()
    {
        // 指定時間が経過したら
        if (Time.time - StartVibrationTime > VibrationTime)
        {
            if (Vibration == true)
            {
                // 振動終了
                Vibration = false;

                // 振動が終わったら初期位置に戻す
                thisTransform.localPosition = initLocalPosition;
            }
        }

        // 振動命令があれば一定時間振動
        if (Vibration)
        {
            // ゲーム開始からの時間取得
            var time = Time.time;

            // パーリンノイズの値を時刻から取得
            var noisePos = _noisePosition.GetValue(time);

            //Debug.Log(initLocalPosition);
            //Debug.Log(noisePos);

            // 各Transformにパーリンノイズの値を加算
            thisTransform.localPosition = new Vector3(initLocalPosition.x + noisePos.x, initLocalPosition.y + noisePos.y, initLocalPosition.z);
        }
    }

    // 関数呼び出しで指定時間振動させる
    public void SetVibration(float time)
    {
        initLocalPosition = thisTransform.localPosition;

        Vibration = true; // 振動命令セット
        VibrationTime = time; // 振動時間をセット
        StartVibrationTime = Time.time; // 振動開始時間をセット
    }

    // 振動中かを返す
    public bool GetVibration()
    {
        return Vibration;
    }
}
