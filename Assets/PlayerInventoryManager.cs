using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private ItemButton[] itemButtons;

    public int currentItem;

    public List<int> ammoCounts = new();

    [Header("Settings")]
    [SerializeField] private int maxAmmo;
    public int startingAmmo;

    private void Awake() {
        for (int i = 0; i < itemButtons.Length; i++) {
            ammoCounts.Add(startingAmmo);
            itemButtons[i].SetAmmo(startingAmmo, maxAmmo);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectItem(1);
        else {
            if (Input.GetKeyDown(KeyCode.Alpha2)) SelectItem(2);
            else {
                if (Input.GetKeyDown(KeyCode.Alpha3)) SelectItem(3);
                else {
                    if (Input.GetKeyDown(KeyCode.Alpha4)) SelectItem(4); }
            }
        }
    }

    public void OnClick() {

    }

    public void SelectItem(int selectedItem) {
        if (currentItem != selectedItem) {
            if (selectedItem > 0 && ammoCounts[selectedItem - 1] <= 0) return;
            if (currentItem > 0) {
                itemButtons[currentItem - 1].ToggleSlotSelection(false);
            }
            if (selectedItem > 0) itemButtons[selectedItem - 1].ToggleSlotSelection(true);
            currentItem = selectedItem;
        } else {
            itemButtons[currentItem - 1].ToggleSlotSelection(false);
            currentItem = 0;
        }
    }

    public GameObject CurrentlySelectedItem() {
        return itemPrefabs[currentItem];
    }

    public void UseAmmo() {
        if (currentItem > 0) {
            ammoCounts[currentItem - 1]--;
            itemButtons[currentItem - 1].SetAmmo(ammoCounts[currentItem - 1], maxAmmo);

            if (ammoCounts[currentItem - 1] == 0) {
                SelectItem(0);
            }
        }
    }
}
