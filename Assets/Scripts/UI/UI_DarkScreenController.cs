using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DarkScreenController : MonoBehaviour
{

    public static UI_DarkScreenController instance;

    Image image;
    Animator anim;
    public bool darkScreenOn = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        image = GetComponent<Image>();
    }

    void Update()
    {
        anim.SetBool("darkScreenOn", darkScreenOn);
    }

    public void RaycastTargetIsActiveFalse()
    {
        image.raycastTarget = false;
    }
}
