using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Transform target; // The enemy to follow
    public Vector3 offset = new Vector3(0, 2f, 0); // Position offset above the enemy

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;

            // Face the camera correctly, avoid flipping backwards
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
        }
    }



    public void SetHealth(float current, float max)
    {
        healthSlider.value = current / max;
    }
}
