using UnityEngine;
using UnityEngine.UI;

//Health bar iznad glave neprijatelja (world space canvas), vidljiv tek kad primi stetu
public class EnemyHealthBar : MonoBehaviour
{
    public Enemy Target;
    public Image Fill;
    public float HeightOffset = 2.5f;

    private float shown = 1;

    void Start()
    {
        if (Target == null) Target = GetComponentInParent<Enemy>();
        gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (Target == null || Target.Health <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.position = Target.transform.position + Vector3.up * HeightOffset;
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
        float target = (float)Target.Health / Target.MaxHealth;
        shown = Mathf.MoveTowards(shown, target, Time.deltaTime * 2f);
        Fill.fillAmount = shown;
    }
}
