//-------------------------------
//　担当：菅眞心
//　内容：砂漠のちび敵の移動
//-------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertEnemyMove : MonoBehaviour
{
    //-------------------------
    //　変数宣言

    //---------------------------
    // 外部取得
    GameObject Player;          // プレイヤーのオブジェクト
    Transform PlayerTrans;      // プレイヤーのTransform


    [SerializeField, Header("生成する針")]
    private GameObject Needle;
    [SerializeField, Header("プレイヤーを感知する距離")]
    private float PlayerDistance = 2.0f;
    [SerializeField, Header("力を溜める時間")]
    private float PowerMaxTime = 1.5f;


    private float PowerTime = 0.0f; // 溜め時間計測用

    Transform ThisTrans;        // 自身のTransform
    Animator ThisAnim;          // 自身のAnimator
    CircleCollider2D ThisCol;  // 自身のCollider

   
    public enum DesertEnemyState
    {
        NONE,       // 待機状態
        FACE,       // 顔を出す
        ATTACK,     // 攻撃状態
        ATTACKEND,  // 攻撃終了
        DATH,       // 倒れる
    }

    public DesertEnemyState EnemyState; //状態管理用変数

    public void Init()
    {
        EnemyState = DesertEnemyState.NONE;     //何もしていない状態に設定
    }

    // Start is called before the first frame update
    void Start()
    {
        //------------------------------------------------
        // Playerの情報を取得
        Player = GameObject.Find("player");
        if (Player == null) Debug.Log("Playerのオブジェクトを取得できませんでした");
        PlayerTrans = Player.transform;

        ThisTrans = transform;                          //自身のTransformを変数化
        ThisAnim = GetComponent<Animator>();            //自身のAnimatorを取得
        ThisCol = GetComponent<CircleCollider2D>();    // 自身のColliderを取得

    }

    // Update is called once per frame
    void Update()
    {

        //----------------------------------------
        //　プレイヤーと自身の距離を求める
        float Distance = Vector3.Magnitude(PlayerTrans.position - ThisTrans.position);
        
        //-----------------------------------------
        //　プレイヤーが感知する距離に来たら行動
        if(Distance < PlayerDistance)
        {
            //------------------------------------------
            //　まだ顔を出していなかったら
            if (EnemyState == DesertEnemyState.NONE)
            {
                OpenFace();     // 顔を出す
                EnemyState = DesertEnemyState.FACE;
            }

            //------------------------------------------
            // 顔を出していたら
            if (EnemyState == DesertEnemyState.FACE)
            {
                Debug.Log(EnemyState);
                PowerTime += Time.deltaTime;    // 時間計測

                //--------------------------------
                // 時間経過したら攻撃開始
                if (PowerTime > PowerMaxTime)
                {
                    ThisAnim.SetBool("Attack", true);

                    EnemyState = DesertEnemyState.ATTACK;
                    PowerTime = 0.0f;
                }
            }

            if(EnemyState == DesertEnemyState.ATTACK)
            {
                //Attack();
            }

        }


    }

    //-------------------------
    //　顔を出す処理
    private void OpenFace()
    {
        // 顔を出すアニメーションを再生
        ThisAnim.SetBool("OpenFace", true);
        //ThisCol.size = new Vector2(ThisCol.size.x, 10.0f);
        ThisCol.offset = new Vector2(ThisCol.offset.x,ThisCol.offset.y + 0.2f);
        ThisTrans.position = new Vector3(ThisTrans.position.x, ThisTrans.position.y + 0.07f);

    }

    //--------------------------------
    //　攻撃処理
    private void Attack()
    {
        //------------------------------------------------
        //　プレイヤーとの角度を求める
        Vector2 Distance = PlayerTrans.position - ThisTrans.position;
        float Radian = Mathf.Atan2(Distance.y, Distance.x);
        float Angle = Radian * Mathf.Rad2Deg;

        //　角度を正規化
        if(Angle < 0)
        {
            Angle += 360;
        }

        Vector3 CreatePos = new Vector3(ThisTrans.position.x + (0.5f * Mathf.Cos(Angle * (Mathf.PI / 180))), ThisTrans.position.y + (0.5f * Mathf.Sin(Angle * (Mathf.PI / 180))),0.0f);
        Instantiate(Needle, CreatePos, Quaternion.Euler(0, Angle, 0));

        ThisAnim.SetBool("Attack", false);
        EnemyState = DesertEnemyState.FACE;

    }


    //　待ちアニメーションに遷移する関数
    public void Wait()
    {
        ThisAnim.SetBool("OpenFace", false);
        //ThisAnim.SetBool("Attack", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //　ひびに当たったら状態をDETHに
        if (collision.gameObject.tag == "Crack")
        {
            ThisAnim.SetBool("Deth", true);
            Destroy(collision.gameObject);
            GetComponent<CircleCollider2D>().enabled = false;
            EnemyState = DesertEnemyState.DATH;
        }
    }



}
