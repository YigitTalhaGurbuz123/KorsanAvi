using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BombInventoryUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform slotContainer;   // Slotlarýn parent objesi
    public GameObject slotPrefab;     // Tek slot prefabý

    [Header("Görsel")]
    public Color selectedColor = new Color(1f, 0.8f, 0f);   // Seçili: altýn
    public Color normalColor = new Color(0.2f, 0.2f, 0.2f); // Normal: koyu

    private BombInventory inventory;
    private GameObject[] slots;
    private Image[] slotImages;
    private Image[] bombIcons;
    private TextMeshProUGUI[] bombNames;

    private void Start()
    {
        inventory = BombInventory.Instance;
        if (inventory == null) return;

        BuildSlots();
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void BuildSlots()
    {
        int count = inventory.bombs.Length;
        slots = new GameObject[count];
        slotImages = new Image[count];
        bombIcons = new Image[count];
        bombNames = new TextMeshProUGUI[count];

        // Varolan slotlarý temizle
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        for (int i = 0; i < count; i++)
        {
            int index = i; // closure için
            GameObject slot = Instantiate(slotPrefab, slotContainer);
            slots[i] = slot;

            slotImages[i] = slot.GetComponent<Image>();

            // Icon (child Image)
            Image[] images = slot.GetComponentsInChildren<Image>();
            if (images.Length > 1) bombIcons[i] = images[1];

            // Ýsim (TextMeshPro)
            bombNames[i] = slot.GetComponentInChildren<TextMeshProUGUI>();

            // Týklama
            Button btn = slot.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() => inventory.SelectBomb(index));

            // Numara göster
            if (bombNames[i] != null)
                bombNames[i].text = inventory.bombs[i].bombName;

            // Ýkon
            if (bombIcons[i] != null && inventory.bombs[i].icon != null)
                bombIcons[i].sprite = inventory.bombs[i].icon;
        }
    }

    private void UpdateUI()
    {
        if (slots == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            bool isSelected = i == inventory.SelectedIndex;

            if (slotImages[i] != null)
                slotImages[i].color = isSelected ? selectedColor : normalColor;

            // Scale animasyonu
            float targetScale = isSelected ? 1.2f : 1f;
            slots[i].transform.localScale = Vector3.Lerp(
                slots[i].transform.localScale,
                Vector3.one * targetScale,
                Time.deltaTime * 10f
            );
        }
    }
}