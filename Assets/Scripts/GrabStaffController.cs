using UnityEngine;
using SeaWizard.Weapons;

public class PlayerGrabController : MonoBehaviour
{
    // transforms for players hand and grab points 
    [Header("References")]
    public Transform grabOrigin;
    public Transform playerHandTransform;
    // variables for range and holding the staff
    [Header("Settings")]
    public float grabRange = 6f;
    public bool holdingStaff = false;

    private BaseStaff currentStaff;

    // logic for grabbing the staff with functions
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
            // single click logic for ice staff bolts / Poison cloud
            if (Input.GetMouseButtonDown(0))
                currentStaff.CastSpell();

            // hold down casting for continual casting (flame thrower spell)
            if (Input.GetMouseButton(0))
                currentStaff.StartCasting();

            if (Input.GetMouseButtonUp(0))
                currentStaff.StopCasting();
        }
    }

    // function with logic for attempting to grab a staff using ray casting
    void TryGrabStaff()
    {
        Camera cam = Camera.main;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * grabRange, Color.green, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            if (hit.collider.CompareTag("Staff"))
            {
                GrabStaff(hit.collider.gameObject);
                holdingStaff = true;
            }
        }
    }
    // function with logic for what happens when grab stuff is run
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
    // function for dropping current staff
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

            currentStaff.StopCasting(); // Safety
            currentStaff = null;
            holdingStaff = false;
        }
    }
}
