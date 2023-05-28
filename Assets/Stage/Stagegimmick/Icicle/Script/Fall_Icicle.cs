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
    private string PlayerTag = "Player"; // プレイヤーのタグ名

    // 落ちているか
    public bool isFall = false;
    // ひびがあたったか
    private bool CrackHit = false;
    // 振動したか
    private bool Vibration = false;
    // 振動後のアニメーションが再生されたか
    private bool fallAnimFinish = false;

    [SerializeField] private Rigidbody2D _rigid2D; // rigidbody
    private Vector3 initTransform; // 初期座標

    // 外部取得
    private BreakBlock breakBlock = null;
    [SerializeField] private VibrationObject vibration;
    private GameObject player;
    private HitEnemy _hitEnemy;
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PolygonCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        // 初期座標保存
        initTransform = transform.position;

        // 振動用スクリプト取得
        vibration = GetComponent<VibrationObject>();

        // プレイヤー取得
        player = GameObject.Find("player");

        // プレイヤーにダメージを与えるため
        _hitEnemy = player.GetComponent<HitEnemy>();
    }

    private void Update()
    {
        // isFallがtrueかつ重力の値が0の時
        if (isFall && _rigid2D.gravityScale == 0f)
        {
            _rigid2D.gravityScale = 1.0f;
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
            // つらら分離アニメーションが終わっていれば
            if (fallAnimFinish == true)
            {
                // 振動が終わっていたら
                if (vibration.GetVibration() == false)
                {
                    // 落下
                    _rigid2D.gravityScale = 1.0f;
                    isFall = true;

                    // レイヤー変更
                    this.gameObject.layer = 11;

                }
            }else
                {
                    // つらら分離アニメーション再生開始
                    _anim.SetBool("FallIcicle", true);
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
                //// 消滅
                //Destroy(this.gameObject);

                // 割れるアニメーション
                _anim.SetBool("BreakIcicle", true);
                //Debug.Log("壊れる");
            }

            // 敵に当たる
            if(collision.gameObject.tag == EnemyTag)
            {
                //// つらら消滅
                //Destroy(this.gameObject);

                //// 割れるアニメーション
                //_anim.SetBool("BreakIcicle", true);

                // 敵消滅
                //Destroy(collision.gameObject);

                EnemyMove enemyMove = collision.gameObject.GetComponent<EnemyMove>();
                //Debug.Log(enemyMove);
                if (enemyMove != null)
                {
                    // 敵撃破状態にする
                    enemyMove.EnemyAI = EnemyMove.AIState.DEATH; // 撃破音、パーティクル、デストロイ
                }

            }

            // 氷に当たる
            if(collision.gameObject.tag == IceTag)
            {
                //// つらら消滅
                //Destroy(this.gameObject);

                // 割れるアニメーション
                //_anim.SetBool("BreakIcicle", true);

                // 接触したオブジェクトが持つ氷われるスクリプト取得
                breakBlock = collision.gameObject.GetComponent<BreakBlock>();

                // 氷われる
                breakBlock.Func_BreakBlock();
            }

            // プレイヤーに当たる
            if(collision.gameObject.tag == PlayerTag)
            {
                //// つらら消滅
                //Destroy(this.gameObject);

                // 割れるアニメーション
                _anim.SetBool("BreakIcicle", true);

                _hitEnemy.HitPlayer(transform);
            }
        }
    }

    private void AnimFinish()
    {
        fallAnimFinish = true;
    }
    private void Invisible()
    {
        // 不可視化
        _spriteRenderer.enabled = false;
        // 当たり判定無効化
        _collider.enabled = false;
    }
}
