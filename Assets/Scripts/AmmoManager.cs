using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    private const int MAXAMMO = 30;
    // private Queue<Ammo> ammoQueue = new Queue<Ammo>(MAXAMMO);
    private Queue<Ammo> ammoQueue = new Queue<Ammo>(MAXAMMO);
    public Queue<Ammo> AmmoQueue => ammoQueue;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        ammoQueue.Clear();
        for (int i = 0; i < MAXAMMO; i++)
        {
            Ammo ammo;
            // Ammo ammo = new Ammo((AmmoColor)Random.Range(0, 3));
            ammo = new Ammo(i < 10 ? AmmoColor.Blue : i > 10 && i < 20 ? AmmoColor.Red : AmmoColor.Yellow);
            ammoQueue.Enqueue(ammo);
        }
    }

    public int CurrentAmmo => ammoQueue.Count;



    public AmmoColor? PeekNextAmmoColor()
    {
        if (ammoQueue.Count > 0)
            return ammoQueue.Peek().Color;
        return null;
    }

    private AmmoColor? TryConsumeAmmo()
    {
        if (ammoQueue.Count > 0)
        {
            return ammoQueue.Dequeue().Color;
        }
        return null;
    }

    public void AddAmmoBox(AmmoColor color)
    {
        int spaceLeft = MAXAMMO - ammoQueue.Count;
        int toAdd = Mathf.Min(10, spaceLeft);
        for (int i = 0; i < toAdd; i++)
        {
            ammoQueue.Enqueue(new Ammo(color));
        }
    }
    public int GetAmmoCountByColor(AmmoColor color)
    {
        int count = 0;
        foreach (var ammo in ammoQueue)
        {
            if (ammo.Color == color)
                count++;
        }
        return count;
    }
}
