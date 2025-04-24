using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float floatSpeed = 2f;
    public float lifetime = 0.5f;
    float timer;

    private Vector3 moveDir = new Vector3(0, 1f, 0);

    public void Setup(float damage, Vector3 pos)
    {
        text.text = Mathf.RoundToInt(damage).ToString();
    }

    void Update()
    {
        if(timer >= lifetime)
            gameObject.SetActive(false);

        timer += Time.deltaTime;
        transform.position += moveDir * floatSpeed * Time.deltaTime;
        text.alpha = Mathf.Lerp(text.alpha, 0, Time.deltaTime * 10); // 점점 사라짐
    }

    void OnDisable()
    {
        text.text = "";
        text.alpha = 1;
        timer = 0.0f;
    }
}
