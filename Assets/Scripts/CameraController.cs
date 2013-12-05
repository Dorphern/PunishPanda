using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject backgroundObject;

    # region Private Methods

    void Start ()
    {
        if (camera.isOrthoGraphic)
        {
            transform.position = positionContainObjectOrtographic(backgroundObject);
        }
        else
        {
            transform.position = positionContainObjectPerspective(backgroundObject);
        }
    }

    Vector3 positionContainObjectPerspective (GameObject gObject)
    {
        Vector3 position = new Vector3(0, 0, 0);

        Vector3 size = gObject.transform.localScale;
        position.x = gObject.transform.position.x;
        position.y = gObject.transform.position.y;

        position.z = -size.y * 0.5f / (Mathf.Tan(0.5f * camera.fieldOfView * Mathf.Deg2Rad));
        float widthDist = (-size.x * 0.5f / (Mathf.Tan(0.5f * camera.fieldOfView * Mathf.Deg2Rad))) * Screen.height / Screen.width;
        if (widthDist < position.z) position.z = widthDist;

        return position;
    }

    Vector3 positionContainObjectOrtographic (GameObject gObject)
    {
        Vector3 position = new Vector3(0, 0, 0);

        Vector3 size = gObject.transform.localScale;
        position.x = gObject.transform.position.x;
        position.y = gObject.transform.position.y;

        float height = size.y / 2;
        float width = size.x / Screen.width * Screen.height / 2;

        if (height < width) 
        {
            camera.orthographicSize = width;
        }
        else 
        {
            camera.orthographicSize = height;
        }

        return position;
    }

    # endregion
}
