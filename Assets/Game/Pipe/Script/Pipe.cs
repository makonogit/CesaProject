//------------------------------------------------------------------------------
// �S���ҁF�����V�S
// ���e  �F�Ђт��������ꏊ�ɃI�u�W�F�N�g�𐶐�����M�~�b�N
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    //--------------------------------------------------------------------------
    // - �ϐ��錾 -

    [Header("�Ђт��琶������I�u�W�F�N�g")]
    public GameObject makeObj;     // ��������I�u�W�F�N�g
    [Header("�Ђт𐶐�����I�u�W�F�N�g")]
    public CrackOrder crackOrder;  // �Ђт𐶐�����I�u�W�F�N�g

    // �ЂтƂ̓����蔻��֘A
    Vector2[] Point;               //��������edgecollider��Point���W
    EdgeCollider2D Edge;           //��������EdgeCollider

    //============================================================
    // - �Փˏ��� -

    void OnTriggerEnter2D(Collider2D collision)
    {
        //--------------------------------------------------------
        // �ЂтƂԂ������ꍇ�̏���

        if (collision.gameObject.tag == "Crack")
        {
            //----------------------------------------------------
            // �Ђт������Ă���ꏊ�ɃI�u�W�F�N�g�𐶐�����

            // EdgeCollider�̏����擾
            Edge = collision.gameObject.GetComponent<EdgeCollider2D>();

            //���̃I�u�W�F�N�g��SpriteRenderer���擾
            SpriteRenderer thisSpriteRenderer = this.GetComponent<SpriteRenderer>();

            for (int i = 0; i < (int)crackOrder.numSummon; i++)
            {
                // �Ђт����̃I�u�W�F�N�g���ɑ��݂��邩�𔻒� 
                if (Edge.points[i].y < this.transform.position.y + thisSpriteRenderer.bounds.size.y / 2
                     && Edge.points[i].y > this.transform.position.y - thisSpriteRenderer.bounds.size.y / 2
                     && Edge.points[i].x < this.transform.position.x + thisSpriteRenderer.bounds.size.x / 2
                     && Edge.points[i].x > this.transform.position.x - thisSpriteRenderer.bounds.size.x / 2)
                {

                    // �Ђт������Ă���ꏊ�ɃI�u�W�F�N�g�𐶐�
                    Vector3 EdgePos = Edge.transform.position;
                    SpriteRenderer crackSr = crackOrder.PrefabObject.GetComponent<SpriteRenderer>();
                    EdgePos = Vector3.MoveTowards(Edge.transform.position, Edge.points[i], crackSr.bounds.size.y * i);
                    GameObject obj = Instantiate(makeObj, EdgePos, Quaternion.identity);

                }
            }
            //----------------------------------------------------
        }
        //--------------------------------------------------------

    }
    //============================================================

}
