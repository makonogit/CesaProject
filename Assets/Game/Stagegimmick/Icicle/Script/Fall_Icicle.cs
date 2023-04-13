//-----------------------------
//担当：二宮怜
//内容：ひびと接触すると落下する
//-----------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall_Icicle : MonoBehaviour
{
    // 変数宣言

    // タグたち
    private string GroundTag = "Ground"; // 地面のタグ名
    private string CrackTag = "Crack"; // ひびオブジェクトのタグ名
    private string EnemyTag = "Enemy"; // 敵オブジェクトのタグ名
    private string IceTag = "Ice"; // 氷ブロックのタグ名

    // 落ちているか
    public bool isFall = false;
    // ひびがあたったか
    private bool CrackHit = false;
    // 振動したか
    private bool Vibration = false;

    private Rigidbody2D rigid2D; // rigidbody
    private Vector3 initTransform; // 初期座標

    // 外部取得
    private BreakBlock breakBlock = null;
    private VibrationObject vibration; 

    // Start is called before the first frame update
    void Start()
    {
        // rigidbody取得
        rigid2D = GetComponent<Rigidbody2D>();

        // 初期座標保存
        initTransform = transform.position;

        // 振動用スクリプト取得
        vibration = GetComponent<VibrationObject>();
    }

    private void Update()
    {
        // isFallがtrueかつ重力の値が0の時
        if (isFall && rigid2D.gravityScale == 0f)
        {
            rigid2D.gravityScale = 1.0f;
        }

        // 天井に張り付いている状態なら
        if(isFall == false && !Vibration)
        {
            // 座標固定
            transform.position = initTransform;
        }

        // ひびに当たったフラグが経っている
        if (CrackHit)
        {
            if (Vibration == false)
            {
                // 振動させる
                vibration.SetVibration(0.7f);
                CrackHit = false;
                Vibration = true;
            }
        }

        // 振動したなら
        if (Vibration)
        {
            if(vibration.GetVibration() == false)
            {
                // 落下
                rigid2D.gravityScale = 1.0f;
                isFall = true;
            }
        }

    }

    // ひびに当たる ひび(isTrriger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たったものがひびなら
        if (collision.gameObject.tag == CrackTag)
        {
            // ひびに接触したフラグ立てる
            CrackHit = true;

            // ひびを消去
            //Destroy(collision.gameObject);
        }
    }

    // 地面に当たる
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 落下状態の時に
        if (isFall)
        {
            // 地面にあたったら
            if (collision.gameObject.tag == GroundTag)
            {
                // 消滅
                Destroy(this.gameObject);
            }

            // 敵に当たる
            if(collision.gameObject.tag == EnemyTag)
            {
                // つらら消滅
                Destroy(this.gameObject);

                // 敵消滅
                Destroy(collision.gameObject);
            }

            // 氷に当たる
            if(collision.gameObject.tag == IceTag)
            {
                // つらら消滅
                Destroy(this.gameObject);

                // 接触したオブジェクトが持つ氷われるスクリプト取得
                breakBlock = collision.gameObject.GetComponent<BreakBlock>();

                // 氷われる
                breakBlock.Func_BreakBlock();
            }
        }
    }
}
