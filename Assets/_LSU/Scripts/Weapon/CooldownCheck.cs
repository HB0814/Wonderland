using Unity.VisualScripting;
using UnityEngine;

public class CooldownCheck : MonoBehaviour
{
    public float cooldown;
    public float timer = 0.0f;
    public bool isAttack = false;

    private void Update()
    {
        timer += Time.deltaTime; //타이머 시간 증가

        if (timer >= cooldown && !isAttack) //쿨타임 종료
        {
            isAttack = true;
            timer = 0.0f;
        }

    }
}
