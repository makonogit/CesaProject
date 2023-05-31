//---------------------------------
//担当：二宮怜
//内容：パーリンノイズを使ってカメラを振動させる
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationCamera : MonoBehaviour
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
                -230
                );
        }
    }

    // 位置の揺れ情報
    [SerializeField] private NoiseTransform _noisePosition;

    // カメラの位置情報
    private Transform thisTransform;

    // Transformの初期状態
    public Vector3 initLocalPosition;

    // 振動命令
    private bool Vibration = false;

    // 振動させる時間
    private float VibrationTime = 0.0f;

    // 振動が始まった時間
    private float StartVibrationTime = 0.0f;

    // 外部取得
    private CameraControl2 _CameraControl;   //カメラ追従
    private SelectZoom _SelectCamera;        //セレクト画面のカメラ処理

    Gamepad gamepad;
    private float vibration_speed = 0.0f;   // 振動速度
    private float speed = 2.5f;             // 速度変動率

    void Awake()
    {
        thisTransform = transform;

        // Transformの初期値を保持
        initLocalPosition = thisTransform.localPosition;

        // パーリンノイズのオフセット初期化
        _noisePosition.SetRandomOffset();

        // 探す
        //PlayerInputMana = GameObject.Find("PlayerInputManager");
        //ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();
        //trigger = PlayerInputMana.GetComponent<InputTrigger>();
        _CameraControl = GetComponent<CameraControl2>();
        if(_CameraControl == null)
        {
            _SelectCamera = GameObject.Find("CameraControl").GetComponent<SelectZoom>();
        }

    }

    // Update is called once per frame
    void Update()
    {

       gamepad = Gamepad.current;


        //// ひび生成したら
        //if (trigger.GetNailTrigger_Right())
        //{
        //    // 振動処理
        //    initLocalPosition = thisTransform.localPosition;
        //    Vibration = true;
        //    VibrationTime = 1.0f; // 振動時間をセット
        //    StartVibrationTime = Time.time; // 振動開始時間をセット
        //    _CameraControl.enabled = false;

        //    // コントローラー振動
        //    gamepad.SetMotorSpeeds(0.0f, 0.5f);
        //}

        // 指定時間が経過したら
        if(Time.time - StartVibrationTime > VibrationTime)
        {
            if (Vibration == true)
            {
                // 振動終了
                Vibration = false;

                if (_CameraControl != null)
                {
                    //_CameraControl.enabled = true;
                    _CameraControl.VibrationStart(false); 
                }
                if(_SelectCamera != null)
                {
                    _SelectCamera._vibration = false;
                }
                // 振動が終わったら初期位置に戻す
                //thisTransform.localPosition = initLocalPosition;

                if (gamepad != null)
                {
                    // コントローラー振動
                    gamepad.SetMotorSpeeds(0.0f, 0.0f);
                }
            }
        }

        // 振動命令があれば一定時間振動
        if (Vibration)
        {
            // ゲーム開始からの時間取得
            var time = Time.time;

            // パーリンノイズの値を時刻から取得
            var noisePos = _noisePosition.GetValue(time);

            //Debug.Log(noisePos);

            // 各Transformにパーリンノイズの値を加算
            thisTransform.localPosition = new Vector3(initLocalPosition.x + noisePos.x,initLocalPosition.y + noisePos.y,-1.0f);
            //thisTransform.localPosition = initLocalPosition + noisePos;
            
        }
    }

    // 関数呼び出しで指定時間振動させる
    public void SetVibration(float time)
    {
        initLocalPosition = thisTransform.localPosition;

        Vibration = true; // 振動命令セット
        VibrationTime = time; // 振動時間をセット
        StartVibrationTime = Time.time; // 振動開始時間をセット
        if (_CameraControl != null)
        {
            _CameraControl.VibrationStart(true); //追従停止
        }
        if (_SelectCamera != null)
        {
            _SelectCamera._vibration = true;
        }
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
            // コントローラー振動
            gamepad.SetMotorSpeeds(time,time);
        }

    }

    public void SetControlerVibration()
    {
        
        //------------------------------
        //　振動を波打ちさせる
        if(vibration_speed > 1.0f)
        {
            speed = -3.0f;
        }
        if (vibration_speed < 0.0f)
        {
            speed = 4.0f;
        }

        vibration_speed += speed * Time.deltaTime;

        // コントローラー振動
        if (gamepad != null) gamepad.SetMotorSpeeds(vibration_speed, vibration_speed);
      
    }

    public void StopControlerVibration()
    {
        // コントローラー振動
        if (gamepad != null) gamepad.SetMotorSpeeds(0.0f,0.0f);
    }

    public bool GetVibration()
    {
        return Vibration;
    }
}
