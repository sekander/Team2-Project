using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuFade : MonoBehaviour
{
    public Image panelImage;           // Assign in Inspector: the UI Panel's Image
    public float frequency = 2f;       // How fast the panel pulses
    public float elapsedTime = 0.0f;   // Accumulated time since script start
    public bool pauseFading = false; // Flag to pause fading

    private bool quitQame = false; // Flag to quit game
    private bool loadGame = false; // Flag to load game

    public float duration = 2.0f; // Duration until event trigger
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = (Mathf.Cos(elapsedTime * frequency * Mathf.PI * 2f) + 1f) / 2f;
        Debug.Log($"Panel Image Color: {panelImage.color.a}");  
        Debug.Log($"Elapsed Time: {elapsedTime}");
        if(!pauseFading)
            elapsedTime += Time.deltaTime;
        
        if (panelImage != null)
        {
            Color color = panelImage.color;
            color.a = alpha;
            panelImage.color = color;

            // if (alpha <= 0.001f)
            if (!pauseFading)
            {
                if (elapsedTime >= duration)
                {
                    pauseFading = true;
                    // Debug.Log($"Fade Counter: {fadeCounter}");
                    // StartCoroutine(TriggerFade(_logoDuration));
                    // StopAllCoroutines();
                    panelImage.gameObject.SetActive(false);
                }
            }
            // if (alpha >= 0.99f && elapsedTime >= 0.5f)
            // {
            //     if(loadGame)
            //         SceneManager.LoadScene(2);

            //     if(quitQame)
            //         Application.Quit();
            //     // fadeCounter++;
            //     // Debug.Log($"Fade Counter: {fadeCounter}");
            // }
        }

    }

    public void LoadGame()
    {
        loadGame = true;
        pauseFading = false;
        // StartCoroutine(FadeOut(duration));
    }

    public void QuitGame()
    {
        quitQame = true;
        pauseFading = false;
        // StartCoroutine(FadeOut(duration));
    }

}
