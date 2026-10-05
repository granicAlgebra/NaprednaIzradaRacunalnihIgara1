using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag_and_drop : MonoBehaviour, IPointerClickHandler
{
    public GameObject Player;
    public GameObject DropItem;
    public GameObject CanvasObject;
    //Prosli parent kojeg koristi kako bi se item vratio nazad u slucaju neuspjesnog dropanja na slot ili izvan inventory panela
    private GameObject _lastParent;
    //Flag za koji sluzi za samo jedno izvrsavanje postavljanja zadnjeg parenta
    private bool _isSetLastParent = false;
    private bool _isItemInAir = false;
    private Player_Inventory _PlayerInventory;

    void Start()
    {
        CanvasObject = GameObject.Find("Canvas");
        _PlayerInventory = GameObject.Find("GameManager").GetComponent<Player_Inventory>();
        Player = GameObject.Find("Player");
    }
    void Update()
    {
        if (_isItemInAir)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _dropItem();
                _isItemInAir = false;
                return;
            }
            //Ako se inventory zatvori dok je u zraku vraca se nazad u slot
            if (Keyboard.current.iKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame)
            {
                transform.SetParent(_lastParent.transform);
                transform.localPosition = Vector2.zero;
                _isItemInAir = false;
                Player_Inventory.EmptySlot = null;
                Player_Inventory.SlotPointer = "";
                Player_Inventory.DragedItemType = "";
                gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            transform.position = (Vector3)Mouse.current.position.ReadValue();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _isItemInAir = true;
            _inAirItem();
        }
        //Dropa item vani u kombinaciji tipke X sa klikom misa
        if (Keyboard.current.xKey.isPressed)
        {
            if (gameObject.GetComponent<Stack>() != null)
            {
                GameObject stackItem = GameObject.Instantiate(DropItem, new Vector3(Player.transform.position.x, Player.transform.position.y - 1f, -2f), Quaternion.identity);
                stackItem.GetComponent<ItemForInventory>().stack = this.gameObject.GetComponent<Stack>().StackAmout;
                GameObject.Destroy(this.gameObject);
            }
            else
            {
                GameObject.Instantiate(DropItem, new Vector3(Player.transform.position.x, Player.transform.position.y - 1f, -2f), Quaternion.identity);
                GameObject.Destroy(this.gameObject);
            }
        }
    }
    private void _dropItem()
    {
        _isSetLastParent = false;
        // Provjerava dali je unutar inventorija 
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Za equipanje itema na lika
            EquipChange("Weapon", _PlayerInventory.WeaponSlot.transform, "WeaponSlot");
            EquipChange("Bag", _PlayerInventory.BagSlot.transform, "BagSlot");
            EquipChange("Glove", _PlayerInventory.GloveSlot.transform, "GloveSlot");
            EquipChange("Shield", _PlayerInventory.ShieldSlot.transform, "ShieldSlot");
            EquipChange("Armour", _PlayerInventory.ArmourSlot.transform, "ArmourSlot");
            //Za promjene polozaja itema u inventoriju i ostale iteme koji se ne mogu equipati
            if (Player_Inventory.EmptySlot != null)
            {
                transform.SetParent(Player_Inventory.EmptySlot.transform);
                transform.localPosition = Vector2.zero;
            }
            else
            {
                transform.SetParent(_lastParent.transform);
                transform.localPosition = Vector2.zero;
            }
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        //Ako ispusti item izvan onda dropa van
        else
        {
            if(gameObject.GetComponent<Stack>() != null)
            {
                GameObject stackItem = GameObject.Instantiate(DropItem, new Vector3(Player.transform.position.x, Player.transform.position.y - 1f, -2f), Quaternion.identity);
                stackItem.GetComponent<ItemForInventory>().stack = this.gameObject.GetComponent<Stack>().StackAmout;
                GameObject.Destroy(this.gameObject);
            }
            else
            {
                GameObject.Instantiate(DropItem, new Vector3(Player.transform.position.x, Player.transform.position.y - 1f, -2f), Quaternion.identity);
                GameObject.Destroy(this.gameObject);
            }            
        }
        //Sve varijalbe vraca na default vrjednosti na kraju svakog dropanja radi sprjecavanja bugova
        Player_Inventory.EmptySlot = null;
        Player_Inventory.SlotPointer = "";
        Player_Inventory.DragedItemType = "";
        _lastParent = null;
        _isItemInAir = false;
    }
    private void _inAirItem()
    {
        SetLastParent();
        //Stavlja mu novog parenta koji je iznad svih panela za drag and drop kako se nebi desili bugovi prekrivanja objekta od drugih panela
        transform.SetParent(CanvasObject.transform);
        //Odblokira raycast kako bi mis pointer bio u kontaktu sa itemima ispod draganog itema
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        Player_Inventory.DragedItemType = transform.tag;
    }
    //Ako postoji vec equipani item u slotu, novi item ce ga zamjeniti, stari ide na mjesto zadnjeg parenta novog itema
    private void EquipChange(string tag, Transform slot, string pointer)
    {
        if (this.transform.tag == tag && slot.childCount > 0 && Player_Inventory.SlotPointer == pointer)
        {
            slot.GetChild(0).transform.SetParent(_lastParent.transform);
            _lastParent.transform.GetChild(0).transform.localPosition = Vector2.zero;            
        }
    }
    //Postavlja zadnjeg parenta
    private void SetLastParent()
    {
        if (!_isSetLastParent)
        {
            _lastParent = this.transform.parent.gameObject;
            _isSetLastParent = true;
        }
    }
}
