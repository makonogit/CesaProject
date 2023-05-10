//---------------------------------------------------------
//担当者：二宮怜
//内容　：プレイヤーが壁に当たっているかを調べる
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    // 変数宣言
    private bool isLeft = false;
    private bool isRight = false;

    public float AdjustY = 0.03f; // 画像の空白部分を無視するための調整用変数
    public float AdjustX = 0.41f; // 画像の空白部分を無視するための調整用変数
    public float AdjustCenterX = 0.15f; // 中央ぞろえ用変数
    public float AdjustCenterY = 0.15f; // 中央ぞろえ用変数

    [SerializeField, Tooltip("レイの長さを調整します。")] private float _length = 0.01f;

    private LayerMask layerMask = 1 << 10 | 1 << 14 | 1 << 17 | 1 << 18 | 1 << 21 | 1 << 22;

    private Transform thisTransform;

    PlayerInputManager.DIRECTION oldDire; // 前フレームの向きを入れておくための変数
    public PlayerInputManager _playerInputManager;

    private void Start()
    {
        thisTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (oldDire != _playerInputManager.Direction)
        {
            AdjustCenterX = -AdjustCenterX;
        }

        // 前フレームの向きとして保存
        oldDire = _playerInputManager.Direction;
    }

    public bool LeftCheck()
    {
        //----------------------------------------------------------------------------------------------------------
        // レイキャストに必要な引数の準備

        // x:left y:bottom
        Vector2 origin_leftUp = new Vector2(thisTransform.position.x - AdjustX + AdjustCenterX,
            thisTransform.position.y + AdjustY + AdjustCenterY);

        // x: y;bottom
        Vector2 origin_leftDown = new Vector2(thisTransform.position.x - AdjustX + AdjustCenterX,
            thisTransform.position.y - AdjustY + AdjustCenterY);

        // 向き
        Vector2 direction = Vector2.left;
        // 長さ
        float length = _length;// 変更者：中川
        // 距離
        Vector2 distance = direction * length;

        //----------------------------------------------------------------------------------------------------------
        // レイ飛ばして何かとぶつかったら生成やめる
        // 上
        RaycastHit2D hit_up = Physics2D.Raycast(origin_leftUp, direction, length, layerMask); // 第三引数 レイ長さ 、第四引数 レイヤー -1は全てのレイヤー

        // 下
        RaycastHit2D hit_down = Physics2D.Raycast(origin_leftDown, direction, length, layerMask); // 第三引数 レイ長さ 、第四引数 レイヤー -1は全てのレイヤー

        //----------------------------------------------------------------------------------------------------------
        // レイを描画
        Debug.DrawRay(origin_leftUp, distance, Color.white);
        Debug.DrawRay(origin_leftDown, distance, Color.white);

        // 片方でも触れていたら
        if(hit_down || hit_up)
        {
            isLeft = true;
        }
        else
        {
            isLeft = false;
        }

        return isLeft;
    }

    public bool RightCheck()
    {
        //----------------------------------------------------------------------------------------------------------
        // レイキャストに必要な引数の準備

        // x:left y:bottom
        Vector2 origin_RightUp = new Vector2(thisTransform.position.x + AdjustX + AdjustCenterX,
            thisTransform.position.y + AdjustY + AdjustCenterY);

        // x: y;bottom
        Vector2 origin_RightDown = new Vector2(thisTransform.position.x + AdjustX + AdjustCenterX,
            thisTransform.position.y - AdjustY + AdjustCenterY);

        // 向き
        Vector2 direction = Vector2.right;
        // 長さ
        float length = _length;// 変更者：中川
        // 距離
        Vector2 distance = direction * length;

        //----------------------------------------------------------------------------------------------------------
        // レイ飛ばして何かとぶつかったら生成やめる
        // 上
        RaycastHit2D hit_up = Physics2D.Raycast(origin_RightUp, direction, length, layerMask); // 第三引数 レイ長さ 、第四引数 レイヤー -1は全てのレイヤー

        // 下
        RaycastHit2D hit_down = Physics2D.Raycast(origin_RightDown, direction, length, layerMask); // 第三引数 レイ長さ 、第四引数 レイヤー -1は全てのレイヤー

        //----------------------------------------------------------------------------------------------------------
        // レイを描画
        Debug.DrawRay(origin_RightUp, distance, Color.green);
        Debug.DrawRay(origin_RightDown, distance, Color.green);

        // 片方でも触れていたら
        if (hit_down || hit_up)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }

        return isRight;
    }
}
