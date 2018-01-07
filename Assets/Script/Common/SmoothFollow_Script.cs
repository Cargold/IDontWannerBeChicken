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

    public float limitPos_Left;
    public float limitPos_Right;

    public void Init_Func()
    {
        thisTransform = this.transform;
        velocity = new Vector3(0.5f, 0.5f, 0.5f);

        this.transform.position = new Vector3(6.48f, 8f, -10f);
    }

    void Update()
    {
        //if (Player_Data.Instance.heroClass.isAlive == false) return;

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
            if (!isLockX)
                newPos.x = target.position.x + offSetX;

            if (!isLockY)
                newPos.y = target.position.y + offSetY;

            if (!isLockZ)
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

        Vector3 _calcMove = Vector3.Slerp(thisTransform.position, newPos, Time.time);

        if (_calcMove.x < limitPos_Left)
            _calcMove = new Vector3(limitPos_Left, _calcMove.y, _calcMove.z);
        else if (limitPos_Right < _calcMove.x)
            _calcMove = new Vector3(limitPos_Right, _calcMove.y, _calcMove.z);

        this.transform.position = _calcMove;
    }
}