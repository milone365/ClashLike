using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeScreen : MonoBehaviour
{

    bool fadein = false;
    bool fadeout = false;
    float speed = 0.01f;
    float alpha = 1;
    Image screen;
    private void Start()
    {
        screen = GetComponent<Image>();
    }
    void Update()
    {
        
        if (fadein)
        {
            alpha += speed;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, alpha);
            if (alpha >= 1)
            {
                fadein = false;
            }
        }
        if (fadeout)
        {
            alpha -= speed;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, alpha);
            if (alpha <= 0)
            {
                fadeout = false;
            }
        }
    }
    public void fadeIn()
    {
        fadein = true;
    }
    public void fadeOut()
    {
        fadeout = true;
    }
    public void gameEnd()
    {
        fadeIn();
    }
}
