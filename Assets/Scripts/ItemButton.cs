using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Sprite normalSlotSprite;
    [SerializeField] private Sprite selectedSlotSprite;
    [SerializeField] private Image slotImage;
    [SerializeField] private TMP_Text ammoText;

    public void ToggleSlotSelection(bool isSelected) {
        slotImage.sprite = isSelected ? selectedSlotSprite : normalSlotSprite;
    }

    public void SetAmmo(float currentAmmo, float maxAmmo) {
        ammoText.text = $"{currentAmmo}/{maxAmmo}";
    }
}
