using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject[] itemPrefabs;

    public int currentItem;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectItem(0);
        else {
            if (Input.GetKeyDown(KeyCode.Alpha2)) SelectItem(1);
            else {
                if (Input.GetKeyDown(KeyCode.Alpha3)) SelectItem(2);
                else {
                    if (Input.GetKeyDown(KeyCode.Alpha4)) SelectItem(3); }
            }
        }
    }

    public void OnClick() {

    }

    public void SelectItem(int selectedItem) {
        if (currentItem != selectedItem) {
            currentItem = selectedItem;
            Debug.Log("Selection = " + currentItem.ToString());
        }
    }

    public GameObject CurrentlySelectedItem() {
        return itemPrefabs[currentItem];
    }
}
