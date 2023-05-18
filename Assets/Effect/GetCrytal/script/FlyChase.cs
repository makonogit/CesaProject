//---------------------------------------
//担当者：中川直登
//内容　：クリスタルを取得した時のエフェクト管理
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlyChase : MonoBehaviour
{
    //------------------------------------------------------------------------
    private enum State 
    {
        NULL,
        MOVE,
        D,
    }
    [SerializeField]
    private State _state = State.NULL;
    [SerializeField]
    private Transform _endPos;
    private Transform _trans;
    [SerializeField]
    private float _endDis;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    HaveCrystal haveCrystal;

    //------------------------------------------------------------------------

    //
    // 関数：SetStart()
    //
    // 内容：呼ぶと動き始める※念のため引数にTransform
    //
    public void SetStart(Transform _end,HaveCrystal have)
    {
        _endPos = _end;
        haveCrystal = have;
    }

    //------------------------------------------------------------------------
    // Use this for initialization

    //
    // 関数：Start()
    //
    void Start()
    {
        _trans = GetComponent<Transform>();
        if (_trans == null) Debug.LogError("Transformのコンポーネントを取得できませんでした。");
        if(_state == State.NULL) _state = State.D;
    }

    // Update is called once per frame

    //
    // 関数：Update()
    //
    void Update()
    {
        MoveSystem();
        End();
    }

    //
    // 関数：End()
    //
    // 内容：終了処理。目的地まで向かうと自身を消す
    //
    private void End()
    {
        Vector3 Distans = _trans.position - _endPos.position;
        if (Distans.magnitude < _endDis ) { Destroy(this.gameObject); }
    }

    //
    // 関数：MoveSystem()
    //
    // 内容：動き方を選ぶ処理
    //
    private void MoveSystem()
    {
        switch (_state) 
        {
            case State.MOVE:
                Move();
                break;
        }
        
    }

    //
    // 関数：Move()
    //
    // 内容：基本的な動きの処理
    //
    private void Move()
    {
        Vector3 rotate = transform.rotation.eulerAngles;
        rotate = new Vector3(rotate.x, rotate.y, rotate.z + rotateSpeed * Time.deltaTime);
        transform.Rotate(rotate);
        
        speed += acceleration * Time.deltaTime;
        Vector3 _moveVec = _endPos.position - _trans.position;
        transform.position += _moveVec.normalized * speed * Time.deltaTime;
    }

    private void OnDestroy()
    {
        haveCrystal.AnimationGetCrystal();
    }

}