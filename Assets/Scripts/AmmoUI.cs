using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public AmmoManager ammoManager; // Reference to the script that has ammoQueue
    public Transform ammoBarContainer; // The panel object
    public GameObject ammoIconPrefab; // The image prefab

    private List<GameObject> currentIcons = new List<GameObject>();

    private void Start()
    {
        RefreshAmmoUI();
    }

    public void RefreshAmmoUI()
    {
        // Clear old icons
        foreach (var icon in currentIcons)
            Destroy(icon);
        currentIcons.Clear();

        // Add current icons based on queue
        foreach (var ammo in ammoManager.AmmoQueue)
        {
            GameObject icon = Instantiate(ammoIconPrefab, ammoBarContainer);
            icon.GetComponent<Image>().color = GetColorFromAmmo(ammo.Color);
            currentIcons.Add(icon);
        }
    }

    private Color GetColorFromAmmo(AmmoColor color)
    {
        switch (color)
        {
            case AmmoColor.Blue: return Color.blue;
            case AmmoColor.Red: return Color.red;
            case AmmoColor.Yellow: return Color.yellow;
            default: return Color.white;
        }
    }
}
