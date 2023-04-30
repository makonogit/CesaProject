//----------------------------------
//  �S���F�����S
//�@�����ςݏd�Ȃ��Ă���
//----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    //-------------------------------
    //�@�ϐ��錾
    private SpriteRenderer thismat;  //���̃I�u�W�F�N�g��Render
    public bool SandHit = false;   //�����������Ă��邩   
    private float Line = 0.0f;
    private PolygonCollider2D thiscoll; //���̃I�u�W�F�N�g��collider

    [SerializeField, Header("����X�s�[�h")]
    private float accumulatespeed;


    // Start is called before the first frame update
    void Start()
    {
        thismat = GetComponent<SpriteRenderer>();
        thiscoll = GetComponent<PolygonCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        //SandHit = false;
        //�����������Ă����珙�X�Ɍ�����悤�ɂ���
        if (SandHit)
        {
            if (Line <= 1.0f)
            {
                Line += accumulatespeed * Time.deltaTime;
                //Vector2[] points = thiscoll.points;
                //points[0].y += Line;
                //points[1].y += Line;
                //thiscoll.SetPath(0, points);
            }
            else
            {
                gameObject.layer = 10;
            }

        }
        else
        {
            if (Line > 0.0f)
            {
                Line -= accumulatespeed * Time.deltaTime;
                //Vector2[] points = thiscoll.points;
                //points[0].y -= Line;
                //points[1].y -= Line;
                //thiscoll.SetPath(0, points);

            }
        }

        

        thismat.material.SetFloat("_Border", Line);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�����������Ă�����
        if (collision.gameObject.tag == "Sand")
        {
            SandHit = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //����������Ȃ��Ȃ�����
        if (collision.gameObject.tag == "Sand")
        {
            SandHit = false;
        }
    }


}