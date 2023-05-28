//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：スチームパーティクルを止めたり生成したり
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Steam : MonoBehaviour
{
    private ParticleSystem particle;

    private bool _active;
    private float _nowTime;
    [SerializeField,Header("アクティブ時間")]
    private float activeTime;
    [SerializeField, Header("クールタイム")]
    private float coolTime;

    // Use this for initialization
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        if (particle == null) Debug.LogError("ParticleSystemのコンポーネントを取得できませんでした。");
        _active = true;
        _nowTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(_active && activeTime <=_nowTime) 
        {
            particle.Stop();
            _active = false;
            _nowTime = 0.0f;
        }
        if (!_active && coolTime <= _nowTime)
        {
            particle.Play();
            _active = true;
            _nowTime = 0.0f;
        }
        _nowTime += Time.deltaTime;
    }
}