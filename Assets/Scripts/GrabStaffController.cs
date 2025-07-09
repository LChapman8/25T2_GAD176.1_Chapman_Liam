using UnityEngine;
using SeaWizard.Weapons;

public class PlayerGrabController : MonoBehaviour
{
    [Header("References")]
    public Transform grabOrigin;
    public Transform playerHandTransform;

    [Header("Settings")]
    public float grabRange = 6f;
    public bool holdingStaff = false;

    [Tooltip("Layers that can be grabbed (e.g. Default, Interactable). Exclude Player layer.")]
    [SerializeField] private LayerMask interactableMask;

    private BaseStaff currentStaff;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentStaff == null)
                TryGrabStaff();
            else
                DropCurrentStaff();
        }

        if (holdingStaff && currentStaff != null)
        {
            if (Input.GetMouseButtonDown(0))
                currentStaff.CastSpell();

            if (Input.GetMouseButton(0))
                currentStaff.StartCasting();

            if (Input.GetMouseButtonUp(0))
                currentStaff.StopCasting();
        }
    }

    void TryGrabStaff()
    {
        Camera cam = Camera.main;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * grabRange, Color.green, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange, interactableMask))
        {
            Debug.Log($"Raycast hit: {hit.collider.name} on layer {LayerMask.LayerToName(hit.collider.gameObject.layer)} with tag {hit.collider.tag}");

            if (hit.collider.CompareTag("Staff"))
            {
                GrabStaff(hit.collider.gameObject);
                holdingStaff = true;
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything interactable.");
        }
    }

    void GrabStaff(GameObject staffObject)
    {
        DropCurrentStaff();

        Rigidbody rb = staffObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        Collider col = staffObject.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        staffObject.transform.SetParent(playerHandTransform);
        staffObject.transform.localPosition = Vector3.zero;
        staffObject.transform.localRotation = Quaternion.identity;

        currentStaff = staffObject.GetComponent<BaseStaff>();
    }

    void DropCurrentStaff()
    {
        if (currentStaff != null)
        {
            Rigidbody rb = currentStaff.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.detectCollisions = true;
            }

            Collider col = currentStaff.GetComponent<Collider>();
            if (col != null)
                col.enabled = true;

            currentStaff.transform.SetParent(null);
            currentStaff.transform.position = grabOrigin.position + grabOrigin.forward * 1.5f;

            currentStaff.StopCasting();
            currentStaff = null;
            holdingStaff = false;
        }
    }
}
