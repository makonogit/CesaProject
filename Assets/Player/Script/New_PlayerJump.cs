//---------------------------------------------------------
//担当者：二宮怜
//内容　：プレイヤージャンプ改訂版
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_PlayerJump : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -
    private float MoveX = 0f; // 使用の予定なし
    public float MoveY = 0f; // そのフレームでのプレイヤーのy軸移動量

    // フラグ変数
    [Header("デバッグ用変数")]
    [Header("状態系")]
    [SerializeField] private bool TriggerJumpflg = false; // ジャンプもしてなく地面にいるときにジャンプボタンが押されたら
    [SerializeField] private bool PressJumpflg = false; // ジャンプボタンが押されている間true
    [SerializeField] private bool OldPressJumpflg = false; // 前フレームの押下状況を保持
    [SerializeField] private bool ReleaseJumpflg = false; // ジャンプを離したらtrue
    [SerializeField] public bool ImFly = false; // 空中にいるか
    [SerializeField] private bool ImDrop = false; // ジャンプを経由しない落下

    // 接地判定系
    [Header("接地判定系")]
    [SerializeField] private bool isGround = false; // 地面に触れているか
    [SerializeField] private bool isOverhead = false; // 天井に触れているか
    [Header("円形のレイ用")]
    [SerializeField] float groundCheckRadius = 0.4f; // 半径
    [SerializeField] float groundCheckOffsetX = 0.45f; // オフセット
    [SerializeField] float groundCheckOffsetY = 0.45f; // オフセット
    [SerializeField] float groundCheckDistance = 0.2f; // キャストする最大距離 （円の半径＋この変数）がキャスト距離？
    // 10 : Ground
    // 14 : Block
    // 17 : EnemyPipe
    // 18 : Pipe
    // 21 : Trolley
    // 22 : IgnoreOverHead
    private LayerMask layerMask = 1 << 10 | 1 << 14 | 1 << 17 | 1 << 18 | 1 << 21 | 1 << 22;
    [SerializeField, Tooltip("レイの長さを調整します。")]
    private float _length = 0.01f;

    // 調整可能変数
    [Header("調整用変数")]
    [SerializeField] private float JumpPower = 3.0f; // ジャンプ力
    [SerializeField] private float gravity; // ジャンプパワーから引いてく重力
    [Header("慣性が働くフレーム数"),SerializeField] private int inertia = 5; // 慣性フレーム数

    // デバッグ用変数
    private int count = 0;

    // トロッコ用
    public bool RideOn = false;

    // HitStop中かどうか
    public bool hitStop = false;

    // プレイヤーの向き
    private PlayerInputManager.DIRECTION oldDire;

    //----------------------------------------------------------------------------------------------------------
    // 外部取得

    [Header("外部取得")]
    // 入力系
    [SerializeField] private PlayerInputManager _playerInputManager; // 同シーン内のオブジェクトのためpublic
    [SerializeField] private InputTrigger _trigger; // トリガーを得る

    // 座標系
    [SerializeField] private Transform _thisTransform;  //自身の座標
    [SerializeField] private Rigidbody2D _thisRigidbody2d; // リジッドボディー

    // 接地判定系
    [SerializeField] private GroundCheck ground; // 接地判定用のスクリプトを取得する変数
    [SerializeField] private OverheadCheck overhead; // 接地判定用のスクリプトを取得する変数

    // ステータス系
    [SerializeField] private PlayerStatas _playerStatus; // プレイヤーのステータスが入っている

    // アニメーション系
    [SerializeField] private Animator anim;

    // サウンド系
    [SerializeField] private SEManager_Player seMana;

    // ひび系
    //-------------菅----------------
    //[SerializeField] private CrackAutoMove crackmove;

    // エフェクト系
    //------------中川---------------
    [SerializeField] private RunDustParticle _runDust;
    //-------------------------------

    // Start is called before the first frame update
    void Start()
    {
        // 入力系
        _trigger = _playerInputManager.GetComponent<InputTrigger>();
        oldDire = _playerInputManager.Direction;

        //--------------------------------------
        // 追加担当者:中川直登
        if (_runDust == null) Debug.LogError("RunDustParticleのコンポーネントを取得できませんでした。");
        //--------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        if (oldDire != _playerInputManager.Direction)
        {
            groundCheckOffsetX = -groundCheckOffsetX;
        }

        // 前フレームの向きとして保存
        oldDire = _playerInputManager.Direction;

        //--------------------------------------
        // 追加担当者:中川直登
        RunDust();

        //--------------------------------------
        // ヒットストップ中でないなら実行
        if (!_playerStatus.GetHitStop())
        {

            // 重力無効化
            _thisRigidbody2d.gravityScale = 0.0f;

            //--------------------------------------
            // 接地判定

            // 接地判定を得る
            isGround = IsGroundCircle();
            // 天井の衝突判定を得る
            isOverhead = overhead.IsOverHead();

            //-----------------------------------------
            // 入力取得

            // トリガー
            TriggerJumpflg = _trigger.GetJumpTrigger(); // 押された瞬間か
            // プレス
            PressJumpflg = _playerInputManager.GetJump(); // 押されているか

            if (ReleaseJumpflg == false)
            {
                // リリース
                ReleaseJumpflg = OldPressJumpflg == true && PressJumpflg == false; // 離された瞬間か
            }

            //----------------------------------------------------
            // ジャンプボタンが押された瞬間

            // 地面にいるときに
            if (isGround == true)
            {
                // ジャンプ入力がされたら
                if (TriggerJumpflg == true)
                {
                    // 私は飛ぶ
                    ImFly = true;

                    // 1フレームのプレイヤーの移動量セット
                    MoveY = JumpPower;

                    // ジャンプse再生
                    seMana.PlaySE_Jump();

                    count = 0;

                    //Debug.Log("トリガー");
                }
            }

            // トロッコに乗りながらジャンプ
            if (RideOn == true && TriggerJumpflg == true)
            {
                // 私は飛ぶ
                ImFly = true;

                // 1フレームのプレイヤーの移動量セット
                MoveY = JumpPower;
                if (RideOn == true)
                {
                    MoveY = 1.2f * JumpPower;
                }

                // ジャンプse再生
                seMana.PlaySE_Jump();

                count = 0;
            }

            // 崖から落ちたら
            if (ImFly == false && isGround == false && ImDrop == false)
            {
                // 私は落ちる
                ImDrop = true;

                // 落下していくだけ
                MoveY = 0f;
            }

            // ジャンプボタンが押されていたら
            if (ImFly == true || ImDrop == true)
            {
                // プレイヤーY座標変化
                _thisTransform.Translate(0f, MoveY * Time.deltaTime, 0f);

                // 重力の影響を受けさせる
                MoveY -= gravity * Time.deltaTime;
            }

            if (isGround == false && PressJumpflg == false)
            {
                // 落下

                // ボタンが離された時のy移動量によって慣性を働かせる
                if (MoveY > JumpPower / 100f * inertia)
                {
                    // 慣性
                    MoveY = JumpPower / 100f * inertia;
                }
            }

            // 天井に当たったら
            if(MoveY > 0f && isOverhead == true && ImFly == true)
            {
                // 落下開始
                MoveY = 0f;
            }

            // ジャンプの処理が終わって地面についたとき(ジャンプが入力されたフレームでは入らない)
            if (isGround == true && ImFly == true && (TriggerJumpflg == false))
            {                                                                  
                // 一連のジャンプ終了                                          
                ImFly = false;                                                 
                MoveY = 0f;
                ReleaseJumpflg = false;
                //Debug.Log(count);                                            
            }                                                                  
                                                                               
            // ジャンプを経由しない落下が終わる                                
            if (isGround == true && ImDrop == true)                            
            {                                                                  
                // 落下終了                                                    
                ImDrop = false;                                                
                MoveY = 0f;                                                    
            }
            
            if(RideOn == true && ImDrop == true)
            {
                ImDrop = false;
                MoveY = 0f;
            }
                                                                               
            // バグ回避用 だけどできないいいいいいいいいいいいいいいいいいい
            //count++;

            //if (TriggerJumpflg == true)
            //{
            //    Debug.Log("-----------------------------------------------------------------------------------");
            //}
            //if (count < 30/*MoveY != 0*/)
            //{
            //    Debug.Log(count);
            //    //Debug.Log(MoveY);
            //}

            //-------------------------------------------------------------------
            // アニメーション関係
            // 上昇アニメーション
            anim.SetBool("jump", MoveY > 0f);
            // 落下アニメーション
            anim.SetBool("drop", MoveY < 0f);

            // 前フレームの押下状況を保持
            OldPressJumpflg = PressJumpflg;

            // 重力もどす
            _thisRigidbody2d.gravityScale = 1.0f;

            //Debug.Log("ヒットストップ中でない");
        }
        else
        {
            // ヒットストップ中も重力0
            if (_thisRigidbody2d.gravityScale == 1.0f)
            {
                _thisRigidbody2d.gravityScale = 0.0f;
            }

            //Debug.Log("ヒットストップ中");

            //Debug.Log(_thisRigidbody2d.gravityScale);
        }
    }

    private void RunDust()
    {
        _runDust.IsJump = !isGround;
    }

    // 円形のレイキャスト
    private bool IsGroundCircle()
    {
        // 戻り値用変数
        bool Return = false;

        RaycastHit2D hit = Physics2D.CircleCast((Vector2)_thisTransform.position + groundCheckOffsetX * Vector2.right + groundCheckOffsetY * Vector2.up, groundCheckRadius, Vector2.down, groundCheckDistance, layerMask);

        if (hit)
        {
            Return = true;
        }

        //Debug.Log(Return);

        return Return;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)_thisTransform.position + groundCheckOffsetX * Vector2.right + groundCheckOffsetY * Vector2.up, groundCheckRadius);
    }
}
