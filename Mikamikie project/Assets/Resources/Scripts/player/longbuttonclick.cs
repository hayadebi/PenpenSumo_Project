using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class longbuttonclick : MonoBehaviour
{
    public bool push = false;
    public void PushDown()
    {
        push = true;
    }

    public void PushUp()
    {
        push = false;
    }
}
