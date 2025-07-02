using UnityEngine;
using SeaWizard.Enemy;

public class SlowEffect : MonoBehaviour
{
    // variables for controling slows
    private BaseEnemy enemy;
    private float originalSpeed;
    private float slowDuration;
    private float slowFactor;
    private float timer;

    // function for applying the slow to an enemy by checking if they have the base enemy script ruduce their move speed by a set for a period of time 
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

    // if the timer has exceeded slow time then remove the slow. 
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