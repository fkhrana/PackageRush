using UnityEngine;
using UnityEngine.UI;

public class CollectibleUI : MonoBehaviour
{
    [SerializeField] private Image[] itemIcons;
    [SerializeField] private Color fadedColor = new Color(1f, 1f, 1f, 0.4f);
    [SerializeField] private Color normalColor = Color.white;

    private void Start()
    {
        foreach (Image icon in itemIcons)
        {
            icon.color = fadedColor;
        }
    }

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

    public void ResetIcons()
    {
        foreach (Image icon in itemIcons)
        {
            icon.color = fadedColor;
        }
        Debug.Log("All icons reset to faded");
    }
}