//---------------------------------------------------------
//’S“–ÒF“ñ‹{—å
//“à—e@F¶¬’†‚Ì‚Ğ‚Ñ‚ğ“G‚É“–‚Ä‚½‚É“G‚ª€‚Ê
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrackHit : MonoBehaviour
{
    //---------------------------------------------------------
    // - •Ï”éŒ¾ -
    private string CrackTag = "Crack";

    // ŠO•”æ“¾
    private CrackCreater order = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        Debug.Log(collision.gameObject.tag);

        // “–‚½‚Á‚½‚à‚Ì‚ª‚Ğ‚Ñ‚È‚ç
        if (collision.gameObject.tag == CrackTag)
        {
            // “–‚½‚Á‚½‚Ğ‚Ñ‚ÌCrackOrder‚ğæ“¾
            order = collision.gameObject.GetComponent<CrackCreater>();

            //¶¬’†‚È‚ç
            if (order.State == CrackCreater.CrackCreaterState.CREATING)
            {
                // “G‚ğÁ‚·
                Destroy(this.gameObject.transform.parent.gameObject);
            }
        }  
    }
}
