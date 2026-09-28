using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Item_Spawner : MonoBehaviour
{
    public TMP_Text ItemName;
    public GameObject Player;
    public List<GameObject> SpawnObject = new List<GameObject>();
    private int _objectNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        ItemName.text = "Spawn: " + SpawnObject[_objectNum].name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_objectNum < SpawnObject.Count)
            {
                _objectNum++;
                if(_objectNum == SpawnObject.Count)
                {
                    _objectNum = SpawnObject.Count - 1;
                }
                ItemName.text = "Spawn: " + SpawnObject[_objectNum].name;
            }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (_objectNum > 0)
            {
                _objectNum--;
                ItemName.text = "Spawn: " + SpawnObject[_objectNum].name;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject item = GameObject.Instantiate(SpawnObject[_objectNum]);
            item.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y - 2f, -2f);
            Debug.Log("Spawned " + SpawnObject[_objectNum].name);
        }
    }
}
