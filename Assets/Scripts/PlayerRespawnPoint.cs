using UnityEngine;

public class PlayerRespawnPoint : MonoBehaviour
{
    public static Vector3 RespawnPosition;

    private void Awake()
    {
        RespawnPosition = transform.position;
    }
}
