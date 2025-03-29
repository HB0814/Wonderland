using Unity.VisualScripting;
using UnityEngine;

public class CooldownCheck : MonoBehaviour
{
    public float cooldown;
    public float timer = 0.0f;
    public bool isAttack = false;

    private void Update()
    {
        timer += Time.deltaTime; //Ÿ�̸� �ð� ����

        if (timer >= cooldown && !isAttack) //��Ÿ�� ����
        {
            isAttack = true;
            timer = 0.0f;
        }

    }
}
