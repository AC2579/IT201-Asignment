using UnityEngine;

public class CylinderRotation : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdateUpdate()
    {
        transform.Rotate(new Vector3(0, 15, 0) * Time.deltaTime);
    }
}
