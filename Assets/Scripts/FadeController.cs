using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;
public class FadeController : MonoBehaviour
{
    public Image panelImage;           // Assign in Inspector: the UI Panel's Image
    public float frequency = 1f;       // How fast the panel pulses
    private float elapsedTime = 0.0f;   // Accumulated time since script start

    [SerializeField] private float _duration = 1f; // Duration until event trigger
    [SerializeField] private float _logoDuration = 5f; // Duration until event trigger

    private int fadeCounter = 0; // Counter for fade events

    private bool pauseFading = false; // Flag to pause fading

    void Update()
    {
        // Update elapsed time correctly using Time.deltaTime

        // Clamp alpha value to ensure it stays within 0–1 range
        // float alpha = Mathf.Clamp((Mathf.Sin(elapsedTime * frequency * Mathf.PI * 2f) + 1f) / 2f + 1.0f, 0f, 1f);
        // Calculate alpha value from a sine wave, normalize to 0–1
        //Start at 0
        // float alpha = (Mathf.Sin(elapsedTime * frequency * Mathf.PI * 2f - Mathf.PI / 2f) + 1f) / 2f;
        // float alpha = (Mathf.Sin(elapsedTime * frequency * Mathf.PI * 2f + Mathf.PI * 2) + 1f) / 2f;
        float alpha = (Mathf.Cos(elapsedTime * frequency * Mathf.PI * 2f) + 1f) / 2f;
        // if(alpha != 0.0f || !pauseFading)
        if(!pauseFading)
            elapsedTime += Time.deltaTime;
        

        // Apply alpha to panel's image color
        if (panelImage != null)
        {
            Color color = panelImage.color;
            color.a = alpha;
            panelImage.color = color;

            if (alpha >= 0.99f && elapsedTime >= 3.0f)
            {
                fadeCounter++;
                Debug.Log($"Fade Counter: {fadeCounter}");
            }
            if (alpha <= 0.01f)
            {
                pauseFading = true;
                // Debug.Log($"Fade Counter: {fadeCounter}");
                StartCoroutine(TriggerFade(_logoDuration));
                // StopAllCoroutines();
            }
            if (fadeCounter >= 1)
            {
        
                pauseFading = true;
                SceneManager.LoadScene(1);
                // StartCoroutine(LoadNextScene());
                // yield return new WaitForSeconds(_duration);
                // SceneManager.LoadScene(1);
        
        
        
                // this.enabled = false; // Disable this script
            }

        }

        // // Optional trigger when elapsedTime exceeds _duration
        // if (elapsedTime >= _duration)
        // {
        //     Debug.Log("Duration reached. Time to trigger something.");
        //     // Add your trigger logic here
        // }

        // Optional debugging
        Debug.Log($"Alpha: {alpha:F2}, Elapsed Time: {elapsedTime:F2}, Fade Counter: {fadeCounter}");
    }
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(_duration);
        SceneManager.LoadScene(1);
    }

    IEnumerator TriggerFade(float duration)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Trigger fade effect
        // fadeCounter++;
        Debug.Log("Triggering fade effect...");
        pauseFading = false; // Pause fading
        // Add your fade logic here
    }
}
