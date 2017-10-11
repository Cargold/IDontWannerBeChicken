using UnityEngine;

public class SmoothFollow_Script : MonoBehaviour
{
    public float smoothTime = 0.3f;

    public bool isLockX;
    public bool isLockY;
    public bool isLockZ;
    public float offSetX;
    public float offSetY;
    public float offSetZ;
    public bool isUseSmoothing;
    public Transform target;

    private Transform thisTransform;
    private Vector3 velocity;

    private void Awake()
    {
        thisTransform = transform;
        velocity = new Vector3(0.5f, 0.5f, 0.5f);
    }

    void FixedUpdate()
    {
        var newPos = Vector3.zero;

        if (isUseSmoothing)
        {
            if (!isLockX)
                newPos.x = Mathf.SmoothDamp(thisTransform.position.x, target.position.x + offSetX, ref velocity.x, smoothTime);

            if (!isLockY)
                newPos.y = Mathf.SmoothDamp(thisTransform.position.y, target.position.y + offSetY, ref velocity.y, smoothTime);

            if (!isLockZ)
                newPos.z = Mathf.SmoothDamp(thisTransform.position.z, target.position.z + offSetZ, ref velocity.z, smoothTime);
        }
        else
        {
            newPos.x = target.position.x + offSetX;
            newPos.y = target.position.y + offSetY;
            newPos.z = target.position.z + offSetZ;
        }

        if (isLockX)
        {
            newPos.x = thisTransform.position.x;
        }

        if (isLockY)
        {
            newPos.y = thisTransform.position.y;
        }

        if (isLockZ)
        {
            newPos.z = thisTransform.position.z;
        }

        transform.position = Vector3.Slerp(thisTransform.position, newPos, Time.time);
    }
}