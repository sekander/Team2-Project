using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    // [SerializeField] private int sceneBuildIndex = 1; // Set this in the Inspector
    // public Image panelImage;           // Assign in Inspector: the UI Panel's Image
    private bool triggerFade = false; // Flag to trigger fade
    private bool triggerLoad = false; // Flag to trigger load
    private bool triggerQuit = false; // Flag to trigger quit   

    [SerializeField] private Image panelImage; // Assign in Inspector: the UI Panel's Image
    [SerializeField] private GameObject loadingPanel; // Assign in Inspector: the loading panel GameObject



    void Update()
    {
        // Check if the fade trigger is set
        // if (triggerFade)
        // {
        //     Color color = panelImage.color;
        //     color.a = Mathf.Lerp(color.a, 1f, Time.deltaTime);

        // }

        // if(triggerLoad && panelImage.color.a >= 0.99f)
        // {
        //     // Load the scene here
        //     SceneManager.LoadScene(2); // Replace with your scene index or name
        // }
        // if(triggerQuit && panelImage.color.a >= 0.99f)
        // {
        //     // Load the scene here
        //     // SceneManager.LoadScene(1); // Replace with your scene index or name
        //     Application.Quit();
        // }
        // float alpha = (Mathf.Cos(elapsedTime * frequency * Mathf.PI * 2f) + 1f) / 2f;
        // Fade the panel alpha from 0 to 255 (0f to 1f in Unity)
        Debug.Log($"Panel Image Color: {panelImage.color.a}");
        if (triggerFade)
        {

            loadingPanel.GetComponent<MenuFade>().duration = 4.0f; // Ensure the MenuFade script is not paused
            loadingPanel.GetComponent<MenuFade>().pauseFading = false; // Ensure the MenuFade script is not paused


            if (loadingPanel.GetComponent<MenuFade>().elapsedTime >= 3.95f)
                SceneManager.LoadScene(2); // Load the next scene after 4 seconds
               
            // Color color = panelImage.color;
            // color.a += 0.005f;
            // // color.a = Mathf.MoveTowards(color.a, 0.05f, Time.deltaTime);
            // panelImage.color = color;
            // if (panelImage.color.a >= 0.99f)
            //     SceneManager.LoadScene(2);
        }

    }

    public void LoadSceneByIndex()
    {
        // triggerFade = true;
        // triggerLoad = true;
        // panelImage.enabled = true; // Ensure the panel is visible
        // loadingPanel.SetActive(true); // Ensure the loading panel is visible 

        panelImage.gameObject.SetActive(true); // Ensure the panel is visible
        triggerFade = true; // Set the fade trigger=
    }


    public void QuitGame()
    {
        // triggerFade = true;
        // triggerQuit = true;
        Debug.Log("Quit Game"); // Optional debug for editor
        Application.Quit();
    }


}
