using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

//Tooltip za iteme iz loota (oprema, oruzje, potion): ime, vrsta, statsi i uputa
public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance;

    public RectTransform Panel;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI TypeText;
    public TextMeshProUGUI StatsText;
    public TextMeshProUGUI HintText;
    public Vector2 Offset = new Vector2(18, -18);

    void Awake()
    {
        Instance = this;
        Panel.gameObject.SetActive(false);
    }

    public static void Show(Inventory_LootItem item)
    {
        if (Instance == null || item == null) return;
        Instance.Fill(item);
        Instance.Panel.gameObject.SetActive(true);
        Instance.FollowMouse();
    }

    public static void Hide()
    {
        if (Instance == null) return;
        Instance.Panel.gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (Panel.gameObject.activeSelf) FollowMouse();
    }

    void FollowMouse()
    {
        if (Mouse.current == null) return;
        Vector2 mouse = Mouse.current.position.ReadValue();
        Canvas canvas = Panel.GetComponentInParent<Canvas>();
        float scale = canvas != null ? canvas.scaleFactor : 1;
        //ako bi izasao van ekrana okreni ga na drugu stranu misa
        Vector2 size = Panel.rect.size * scale;
        bool flipX = mouse.x + Offset.x * scale + size.x > Screen.width;
        bool flipY = mouse.y + Offset.y * scale - size.y < 0;
        Panel.pivot = new Vector2(flipX ? 1 : 0, flipY ? 0 : 1);
        Vector2 off = new Vector2(flipX ? -Offset.x : Offset.x, flipY ? -Offset.y : Offset.y) * scale;
        Panel.position = mouse + off;
    }

    void Fill(Inventory_LootItem item)
    {
        string stats = "";
        string type = "";
        Color nameColor = new Color(0.25f, 0.15f, 0.08f);
        string hint = "";
        string green = "<color=#2E6B1A>";

        if (item.ItemType == "gear" && item.Gear != null)
        {
            GearData g = item.Gear;
            type = SlotName(g.Slot);
            if (g.Rarity == "magic") nameColor = new Color(0.15f, 0.3f, 0.75f);
            if (g.Rarity == "rare") nameColor = new Color(0.78f, 0.45f, 0.02f);
            if (g.Armour != 0) stats += green + "+" + g.Armour + " Armour</color>\n";
            if (g.MaxHealth != 0) stats += green + "+" + g.MaxHealth + " Max HP</color>\n";
            if (g.Damage != 0) stats += green + "+" + g.Damage + " Damage</color>\n";
            if (g.Speed != 0) stats += green + "+" + g.Speed + " Speed</color>\n";
            if (!string.IsNullOrEmpty(g.Description)) stats += "<i>" + g.Description + "</i>\n";
            hint = item.IsEquipped() ? "Right click: Unequip" : "Right click: Equip";
        }
        else if (item.ItemType == "weapon" && item.Weapon != null)
        {
            WeaponData w = item.Weapon;
            type = "Weapon - " + w.WeaponType;
            nameColor = new Color(0.55f, 0.12f, 0.06f);
            stats += green + "+" + w.Damage + " Damage</color>\n";
            stats += "Attack speed " + w.AttackSpeed.ToString("0.0") + "x\n";
            stats += "Reach " + w.LightRange.ToString("0.0") + " m\n";
            stats += "Whirlwind (Q): x" + w.HeavyDamageMultiplier + " damage, " + w.HeavyRadius.ToString("0.0") + " m\n";
            hint = item.IsEquipped() ? "Right click: Unequip" : "Right click: Equip";
        }
        else if (item.ItemType == "potion")
        {
            type = "Potion";
            nameColor = new Color(0.6f, 0.1f, 0.3f);
            stats += green + "Restores " + item.HealAmount + " HP</color>\n";
            stats += "Amount: " + item.Amount + "\n";
            hint = "Right click: Drink";
        }

        NameText.text = item.ItemName + (item.IsEquipped() ? " <size=70%>(equipped)</size>" : "");
        NameText.color = nameColor;
        TypeText.text = type;
        StatsText.text = stats.TrimEnd('\n');
        HintText.text = hint;
    }

    static string SlotName(string slot)
    {
        if (slot == "head") return "Helmet";
        if (slot == "armour") return "Body Armour";
        if (slot == "gloves") return "Gloves";
        if (slot == "boots") return "Boots";
        if (slot == "ring") return "Ring";
        if (slot == "amulet") return "Amulet";
        return slot;
    }
}
