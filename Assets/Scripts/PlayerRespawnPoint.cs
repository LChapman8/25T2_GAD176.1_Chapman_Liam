using UnityEngine;

public class PlayerRespawnPoint : MonoBehaviour
{
    public static Vector3 RespawnPosition;
    // a function that handles the respawn location
    private void Awake()
    {
        RespawnPosition = transform.position;
    }
}
