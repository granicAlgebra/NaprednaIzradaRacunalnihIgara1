using UnityEngine;
using TMPro;

//Broj stete koji odleti iznad glave i nestane
public class DamageNumber : MonoBehaviour
{
    public float Lifetime = 0.9f;
    public float RiseSpeed = 1.6f;

    private TextMeshPro text;
    private float age;
    private Vector3 drift;
    private Vector3 baseScale;

    //Prefab je u Resources pa ga moze stvorit bilo tko
    public static void Spawn(Vector3 position, int amount, Color color)
    {
        GameObject prefab = Resources.Load<GameObject>("DamageNumber");
        if (prefab == null) return;
        GameObject go = Instantiate(prefab, position, Quaternion.identity);
        DamageNumber dn = go.GetComponent<DamageNumber>();
        dn.text = go.GetComponent<TextMeshPro>();
        dn.text.text = amount.ToString();
        dn.text.color = color;
    }

    void Start()
    {
        if (text == null) text = GetComponent<TextMeshPro>();
        drift = new Vector3(Random.Range(-0.4f, 0.4f), 0, 0);
        baseScale = transform.localScale;
    }

    void LateUpdate()
    {
        age += Time.deltaTime;
        float t = age / Lifetime;
        if (t >= 1)
        {
            Destroy(gameObject);
            return;
        }
        transform.position += (Vector3.up * RiseSpeed * (1 - t) + drift) * Time.deltaTime;
        //kratki "pop" na pocetku
        float pop = t < 0.15f ? Mathf.Lerp(0.6f, 1.3f, t / 0.15f) : Mathf.Lerp(1.3f, 1f, (t - 0.15f) / 0.85f);
        transform.localScale = baseScale * pop;
        text.alpha = t < 0.6f ? 1 : 1 - (t - 0.6f) / 0.4f;
        //okreni prema kameri
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
