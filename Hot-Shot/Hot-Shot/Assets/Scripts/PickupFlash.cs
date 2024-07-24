using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickupFlash : MonoBehaviour
{
    public Image flashImage;   // Reference to the Image component
    public float flashDuration = 0.2f;   // Duration of the flash

    void Start()
    {
        // Ensure the image is initially transparent
        SetImageAlpha(0f);
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Set the image to fully opaque
        SetImageAlpha(1f);
        yield return new WaitForSeconds(flashDuration);
        // Fade the image back to transparent
        SetImageAlpha(0f);
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = flashImage.color;
        color.a = alpha;
        flashImage.color = color;
    }
}

