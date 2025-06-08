using UnityEngine;
using UnityEngine.UI;

public class CollectibleUI : MonoBehaviour
{
    [SerializeField] private Image[] itemIcons; // Array untuk 3 ikon di Canvas
    [SerializeField] private Color fadedColor = new Color(1f, 1f, 1f, 0.4f); // Warna pudar
    [SerializeField] private Color normalColor = Color.white; // Warna normal

    private void Start()
    {
        // Set semua ikon ke pudar di awal
        foreach (Image icon in itemIcons)
        {
            icon.color = fadedColor;
        }
    }

    // Dipanggil oleh GameManager saat item dikumpulkan
    public void UpdateIcon(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < itemIcons.Length)
        {
            itemIcons[itemIndex].color = normalColor;
            Debug.Log($"Updated icon {itemIndex} to normal color");
        }
        else
        {
            Debug.LogError($"Invalid item index: {itemIndex}");
        }
    }

    // Reset semua ikon ke pudar (dipanggil saat scene dimuat ulang)
    public void ResetIcons()
    {
        foreach (Image icon in itemIcons)
        {
            icon.color = fadedColor;
        }
        Debug.Log("All icons reset to faded");
    }
}