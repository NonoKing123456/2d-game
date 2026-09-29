using UnityEngine;

public class ImgBackground : MonoBehaviour
{
    public Transform cameraTransform;
    [Range(0f, 1f)] public float followRatio = 0.95f;

    private Vector3 cameraStart;
    private Vector3 skyStart;

    private void Start()
    {
        cameraStart = cameraTransform.position;
        skyStart = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 moved = cameraTransform.position - cameraStart;

        transform.position = skyStart + new Vector3(
            moved.x * followRatio,
            moved.y * followRatio,
            0f
        );
    }
}
