using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    //Controls
    public class playerControls
    {
        public KeyCode moveLeft = KeyCode.A;
        public KeyCode moveRight = KeyCode.D;
        public KeyCode jump = KeyCode.Space;
        public KeyCode use = KeyCode.E;
    }
    public playerControls player = new playerControls();
   
}
