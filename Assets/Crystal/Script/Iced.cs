//---------------------------------------------------------
//’S“–ÒF“ñ‹{—å
//“à—e@F“B‚ğ‘Å‚Â‚ÆŠ„‚ê‚é•X‚Ìˆ—
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iced : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - •Ï”éŒ¾ -
    private string NailTag = "UsedNail";

    // “Š‚°‚½“B‚Æ•X‚Ì•”•ª‚ªÚG‚µ‚½‚ç•X‚ªŠ„‚ê‚é(Œ»’iŠK‚Å‚ÍÁ‚¦‚é)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == NailTag)
        {
            Material mat = GetComponent<SpriteRenderer>().material;

            mat.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
