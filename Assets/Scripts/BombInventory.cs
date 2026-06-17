using UnityEngine;

public class BombInventory : MonoBehaviour
{
    public static BombInventory Instance { get; private set; }

    [Header("Bombalar")]
    public BombData[] bombs; // Inspector'da 7 bombayý sürükle býrak

    private int selectedIndex = 0;

    public BombData SelectedBomb =>
        bombs != null && bombs.Length > 0 ? bombs[selectedIndex] : null;

    public int SelectedIndex => selectedIndex;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        HandleScrollSelection();
        HandleNumberKeySelection();
    }

    private void HandleScrollSelection()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f) SelectBomb(selectedIndex - 1);
        else if (scroll < 0f) SelectBomb(selectedIndex + 1);
    }

    private void HandleNumberKeySelection()
    {
        // 1-7 tuþlarýyla seçim
        for (int i = 0; i < Mathf.Min(bombs.Length, 9); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectBomb(i);
        }
    }

    public void SelectBomb(int index)
    {
        if (bombs == null || bombs.Length == 0) return;

        // Döngüsel seçim (sona gelince baþa döner)
        selectedIndex = (index % bombs.Length + bombs.Length) % bombs.Length;

        Debug.Log($"Seçili Bomba: {SelectedBomb.bombName}");
    }
}