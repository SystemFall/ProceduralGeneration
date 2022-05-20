using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    public Text display_Text;

    private int avgFrameRate;

    public void Update()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        display_Text.text = avgFrameRate.ToString() + " FPS";
    }
}
