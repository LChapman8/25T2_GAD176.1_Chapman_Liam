using SeaWizard.Weapons;
using UnityEngine;

public class PlayerGrabController : MonoBehaviour
{
    [Header("References")]
    public Transform grabOrigin;
    public Transform playerHandTransform;

    [Header("Settings")]
    public float grabRange = 6f;
    public bool holdingStaff = false;

    private BaseStaff currentStaff;

    //grabs the staff im looking at if i dont have one, if one is currently held, drops it instead.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentStaff == null)
            {
                TryGrabStaff();
            }
            else
            {
                DropCurrentStaff();
            }
        }
    }
    // uses ray cast in crosshair direction and if it hits a staff, then pick it up.
    void TryGrabStaff()
    {
        Camera cam = Camera.main;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Debug.DrawRay(ray.origin, ray.direction * grabRange, Color.green, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            Debug.Log("Ray hit: " + hit.collider.gameObject.name);

            if (hit.collider.CompareTag("Staff"))
            {
                GrabStaff(hit.collider.gameObject);
                holdingStaff = true;
            }
        }
    }
    // drops the current staff if i have one, grabs the staff im looking at and puts it in my player hand transform, makes it a child.
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
        {
            col.enabled = false;
        }

        staffObject.transform.SetParent(playerHandTransform);
        staffObject.transform.localPosition = Vector3.zero;
        staffObject.transform.localRotation = Quaternion.identity;

        currentStaff = staffObject.GetComponent<BaseStaff>();
        if (currentStaff != null)
        {
            // enables the script so update runs
            currentStaff.enabled = true; 
        }
    }
    // if i have a staff in my hand and E is press, drops the current staff infront of me and removes it as a child.
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
            {
                col.enabled = true;
            }

            currentStaff.transform.SetParent(null);
            currentStaff.transform.position = grabOrigin.position + grabOrigin.forward * 1.5f;

            //  disables script when dropped
            currentStaff.enabled = false; 
            currentStaff = null;
            holdingStaff = false;
        }
    }
}
