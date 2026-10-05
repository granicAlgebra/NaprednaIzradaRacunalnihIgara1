using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;

public class Collect_Items : MonoBehaviour
{
    public Player_Inventory PlayerInventory;
    private GameObject _itemForPickup;
    public TMP_Text MessageText;
    public GameObject MessageContainer;

    public UI_Controler UiControler;
    public Loot_Window LootWindow;
    private GameObject _lootForPickup;
    private bool _timerStart = false;
    private int _timerTicks = 0;
    private void FixedUpdate()
    {   
        //Timer za prikazivanje poruke koja traje 5 sec
        if (_timerStart)
        {
            // 50 ticks = 1sec
            _timer(250);
        }
    }
    private void Update()
    {
        _pickupItem(_itemForPickup);

        //Otvori loot prozor na F kad je player kraj vrece
        if (_lootForPickup != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            LootWindow.Open(_lootForPickup.GetComponent<Loot_Bag>());
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        //Pokupi Heart i doda vrjednost
        if(collision.tag == "Heart")
        {
            if(Player_Controler.CurrentHealth < Player_Controler.MaxHealth)
            {
                Player_Controler.CurrentHealth += 20;
                if(Player_Controler.CurrentHealth > Player_Controler.MaxHealth)
                {
                    Player_Controler.CurrentHealth = Player_Controler.MaxHealth;
                }
                Debug.Log("Collected: " + collision.name);
            }
            UiControler.OnChangedHealth.Invoke();
            SFX.Play(SFX.Instance.DrinkPotion, 0.6f);
            Destroy(collision.gameObject);
        }
        //Pokupi Ruple i doda vrjednost
        if (collision.tag == "Ruple")
        {
            Player_Controler.RupleCurrency += collision.GetComponent<Ruple_Value>().RupleValue;
            UiControler.OnChangedRuples.Invoke();
            SFX.Play(SFX.Instance.Coins);
            Destroy(collision.gameObject);
            Debug.Log("Collected: " + collision.name);
        }
        //Itemi se vise ne kupe sa poda nego samo preko loot sistema
        //if (collision.tag == "Item")
        //{
        //    _itemForPickup = collision.gameObject;
        //}
        if (collision.tag == "Loot")
        {
            _lootForPickup = collision.gameObject;
        }
    }
   
    private void OnTriggerExit(Collider collision)
    {
        //Ako je igrac dalje od itema, nemoze ga vise pokupit
        if (collision.tag == "Item")
        {
            _itemForPickup = null;
        }
        if (collision.tag == "Loot")
        {
            _lootForPickup = null;
        }
    }
    //Pokupi item
    private void _pickupItem(GameObject item)
    {
        if(item != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {   
                //Provjerava dali postoji item u weapon slotu po njegovoj vrsti i dodaje ako je slot prazan, ako nije doda u prazan slot u inventoriju
                if(PlayerInventory.WeaponSlot.transform.childCount == 0 && item.GetComponent<ItemForInventory>().itemType == "weapon")
                {
                    _inportItem(PlayerInventory.WeaponSlot.transform, item);                   
                    return;
                }
                else if (PlayerInventory.ShieldSlot.transform.childCount == 0 && item.GetComponent<ItemForInventory>().itemType == "shield")
                {
                    _inportItem(PlayerInventory.ShieldSlot.transform, item);
                    return;
                }
                else if (PlayerInventory.BootsSlot.transform.childCount == 0 && item.GetComponent<ItemForInventory>().itemType == "boots")
                {
                    _inportItem(PlayerInventory.BootsSlot.transform, item);                  
                    return;
                }
                else if (PlayerInventory.ArmourSlot.transform.childCount == 0 && item.GetComponent<ItemForInventory>().itemType == "armour")
                {
                    _inportItem(PlayerInventory.ArmourSlot.transform, item);
                    return;
                }
                else if (PlayerInventory.GloveSlot.transform.childCount == 0 && item.GetComponent<ItemForInventory>().itemType == "glove")
                {
                    _inportItem(PlayerInventory.GloveSlot.transform, item);
                    return;
                }
                else if (PlayerInventory.BagSlot.transform.childCount == 0 && item.GetComponent<ItemForInventory>().itemType == "bag")
                {
                    _inportItem(PlayerInventory.BagSlot.transform, item);
                    return;
                }
                //Stackable itemi
                else if (item.GetComponent<ItemForInventory>().itemType == "stackable")
                {
                    bool hasSameItem = false;
                    //Provjerava postoji li vec stackable item u inventoriju

                    foreach (GameObject slot in Player_Inventory.Inventory)
                    {
                        try
                        {
                            //Ako je nasao item
                            if (slot.transform.GetChild(0).transform.tag == item.GetComponent<ItemForInventory>().inventoryItem.transform.tag)
                            {
                                //Ako item ima infinity stack - MaxStack treba bit 0
                                if(slot.GetComponentInChildren<Stack>().MaxStack == 0)
                                {
                                    slot.GetComponentInChildren<Stack>().StackAmout += item.GetComponent<ItemForInventory>().stack;
                                    slot.GetComponentInChildren<Stack>().OnValueChanged.Invoke();
                                    GameObject.Destroy(item);
                                    hasSameItem = true;
                                    break;
                                }
                                //Provjerava kolicinu stacka i dodaje ako ima mjesta, ako je ostalo u stacku za ostatak trazi dalje mjesta
                                else if (slot.GetComponentInChildren<Stack>().StackAmout <= slot.GetComponentInChildren<Stack>().MaxStack)
                                {
                                    slot.GetComponentInChildren<Stack>().StackAmout += item.GetComponent<ItemForInventory>().stack;
                                    if (slot.GetComponentInChildren<Stack>().StackAmout > slot.GetComponentInChildren<Stack>().MaxStack)
                                    {
                                        item.GetComponent<ItemForInventory>().stack = slot.GetComponentInChildren<Stack>().StackAmout - slot.GetComponentInChildren<Stack>().MaxStack;
                                        slot.GetComponentInChildren<Stack>().StackAmout = slot.GetComponentInChildren<Stack>().MaxStack;
                                        slot.GetComponentInChildren<Stack>().OnValueChanged.Invoke();
                                        continue;
                                    }
                                    else
                                    {
                                        slot.GetComponentInChildren<Stack>().OnValueChanged.Invoke();
                                        GameObject.Destroy(item);
                                        hasSameItem = true;
                                        break;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    //Ako nema doda novi na prazan slot
                    if (!hasSameItem)
                    {
                        _setItemInInventory(item);
                    }
                }
                //Svi ostali itemi npr Jabuka
                else
                {
                    _setItemInInventory(item);
                }               
            }
        }
    }
    private void _inportItem(Transform slot, GameObject item)
    {
        GameObject newItem = GameObject.Instantiate(item.GetComponent<ItemForInventory>().inventoryItem.gameObject, slot.transform);
        newItem.GetComponent<Equip_Trigger>().OnEquipedWhenCollected.Invoke();
        newItem.transform.localPosition = Vector2.zero;
        GameObject.Destroy(item);
    }
    private void _setItemInInventory(GameObject item)
    {
        bool emptySlot = true;
        //Trazi prazan slot
        foreach (GameObject slot in Player_Inventory.Inventory)
        {
            if (slot.transform.childCount == 0)
            {
                GameObject newItem = GameObject.Instantiate(item.GetComponent<ItemForInventory>().inventoryItem.gameObject, slot.transform);
                newItem.transform.localPosition = Vector2.zero;
                if(item.GetComponent<ItemForInventory>().stack > 0 && newItem.GetComponent<Stack>() != null)
                {
                    newItem.GetComponent<Stack>().StackAmout = item.GetComponent<ItemForInventory>().stack;
                    newItem.GetComponent<Stack>().OnValueChanged.Invoke();
                }
                GameObject.Destroy(item);
                emptySlot = true;
                break;
            }
            emptySlot = false;
        }
        //Izbacuje poruku ako nema mjesta
        if (!emptySlot)
        {            
            Debug.Log("Inventory is full!");
            _timerStart = true;
            MessageText.text = "Inventory is full!";
            MessageContainer.SetActive(true);
            _timerTicks = 0;
        }
    }
    //Implementacija vlastitog timera posto IEnumerator,  yield return new WaitForSeconds(5) i StartCoroutine()  funkcionira samo prvi put bez obzira sto bi Stopirao Coroutinu
    private void _timer(int ticks)
    {
        _timerTicks++;
        if(_timerTicks > ticks)
        {
            MessageContainer.SetActive(false);
            _timerStart = false;
        }
    }
}
