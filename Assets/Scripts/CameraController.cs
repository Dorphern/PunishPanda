using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject backgroundObject;

    # region Private Methods

    void Start ()
    {
        transform.position = positionContainObject(backgroundObject);
    }

    Vector3 positionContainObject (GameObject gObject)
    {
        Vector3 position = new Vector3(0, 0, 0);

        Bounds bounds = gObject.GetComponent<MeshFilter>().mesh.bounds;
        Vector3 size = gObject.transform.localScale;
        position.x = gObject.transform.position.x;
        position.y = gObject.transform.position.y;

        position.z = -size.y * 0.5f / (Mathf.Tan(0.5f * camera.fieldOfView * Mathf.Deg2Rad));
        float widthDist = (-size.x * 0.5f / (Mathf.Tan(0.5f * camera.fieldOfView * Mathf.Deg2Rad))) * Screen.height / Screen.width;
        if (widthDist < position.z) position.z = widthDist;

        return position;
    }

    # endregion
}