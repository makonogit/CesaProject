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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            CheeckGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
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
