using UnityEngine;

//Oprema koja se moze equipat (kaciga, oklop, rukavice, cizme, prsten, ogrlica)
[CreateAssetMenu(fileName = "NewGear", menuName = "Game/Gear Data")]
public class GearData : ScriptableObject
{
    public string ItemName = "Item";
    public string Slot = "head"; // head, armour, gloves, boots, ring, amulet
    public Sprite Icon;
    public string Rarity = "common"; // common, magic, rare
    [TextArea] public string Description;

    [Header("Bonusi")]
    public int Armour;
    public int MaxHealth;
    public int Damage;
    public int Speed;
}
