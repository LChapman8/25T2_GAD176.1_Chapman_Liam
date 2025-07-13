using UnityEngine;
using SeaWizard.Weapons;

public class PlayerGrabController : MonoBehaviour
{
    // references to the grab origin and the players hand where the staff goes 
    [Header("References")]
    public Transform grabOrigin;
    public Transform playerHandTransform;
    // variables for the range and if the players holding a staff 
    [Header("Settings")]
    public float grabRange = 6f;
    public bool holdingStaff = false;
    // a layer mask list that allows you to pick which layers are grabable 
    [Tooltip("Layers that can be grabbed (e.g. Default, Interactable). Exclude Player layer.")]
    [SerializeField] private LayerMask interactableMask;
    // a reference to the bass staff linking to the current
    private BaseStaff currentStaff;

    // on update if the player presses "E" try to grab a staff or drop one if they already have one
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
    // a function that uses raycasting to centre screen to try and detect if a staff thats grabbable is in range 
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
    // a function to pick up the object, moving to a child of the players hand. 
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
    // a function for dropping a currently held staff and removing it as a child 
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
