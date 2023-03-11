using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMove : MonoBehaviour
{
   

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //  ÉLÅ[ì¸óÕ
        if (Input.GetKeyDown("joystick button 0"))
        {
            
            SceneManager.LoadScene("TestScene");
        }

    }
}
