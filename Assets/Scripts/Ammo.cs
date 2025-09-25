using UnityEngine;


public enum AmmoColor
{
    Red,
    Blue,
    Yellow,
}
    // This class is a placeholder for the Ammo component in Unity.
    // It currently does not contain any properties or methods.

public class Ammo : MonoBehaviour
{
    public AmmoColor Color { get; private set; }

    public Ammo(AmmoColor color)
    {
        Color = color;
    }   
}
