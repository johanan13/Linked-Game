using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonFx : MonoBehaviour
{
    public AudioSource myFx;
    public AudioClip hoverFx;
    public AudioClip clickFx;
    public TextMeshProUGUI buttonText;
    public Color hoverColor = Color.yellow;

    private Color originalColor;


    private void Start()
    {
        // Store the original text color at runtime
        if (buttonText != null)
            originalColor = buttonText.color;
    }

    public void Hover()
    {
        if (buttonText != null)
            buttonText.color = hoverColor;
        myFx.PlayOneShot(hoverFx);
    }

    public void Click()
    {
        myFx.PlayOneShot(clickFx);
    }

    public void ResetColor()
    {
        // Revert the text color to the original color
        if (buttonText != null)
            buttonText.color = originalColor;
    }
}
