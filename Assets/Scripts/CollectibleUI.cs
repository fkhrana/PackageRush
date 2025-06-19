using UnityEngine;
using UnityEngine.UI;

public class CollectibleUI : MonoBehaviour
{
    [SerializeField] private Image[] itemIcons;
    [SerializeField] private ItemData[] keyItems; 
    [SerializeField] private Color fadedColor = new Color(1f, 1f, 1f, 0.4f);
    [SerializeField] private Color normalColor = Color.white;

    private void Start()
    {
        if (keyItems.Length != itemIcons.Length)
        {
            Debug.LogError($"Mismatch between keyItems ({keyItems.Length}) and itemIcons ({itemIcons.Length})!");
        }

        for (int i = 0; i < itemIcons.Length; i++)
        {
            if (i < keyItems.Length && keyItems[i] != null)
            {
                itemIcons[i].sprite = keyItems[i].icon;
                itemIcons[i].color = fadedColor;
            }
            else
            {
                itemIcons[i].sprite = null;
                itemIcons[i].color = fadedColor;
                Debug.LogWarning($"keyItems[{i}] is null or out of range!");
            }
        }
    }

    public void UpdateIcon(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < itemIcons.Length && itemIndex < keyItems.Length)
        {
            if (keyItems[itemIndex] != null)
            {
                itemIcons[itemIndex].sprite = keyItems[itemIndex].icon;
                itemIcons[itemIndex].color = normalColor;
                Debug.Log($"Updated icon {itemIndex} ({keyItems[itemIndex].itemName}) to normal color");
            }
            else
            {
                Debug.LogError($"keyItems[{itemIndex}] is null!");
            }
        }
        else
        {
            Debug.LogError($"Invalid item index: {itemIndex}");
        }
    }

    public void ResetIcons()
    {
        for (int i = 0; i < itemIcons.Length; i++)
        {
            if (i < keyItems.Length && keyItems[i] != null)
            {
                itemIcons[i].sprite = keyItems[i].icon;
                itemIcons[i].color = fadedColor;
            }
            else
            {
                itemIcons[i].sprite = null;
                itemIcons[i].color = fadedColor;
            }
        }
        Debug.Log("All icons reset to faded");
    }
}