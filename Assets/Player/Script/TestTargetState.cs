//------------------------------------
//担当：菅眞心
//内容：照準の状態管理(仮)
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTargetState : MonoBehaviour
{
    public bool CheeckGround = false;

    Transform thistrans;
    LayerMask layerMask = 1 << 10;

    Vector2 Movement;

    [SerializeField, Header("Hammer")]
    private Hammer hammer;  //ハンマーのスクリプト

    // Start is called before the first frame update
    void Start()
    {
        //thistrans = transform;
        //layerMask = 1 << 10;
        //layerMask = ~layerMask;

    }

    private void Update()
    {
        //RaycastHit2D hit = Physics2D.Raycast(thistrans.position, Vector2.down,0.3f,layerMask);

        //Debug.DrawRay(thistrans.position,Vector2.down * 0.3f,Color.red);

        //if (hit)
        //{
        //    Debug.Log(hit.collider.gameObject);
        //}

        //Debug.Log(hammer.GetInput());

        if (CheeckGround)
        {
            hammer.SetAngleLook(true);

            if(Movement.x < 0 && Movement.x > hammer.GetInput().x)
            {
                CheeckGround = false;
            }

        }
        else
        {
            hammer.SetAngleLook(false);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            Movement = hammer.GetInput();
            CheeckGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            Movement = hammer.GetInput();
            CheeckGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            CheeckGround = false;
        }
    }
}
