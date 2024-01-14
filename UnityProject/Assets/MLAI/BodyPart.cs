using UnityEngine;

public class BodyPart : MonoBehaviour
{
  public ArticulationBody ArticulationBody => articulationBody;
  public CollisionChecker GroundChecker => groundChecker;

  [SerializeField] private ArticulationBody articulationBody;
  [SerializeField] private CollisionChecker groundChecker;

  private Vector3 startPosition;
  private Quaternion startRotation;

  private Vector3 stiffness;
  private Vector3 forceLimit;

  private void Reset()
  {
    articulationBody = GetComponentInChildren<ArticulationBody>();
    groundChecker = GetComponentInChildren<CollisionChecker>();

    startPosition = articulationBody.transform.localPosition;
    startRotation = articulationBody.transform.localRotation;
  }

  private void Awake()
  {
    stiffness.x = articulationBody.xDrive.stiffness;
    stiffness.y = articulationBody.yDrive.stiffness;
    stiffness.z = articulationBody.zDrive.stiffness;

    forceLimit.x = articulationBody.xDrive.forceLimit;
    forceLimit.y = articulationBody.yDrive.forceLimit;
    forceLimit.z = articulationBody.zDrive.forceLimit;
  }

  public void ResetBody()
  {
    articulationBody.transform.localPosition = startPosition;
    articulationBody.transform.localRotation = startRotation;

    articulationBody.velocity = Vector3.zero;
    articulationBody.angularVelocity = Vector3.zero;

    articulationBody.jointPosition = new ArticulationReducedSpace(0f, 0f, 0f);
    articulationBody.jointForce = new ArticulationReducedSpace(0f, 0f, 0f);
    articulationBody.jointVelocity = new ArticulationReducedSpace(0f, 0f, 0f);

    SetTargetRotationX(0);
    SetTargetRotationY(0);
    SetTargetRotationZ(0);

    SetForceLimitDriveX(1);
    SetForceLimitDriveY(1);
    SetForceLimitDriveZ(1);

    articulationBody.velocity = Vector3.zero;
    articulationBody.angularVelocity = Vector3.zero;
  }

  public void SetTargetRotation(Vector3 rotation)
  {
    SetTargetRotationX(rotation.x);
    SetTargetRotationY(rotation.y);
    SetTargetRotationZ(rotation.z);
  }
  public Vector3 GetForceLimitDrive()
  {
    return new Vector3(articulationBody.xDrive.forceLimit / forceLimit.x, 
      articulationBody.yDrive.forceLimit / forceLimit.y, 
      articulationBody.zDrive.forceLimit / forceLimit.z);
  }
  public void SetForceLimitDrive(Vector3 rotation)
  {
    SetForceLimitDriveX(rotation.x);
    SetForceLimitDriveY(rotation.y);
    SetForceLimitDriveZ(rotation.z);
  }
  public void SetTargetRotationX(float value)
  {
    articulationBody.xDrive = SetTargetRotationDrive(articulationBody.xDrive, value);
  }
  public void SetTargetRotationY(float value)
  {
    articulationBody.yDrive = SetTargetRotationDrive(articulationBody.yDrive, value);
  }
  public void SetTargetRotationZ(float value)
  {
    articulationBody.zDrive = SetTargetRotationDrive(articulationBody.zDrive, value);
  }

  public void SetForceLimitDriveX(float value)
  {
    articulationBody.xDrive = SetForceLimitDrive(articulationBody.xDrive, value, forceLimit.x);
  }
  public void SetForceLimitDriveY(float value)
  {
    articulationBody.yDrive = SetForceLimitDrive(articulationBody.yDrive, value, forceLimit.y);
  }
  public void SetForceLimitDriveZ(float value)
  {
    articulationBody.zDrive = SetForceLimitDrive(articulationBody.zDrive, value, forceLimit.z);
  }

  private ArticulationDrive SetTargetRotationDrive(ArticulationDrive drive, float value)
  {
    var rot = Mathf.Lerp(drive.lowerLimit, drive.upperLimit, (value + 1f) * 0.5f);
    drive.target = rot;
    return drive;
  }
  private ArticulationDrive SetForceLimitDrive(ArticulationDrive drive, float value, float maxValue)
  {
    var forceLimit = (value + 1f) * 0.5f * maxValue;
    drive.forceLimit = forceLimit;
    return drive;
  }
}
