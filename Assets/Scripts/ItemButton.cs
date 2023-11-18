using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Sprite normalSlotSprite;
    [SerializeField] private Sprite selectedSlotSprite;
    [SerializeField] private Image slotImage;
    [SerializeField] private TMP_Text ammoText;

    [SerializeField] private AudioSource AudioSource_UI;
    [SerializeField] private AudioClip clic_ui;

    public void ToggleSlotSelection(bool isSelected) {
        slotImage.sprite = isSelected ? selectedSlotSprite : normalSlotSprite;
        
        AudioSource_UI.pitch = UnityEngine.Random.Range(.9f, 1.1f);
        AudioSource_UI.PlayOneShot(clic_ui);
        
    }

    public void SetAmmo(float currentAmmo, float maxAmmo) {
        ammoText.text = $"{currentAmmo}/{maxAmmo}";
    }
}
