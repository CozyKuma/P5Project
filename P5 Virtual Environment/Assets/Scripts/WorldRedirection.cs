using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRedirection : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject target;
    public GameObject origin;
    public float curvatureGain = 1.5f;
    [SerializeField]
    private float rotation;
    [SerializeField]
    private float angleDiff;
    [SerializeField]
    private bool isMoving;
    public float movementThreshold = 0.05f;

    [SerializeField]
    private Vector3 oldEulerAngles;
    [SerializeField]
    private Vector3 currentEulerAngles;
    [SerializeField]
    private Vector3 oldPosition;
    [SerializeField]
    private Vector3 currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        oldEulerAngles = playerObject.transform.rotation.eulerAngles;
        currentEulerAngles = playerObject.transform.rotation.eulerAngles;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentEulerAngles = playerObject.transform.rotation.eulerAngles;
        currentPosition = playerObject.transform.position;

        float targetDirection = findTargetDirection(playerObject, target);
        float centerDirection = findTargetDirection(playerObject, origin);
        //RotationGain(targetDirection);
        SteeringToCenter();

        oldEulerAngles = playerObject.transform.rotation.eulerAngles;
        oldPosition = playerObject.transform.position;

    }

    private void SteeringToCenter()
    {
        // try to steer right always
        float posDiffX = oldPosition.x - currentPosition.x; // Checks if there is movement along the x-axis
        float posDiffZ = oldPosition.z - currentPosition.z; // Checks if there is movement along the z-axis
        angleDiff = Mathf.DeltaAngle(oldEulerAngles.y, currentEulerAngles.y); // Used to check if currently rotating

        if (posDiffX > movementThreshold || posDiffX < movementThreshold * -1 || posDiffZ > movementThreshold || posDiffZ < movementThreshold * -1)
        {
            isMoving = true;
        } else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            rotation = curvatureGain * Time.deltaTime;
            transform.RotateAround(playerObject.transform.position, playerObject.transform.up, rotation);
        }
    }

    private void RotationGain(float targetDirection)
    {

        angleDiff = Mathf.DeltaAngle(oldEulerAngles.y, currentEulerAngles.y); // Used to check if currently rotating

        if (Mathf.Abs(angleDiff) < 0.01f) // does not rotate if the difference is too small.
        {

        }
        else
        {
            rotation = angleDiff * curvatureGain * Time.deltaTime;
            if (targetDirection > 5f)
            {
                // turn left
                //Debug.Log("Turn Left to Target");
                transform.RotateAround(playerObject.transform.position, playerObject.transform.up, rotation);
            }
            else if (targetDirection < -5f)
            {
                // turn right
                //Debug.Log("Turn Right to Target");
                transform.RotateAround(playerObject.transform.position, playerObject.transform.up, rotation * -1);
            }
            else if (targetDirection >= -5f && targetDirection <= 5f)
            {
                // On Target
            }
        }
    }

    private float findTargetDirection(GameObject player, GameObject target)
    {
        Vector3 targetAngle = target.transform.position - playerObject.transform.position;
        Vector3 forward = player.transform.forward;

        float angle = Vector3.SignedAngle(targetAngle, forward, Vector3.up);


        //Debug.Log(angle);
        return angle;
    }
}
