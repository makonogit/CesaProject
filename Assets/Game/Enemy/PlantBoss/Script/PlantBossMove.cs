//----------------------------
//　担当：菅眞心
//　内容：プラント場のボス行動
//----------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBossMove : MonoBehaviour
{

    //-------------------------------
    // 変数宣言

    public enum PlantBossMoveState
    {
        NONE,       // 何もしていない
        WALK,       // 歩く
        ATTACK,     // 攻撃
        DETH,       // 死亡
    }

    [SerializeField,Header("ボスの状態")]
    private PlantBossMoveState State;        // ボスの状態

    [SerializeField, Header("生成する敵のオブジェクト")]
    private GameObject PlantEnemy;

    //[SerializeField,Header("1回の攻撃で生成する敵の数")]
    private int CreateenemyNum = 0;

    [SerializeField, Header("生成する座標リスト")]
    private List<Vector2> CreatePosList; 

    [SerializeField, Header("生成した敵の座標リスト")]
    private List<Vector2> EnemyPos;

    [SerializeField,Header("攻撃間隔")]
    private float AttackTime;

    private float AttackTimeMesure = 0.0f;    //攻撃時間計測用

    private GameObject EnemyManager;          //生成した敵管理用
    private PolygonCollider2D BodyColl;       //体のコライダー

    //　画面端座標
    private Vector2 LeftDownPos;  //　左下
    private Vector2 RightUpPos;  //　右上

    //-----------------------------------
    //　体のパーツ用変数
    //private struct Body
    //{
    //    public GameObject Parts;   //　パーツ
    //    public Animator anim;      //　アニメーター
    //    public bool Conect;        //　接続されているか
    //}

    private GameObject LeftArm;     // 左手
    private GameObject RightArm;    // 右手
    private GameObject LeftFoot;    // 左足
    private GameObject RightFoot;   // 右足

    EdgeCollider2D WalkSpeace;  // 移動距離用EdgeCollider
    private float[] Limit = new float[2];
    private int WalkDirection;  // 歩く向き　0:左　1:右
    
    [SerializeField,Header("移動速度")]
    private float MoveSpeed;    // 移動速度

    private Animator anim;      //　Animator


    // Start is called before the first frame update
    void Start()
    {
        State = PlantBossMoveState.NONE; // 何もしていない状態から初期化

        //------------------------------------------
        //　体のパーツを取得

        LeftArm = transform.Find("LeftArm").gameObject;
        RightArm = transform.Find("RightArm").gameObject;
        LeftFoot = transform.Find("LeftKnees").gameObject;
        RightFoot = transform.Find("Rightknees").gameObject;

        BodyColl = GetComponent<PolygonCollider2D>();
        if(BodyColl == null)
        {
            Debug.Log("体の当たり判定がない");
        }

        //　Animatorを取得
        anim = GetComponent<Animator>();

        //　移動範囲を指定
        WalkSpeace = GetComponent<EdgeCollider2D>();    //　移動用Edgeを取得
        Limit[0] = WalkSpeace.points[0].x + transform.position.x;
        Limit[1] = WalkSpeace.points[1].x + transform.position.x;

        WalkDirection = Random.Range(0,1);              //　方向をランダムに初期化

        // 画面端の位置を設定する
        PolygonCollider2D CreateArea = transform.GetChild(transform.childCount - 1).GetComponent<PolygonCollider2D>();
        LeftDownPos = new Vector2(CreateArea.points[2].x + transform.position.x + 2.5f, CreateArea.points[2].y + 4.0f);
        RightUpPos = new Vector2(CreateArea.points[0].x + transform.position.x + 2.5f , CreateArea.points[0].y + 1.8f);

        //敵管理用オブジェクトを取得
        EnemyManager = GameObject.Find("CreateEnemyManager");
        
    }

    // Update is called once per frame
    void Update()
    {

        //----------------------------------
        //　状態によって処理する
        switch (State)
        {
            case PlantBossMoveState.NONE:
                break;
            case PlantBossMoveState.WALK:
                
                AttackTimeMesure += Time.deltaTime;
               
                Walk();     //　歩く
                
                if (AttackTimeMesure > AttackTime)
                {
                    CreateenemyNum = Random.Range(1, 6);

                    //一定時間経過したら攻撃開始
                    State = PlantBossMoveState.ATTACK;
                    AttackTimeMesure = 0.0f;

                    //敵が生成されていたら一回全て破棄してから生成
                    if(EnemyManager.transform.childCount > 0)
                    {
                        for (int i = 0; i < EnemyManager.transform.childCount; i++){
                            Destroy(EnemyManager.transform.GetChild(i).gameObject);
                        }
                    }

                }

                //　両足消えたらコライダーのサイズを変更
                if (LeftFoot == null && RightFoot == null)
                {
                    Vector2[] point = BodyColl.points;
                    point[2].y = -1.4f;
                    point[3].y = -1.4f;

                    BodyColl.SetPath(0, point);

                }

                break;
            case PlantBossMoveState.ATTACK:

                //　生成数に到達するまで敵を生成
                while (EnemyPos.Count < CreateenemyNum)
                {
                    CreateEnemy();
                }

                AttackTimeMesure += Time.deltaTime;

                if (AttackTimeMesure > AttackTime - 2)
                {
                    EnemyPos.Clear();                   //敵のリストを初期化
                    ////// 敵を抹消
                    ////for(int i = 0; i < EnemyManager.transform.childCount; i++)
                    ////{
                    ////    Destroy(EnemyManager.transform.GetChild(i).gameObject);
                    ////}
                    State = PlantBossMoveState.WALK;    //生成終了+一定時間経過したらしたら歩行に戻る
                }

                break;
            case PlantBossMoveState.DETH:

                //　リザルトに移行
                if (!transform.Find("core").GetComponent<BreakCore>())
                {
                    transform.Find("core").gameObject.layer = 15;
                    transform.Find("core").gameObject.AddComponent<BreakCore>();
                }

                break;
            default:
                Debug.Log("存在しない状態です");
                break;
        }

        //　全て破壊されたら死亡
        if (LeftArm.transform.childCount == 0 && RightArm.transform.childCount == 0 &&
            LeftFoot == null && RightFoot == null)
        {
            Destroy(BodyColl);  //体のコライダーを消去
            Destroy(LeftFoot);
            Destroy(RightFoot);
            transform.Find("core").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            transform.Find("Head").GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            State = PlantBossMoveState.DETH;
        }

        anim.SetBool("Walk", State == PlantBossMoveState.WALK);
        anim.SetBool("Attack", State == PlantBossMoveState.ATTACK);
        //LeftArm.anim.SetBool("LeftAttack", State == PlantBossMoveState.ATTACK);
        //RightArm.anim.SetBool("RightAttack", State == PlantBossMoveState.ATTACK);
    }

    //----------------------
    //　歩行関数
    //　引数：なし
    //　戻り値：なし
    private void Walk()
    {
        // 設定されている端まで移動する
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(Limit[WalkDirection], transform.position.y, transform.position.z), MoveSpeed * Time.deltaTime);
        
        //　移動範囲端との距離を求める
        float Distance = 
        Vector3.Magnitude(transform.position - new Vector3(Limit[WalkDirection], transform.position.y,transform.position.z));

        //　範囲を超えたら移動方向を変更する
        if (Distance <= 0.2f)
        {
            WalkDirection = WalkDirection == 0 ? 1 : 0;
        }

    }

    //----------------------------------
    //　敵を生成する関数
    //　引数：なし
    //　戻り値：なし
    private void CreateEnemy()
    {
        int CreatePos = Random.Range(0, CreatePosList.Count);   // 生成する座標を指定

        //　同じ座標が登録されていなければ座標追加
        if (!EnemyPos.Contains(CreatePosList[CreatePos]))
        {
            // 座標登録
            EnemyPos.Add(CreatePosList[CreatePos]);

            //　末尾の要素位置に生成
            GameObject obj = Instantiate(PlantEnemy, EnemyPos[EnemyPos.Count - 1], Quaternion.identity);
            obj.transform.parent = EnemyManager.transform;  //敵管理用オブジェクトの子オブジェクトに

        }

        {
            ////　生成する方向を決める　1:左 2:右 3:上 4:下
            //int CreateDirection = 1/*Random.Range(1, 4)*/;

            //float UpDownPos = (Random.RandomRange(LeftDownPos.y, RightUpPos.y));  //　0.3間隔で座標を指定

            //// 向きによって座標を指定
            //Vector2 VerticalPos = CreateDirection == 1 ? new Vector2(LeftDownPos.x, UpDownPos) :
            //    new Vector2(RightUpPos.x, UpDownPos);

            //if (!EnemyPos.Contains(VerticalPos))
            //{
            //    // 座標登録
            //    EnemyPos.Add(VerticalPos);

            //    //　末尾の要素位置に生成
            //    GameObject obj = Instantiate(PlantEnemy, EnemyPos[EnemyPos.Count - 1], Quaternion.identity);
            //    obj.transform.parent = EnemyManager.transform;  //敵管理用オブジェクトの子オブジェクトに
            //}
            //　生成する位置を決める
            //if(CreateDirection < 3)
            //{
            //    float UpDownPos = (Random.RandomRange(LeftDownPos.y, RightUpPos.y));  //　0.3間隔で座標を指定

            //    // 向きによって座標を指定
            //    Vector2 VerticalPos = CreateDirection == 1 ? new Vector2(LeftDownPos.x, UpDownPos) :
            //        new Vector2(RightUpPos.x, UpDownPos);

            //    //　同じ座標が登録されていなければ座標追加
            //    if (!EnemyPos.Contains(VerticalPos))
            //    {
            //        // 座標登録
            //        EnemyPos.Add(VerticalPos);

            //        //　末尾の要素位置に生成
            //        Instantiate(PlantEnemy, EnemyPos[EnemyPos.Count - 1], Quaternion.identity);
            //    }
            //}
            //else
            //{

            //    float LeftRightPos = (Random.RandomRange(LeftDownPos.x, RightUpPos.x) / 5.5f) * 5.5f;  //　0.6間隔で座標を指定

            //    //　向きによって座標を指定
            //    Vector2 HorizonPos = CreateDirection == 3 ? new Vector2(LeftRightPos, RightUpPos.y) :
            //        new Vector2(LeftRightPos, LeftDownPos.y);

            //    //　同じ座標が登録されていなければ座標追加
            //    if (!EnemyPos.Contains(HorizonPos))
            //    {
            //        //　座標登録
            //        EnemyPos.Add(HorizonPos);

            //        //　末尾の要素位置に生成
            //        Instantiate(PlantEnemy, EnemyPos[EnemyPos.Count - 1], Quaternion.identity);
            //    }

            //}
        }
    }

    //-----------------------------------------
    //　対戦開始関数
    //　引数：なし
    //　戻り値：なし
    public void BattleStart()
    {
        State = PlantBossMoveState.WALK;
    }
    
}
