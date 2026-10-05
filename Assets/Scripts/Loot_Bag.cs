using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public string Name;
    public string ItemType; // weapon, potion, ruple, stackable...
    public int Amount = 1;
    public GameObject InventoryItem; // prefab za inventory, ako je prazno radi se Inventory_LootItem
    public WeaponData Weapon;        // za ItemType = weapon
    public GearData Gear;            // za ItemType = gear
    public Sprite Icon;              // za potione i ostalo (oruzje uzima ikonu iz WeaponData)
    public int HealAmount = 30;      // za ItemType = potion
}

public class Loot_Bag : MonoBehaviour
{
    public List<LootItem> Items = new List<LootItem>();
    public bool DestroyWhenEmpty = true; // skrinja ostaje, vreca nestane

    private bool glowOff = false;

    //poklopac skrinje (samo za skrinju) - otvori se kad je prazna
    public Transform Lid;
    public float LidOpenAngle = -110f;
    public float LidOpenSpeed = 3f;
    private float lidT = 0;

    void Update()
    {
        if (Lid != null && Items.Count == 0 && lidT < 1)
        {
            lidT = Mathf.Min(1, lidT + Time.deltaTime * LidOpenSpeed);
            //malo odskoci na kraju
            float t = 1 - Mathf.Pow(1 - lidT, 3);
            Lid.localEulerAngles = new Vector3(LidOpenAngle * t, 0, 0);
        }
        if (Items.Count == 0 && DestroyWhenEmpty)
        {
            Destroy(gameObject);
        }
        //prazna skrinja ostaje ali vise ne svijetli
        if (Items.Count == 0 && DestroyWhenEmpty == false && glowOff == false)
        {
            glowOff = true;
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                if (r is ParticleSystemRenderer) continue;
                if (r.sharedMaterials.Length > 1)
                {
                    r.sharedMaterials = new Material[] { r.sharedMaterials[0] };
                }
            }
            foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
            {
                ps.Stop();
            }
        }
    }
}
