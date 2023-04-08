//=========================================
// 担当：藤原昂祐
// 内容：洞窟のボスのプレイヤーを掴む攻撃
//=========================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripPlayer_CaveBoss : MonoBehaviour
{
    //=====================================
    // *** 変数宣言 ***
    //=====================================

    //-------------------------------------
    // *** インスタンス ***

    public static GripPlayer_CaveBoss instance;// このクラスのインスタンス

    //-------------------------------------
    // *** 状態関連 ***

    enum GripPlayerStateID // 状態ID
    {
        NULL,          // 状態なし
        ACCESS,        // 探索状態
        MOVE,          // 移動状態
        ATTACK,        // 攻撃状態
        GRIP,          // 捕まえた状態
        RETURN,        // 戻り状態
        END            // 行動終了
    }
    GripPlayerStateID oldGripPlayerState = GripPlayerStateID.NULL;  // 前の状態
    GripPlayerStateID nowGripPlayerState = GripPlayerStateID.ACCESS;// 現在の状態
    GripPlayerStateID nextGripPlayerState = GripPlayerStateID.NULL; // 次の状態

    //-------------------------------------
    // *** 攻撃関連 ***

   
    public GameObject rightHand;// 右手
    [Header("左手")]
    public GameObject leftHand; // 左手
    [Header("捕まえる時間")]
    public int gripTim = 1000;  // 拘束時間

    int gripTimCnt;             // 拘束時間をカウント
    Vector2 playerGripPos;      // プレイヤーを拘束する座標

    //-------------------------------------
    // *** 移動関連 ***

    [Header("手の移動速度")]
    public float moveSpeed = 0.01f;// 手の移動速度

    Vector2 startPos_R;            // 右手の初期位置
    Vector2 startPos_L;            // 左手の初期位置

    //-------------------------------------
    // *** 外部オブジェクト ***

    GameObject objPlayer;// プレイヤー

    //=====================================
    // *** 初期化処理 ***
    //=====================================

    void Start()
    {
        //--------------------------------
        // *** 変数の初期化 ***

        // このクラスのインスタンスを生成
        if (instance == null)
        {
            instance = this;
        }

        // プレイヤーのオブジェクトを取得
        objPlayer = GameObject.Find("player");
    }

    //=====================================
    // *** プレイヤーを掴む処理 ***
    //
    // 引数　：無し
    // 戻り値：攻撃が終了したか（true：終了、false：攻撃中）
    //=====================================

    public bool GripPlayer()
    {
        //---------------------------------------
        // *** 現在の状態によって処理を分岐 ***

        if (nextGripPlayerState != GripPlayerStateID.NULL)
        {
            oldGripPlayerState = nowGripPlayerState;
            nowGripPlayerState = nextGripPlayerState;
            nextGripPlayerState = GripPlayerStateID.NULL;
        }

        switch (nowGripPlayerState)
        {

            // 索敵状態
            case GripPlayerStateID.ACCESS:
                Access();
                break;
            // 移動状態
            case GripPlayerStateID.MOVE:
                Move();
                break;
            // 攻撃状態
            case GripPlayerStateID.ATTACK:
                Attack();
                break;
            // 捕まえた状態
            case GripPlayerStateID.GRIP:
                Grip();
                break;
            // 戻り状態
            case GripPlayerStateID.RETURN:
                Return();
                break;
            // 終了状態
            case GripPlayerStateID.END:
                nextGripPlayerState = GripPlayerStateID.ACCESS;
                return true;
                break;
        }

        return false;
    }

    //=====================================
    // *** 索敵処理 ***
    //=====================================

    void Access()
    {
        //-----------------------------------------
        // *** 移動範囲内を左右に反復して移動 ***

        Move_CaveBoss.instance.Move();

        //-----------------------------------------
        // *** Playerが真下に居るかをRayで判定 ***

        foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(transform.position, Vector2.down))
        {
            if (hit_view)
            {
                if (hit_view.collider.gameObject.CompareTag("Player"))
                {
                    // 手の初期位置を保存
                    startPos_R = rightHand.transform.position;
                    startPos_L = leftHand.transform.position;

                    // 手を移動状態に変更
                    nextGripPlayerState = GripPlayerStateID.MOVE;
                }
            }
        }
    }

    //=====================================
    // *** 移動処理 ***
    //=====================================

    void Move()
    {
        //--------------------------------------------------
        // *** プレイヤーの座標まで移動 ***

        Vector2 position_R = rightHand.transform.position;
        Vector2 position_L = leftHand.transform.position;

        position_R.x += moveSpeed * 0.1f;
        position_L.x -= moveSpeed * 0.1f;
        position_R.y -= moveSpeed;
        position_L.y -= moveSpeed;

        rightHand.transform.position = position_R;
        leftHand.transform.position = position_L;

        //--------------------------------------------------
        // *** プレイヤーの座標に着いたら攻撃状態にする ***

        if (objPlayer.transform.position.y > position_R.y)
        {
            nextGripPlayerState = GripPlayerStateID.ATTACK;
        }
    }

    //=====================================
    // *** 攻撃処理 ***
    //=====================================

    void Attack()
    {
        //--------------------------------------------------
        // *** 手をプレイヤーを挟むように移動する ***

        Vector2 position_R = rightHand.transform.position;
        Vector2 position_L = leftHand.transform.position;

        position_R.x -= moveSpeed;
        position_L.x += moveSpeed;

        rightHand.transform.position = position_R;
        leftHand.transform.position = position_L;

        //--------------------------------------------------
        // *** プレイヤーを捕まえたかをRayで判定 ***

        if ((objPlayer.transform.position.x < position_L.x)||(objPlayer.transform.position.x > position_R.x))
        {
            foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(leftHand.transform.position, Vector2.left, position_L.x - position_R.x))
            {
                if (hit_view)
                {
                    //---------------------------------------------
                    // 捕まえたらGRIP状態にする

                    if (hit_view.collider.gameObject.CompareTag("Player"))
                    {
                        nextGripPlayerState = GripPlayerStateID.GRIP;
                    }
                }
                else
                {
                    //---------------------------------------------
                    // 捕まえられなかったらRETURN状態にする

                    nextGripPlayerState = GripPlayerStateID.RETURN;
                }
            }
        }
    }

    //=====================================
    // *** 捕まえる処理 ***
    //=====================================

    void Grip()
    {
        //---------------------------------
        // *** プレイヤーを捕まえる ***

        // 捕まえている時間をカウント
        gripTimCnt++;

        // 捕まえた座標を保存
        if (gripTimCnt == 1)
        {
            playerGripPos = objPlayer.transform.position;
        }

        // 時間が経ったらRETURN状態にする
        if (gripTimCnt > gripTim)
        {
            nextGripPlayerState = GripPlayerStateID.RETURN;
            gripTimCnt = 0;
        }

        // 移動ステックが動いたらカウントを早める
        if(playerGripPos.x != objPlayer.transform.position.x)
        {
            gripTimCnt++;
        }

        // プレイヤーを保存した座標に移動
        objPlayer.transform.position = playerGripPos;
    }

    //=====================================
    // *** 元の位置に戻る処理 ***
    //=====================================

    void Return()
    {
        //---------------------------------------
        // *** 手を元の座標に移動 ***

        // 手の座標を取得
        Vector2 position_R = rightHand.transform.position;
        Vector2 position_L = leftHand.transform.position;

        // 右手
        if (position_R.x > startPos_R.x)
        {
            position_R.x -= moveSpeed;
        }
        if (position_R.x < startPos_R.x)
        {
            position_R.x += moveSpeed;
        }
        if (position_R.y < startPos_R.y)
        {
            position_R.y += moveSpeed;
        }

        // 左手
        if (position_L.x > startPos_L.x)
        {
            position_L.x -= moveSpeed;
        }
        if (position_L.x < startPos_L.x)
        {
            position_L.x += moveSpeed;
        }
        if (position_L.y < startPos_L.y)
        {
            position_L.y += moveSpeed;
        }

        //---------------------------------------
        // *** 移動が終わったらEND状態にする ***

        else
        {
            nextGripPlayerState = GripPlayerStateID.END;
        }

        // 座標の変更を適用
        rightHand.transform.position = position_R;
        leftHand.transform.position = position_L;
    }
}
