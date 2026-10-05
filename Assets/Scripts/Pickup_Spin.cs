using UnityEngine;

//Vrti i podize pickup (ruple, heart) i privlaci ga playeru kad je blizu
public class Pickup_Spin : MonoBehaviour
{
    public float RotateSpeed = 180;
    public float MagnetRange = 4;
    public float MagnetSpeed = 8;

    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        transform.Rotate(0, RotateSpeed * Time.deltaTime, 0, Space.World);

        GameObject player = GameObject.Find("Player");
        if (player != null && Vector3.Distance(transform.position, player.transform.position) < MagnetRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position + Vector3.up * 0.8f, MagnetSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, startY + Mathf.Sin(Time.time * 3) * 0.15f, transform.position.z);
        }
    }
}
