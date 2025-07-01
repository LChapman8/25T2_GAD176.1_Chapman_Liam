using UnityEngine;
using SeaWizard.Enemy;

public class SlowEffect : MonoBehaviour
{
    private BaseEnemy enemy;
    private float originalSpeed;
    private float slowDuration;
    private float slowFactor;
    private float timer;

    public void ApplySlow(float factor, float duration)
    {
        if (enemy == null)
            enemy = GetComponent<BaseEnemy>();

        if (enemy != null)
        {
            originalSpeed = enemy.moveSpeed;
            slowFactor = factor;
            slowDuration = duration;

            enemy.moveSpeed *= slowFactor;
            timer = 0f;
        }
    }

    private void Update()
    {
        if (enemy == null) return;

        timer += Time.deltaTime;
        if (timer >= slowDuration)
        {
            enemy.moveSpeed = originalSpeed;
            Destroy(this);
        }
    }
}