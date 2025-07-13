using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    // reference to the ui for enemy health and the enemy 
    public Slider healthSlider;
    public Transform target;
    // sets it to flow alittle above the enemies head 
    public Vector3 offset = new Vector3(0, 2f, 0); 

    // i put this in trying to make it so it never flips backwards it worked for a single enemy but when there is 2 in the scene it stops working never worked out why 
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;

            // face the camera correctly and avoid flipping backwards
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
        }
    }


    // sets the health value in the health bar for current/max enemy health
    public void SetHealth(float current, float max)
    {
        healthSlider.value = current / max;
    }
}
