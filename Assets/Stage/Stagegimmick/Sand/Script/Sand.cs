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
    private Transform thistrans;
    private float MaxScale;
    public bool SandHit = false;   //�����������Ă��邩   
    [SerializeField]private float Line = 0.0f;
    
    private PolygonCollider2D thiscoll; //���̃I�u�W�F�N�g��collider
    private float MaxSand;

    private float Wait = 0.0f; //���Ԍv���p

    [SerializeField, Header("����X�s�[�h")]
    private float accumulatespeed;


    // Start is called before the first frame update
    void Start()
    {
        thismat = GetComponent<SpriteRenderer>();
        thiscoll = GetComponent<PolygonCollider2D>();
        thistrans = transform;
        MaxScale = thistrans.localScale.y;  //�c�̒������擾
        MaxSand = thiscoll.points[0].y;
    }

    // Update is called once per frame
    void Update()
    {
        //�����������Ă����珙�X�Ɍ�����悤�ɂ���
        if (SandHit)
        {
            {
                //if (Line< 1.0f)
                //{
                //    Line += accumulatespeed * Time.deltaTime;
                //    Vector2[] points = thiscoll.points;

                //    if (points[0].y < MaxSand)
                //    {
                //        points[0].y += Line;
                //        points[1].y += Line;
                //    }

                //    thiscoll.SetPath(0, points);
                //}
                //else
                //{
                //    gameObject.layer = 10;
                //}
            }

            if(thistrans.localScale.y < MaxScale)
            {
                thistrans.localScale = new Vector3(thistrans.localScale.x, thistrans.localScale.y + accumulatespeed * Time.deltaTime, thistrans.localScale.z);
                thistrans.localPosition = new Vector3(thistrans.localPosition.x, thistrans.localPosition.y + (accumulatespeed / 2) * Time.deltaTime, thistrans.localPosition.z);

                //// �R���C�_�[�̐ݒ�
                //Vector2[] points = thiscoll.points;

                //if (points[0].y < MaxSand)
                //{
                //    points[0].y += accumulatespeed * Time.deltaTime;
                //    points[1].y += accumulatespeed * Time.deltaTime;
                //}

                //thiscoll.SetPath(0, points);

            }
            else
            {
                gameObject.layer = 10;
            }

        }
        else
        {
            {
                //if (Line > 0.0f)
                //{
                //    Line -= accumulatespeed * Time.deltaTime;
                //    Vector2[] points = thiscoll.points;
                //    if (points[0].y > points[2].y)
                //    {
                //        points[0].y -= Line;
                //        points[1].y -= Line;
                //    }
                //    thiscoll.SetPath(0, points);

                //}
                //else
                //{
                //    gameObject.layer = 15;
                //}
            }

            if (thistrans.localScale.y > 0.1f)
            {
                thistrans.localScale = new Vector3(thistrans.localScale.x, thistrans.localScale.y - accumulatespeed * Time.deltaTime, thistrans.localScale.z);
                thistrans.localPosition = new Vector3(thistrans.localPosition.x, thistrans.localPosition.y - (accumulatespeed / 2) * Time.deltaTime, thistrans.localPosition.z);

                //// �R���C�_�[�̐ݒ�
                //Vector2[] points = thiscoll.points;

                //if (points[0].y > points[2].y + 0.1f)
                //{
                //    points[0].y -= (accumulatespeed / 2) * Time.deltaTime;
                //    points[1].y -= (accumulatespeed / 2) * Time.deltaTime;
                //}

                //thiscoll.SetPath(0, points);

            }
            else
            {
                gameObject.layer = 15;
            }

        }

        // 0.5�b�҂��ē����蔻��𖳌���
        if (SandHit)
        {
            Wait += Time.deltaTime;
            if (Wait > 0.3f)
            {
                SandHit = false;
                Wait = 0.0f;
            }
        }
        else
        {
            SandHit = false;
        }

        //thismat.material.SetFloat("_Border", Line);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�����������Ă�����
        if (collision.gameObject.tag == "Sand" && !SandHit)
        {
            SandHit = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //����������Ȃ��Ȃ�����
        if (collision.gameObject.tag == "Sand")
        {
            Debug.Log("������Ȃ�����");
            SandHit = false;
        }
    }


}