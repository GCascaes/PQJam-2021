using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArturTest : MonoBehaviour
{
    [SerializeField] FlashController flash;
    public void Flash()
    {
        flash.Flash();
    }
}
