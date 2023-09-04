using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class longbuttonclick : MonoBehaviour
{
    public bool push = false;

    public bool dashtrg = false;
    public bool jumptrg = false;
    public player pl;

    private void Update()
    {
        if (push && pl && dashtrg) pl.PlayerDash();
        else if (push && pl && jumptrg) pl.PlayerJump();
    }
    public void PushDown()
    {
        push = true;
    }

    public void PushUp()
    {
        push = false;
    }
}
