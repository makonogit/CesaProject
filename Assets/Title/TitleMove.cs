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
        //  �L�[����
        if (Input.GetKeyDown("joystick button 0"))
        {
            
            SceneManager.LoadScene("TestScene");
        }

    }
}
