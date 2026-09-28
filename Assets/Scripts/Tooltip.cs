using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class Tooltip : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public List<string> AttributeList = new List<string>();
    public string ItemNameText;
    public string ItemDescriptionText;
    private GameObject _toolTipPannel;
    private GameObject _attributePannel;
    public GameObject AttributeField;
    private TMP_Text _itemName;
    private TMP_Text _itemDescription;

    private List<GameObject> AttributesCreated = new List<GameObject>();
    private UI_Controler _uiControler;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Input.GetKey(KeyCode.X))
        {
            _uiControler.ToolTipActive();
            _toolTipPannel.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;
            foreach (string attribute in AttributeList)
            {
                GameObject newAttribute = GameObject.Instantiate(AttributeField, _attributePannel.transform);
                newAttribute.GetComponent<TextMeshProUGUI>().SetText(attribute);
                AttributesCreated.Add(newAttribute);
            }
            _itemName.SetText(ItemNameText);
            _itemDescription.SetText(ItemDescriptionText);
        }       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _toolTipPannel.SetActive(false);
        foreach (GameObject attribute in AttributesCreated)
        {
            GameObject.Destroy(attribute);
        }
        AttributesCreated.Clear();
    }

    void Start()
    {
        _uiControler = GameObject.Find("GameManager").GetComponent<UI_Controler>();
        _toolTipPannel = _uiControler.Tooltip;
        _attributePannel = _toolTipPannel.transform.GetChild(2).gameObject;
        _itemName = _toolTipPannel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _itemDescription = _toolTipPannel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)||Input.GetMouseButtonDown(2))
        {
            _toolTipPannel.SetActive(false);
            foreach (GameObject attribute in AttributesCreated)
            {
                GameObject.Destroy(attribute);
            }
            AttributesCreated.Clear();
        }
    }
}
