using UnityEngine;

public class UnitSegment : MonoBehaviour
{
  public EventCollision Trigger => trigger;
  public ArticulationBody ArticulationBody => articulationBody;

  [SerializeField]
  private ArticulationBody articulationBody;

  [SerializeField]
  private EventCollision trigger;

  private void Reset()
  {
    articulationBody = GetComponent<ArticulationBody>();
    trigger = GetComponent<EventCollision>();
  }

  public void RotateX(float delta)
  {
    var drive = articulationBody.xDrive;
    switch (articulationBody.swingYLock)
    {
      case ArticulationDofLock.LimitedMotion:
        drive.target = Mathf.Clamp(drive.target + delta, drive.lowerLimit, drive.upperLimit);
        break;

      case ArticulationDofLock.FreeMotion:
        drive.target = Mathf.Repeat(drive.target + delta, 359);
        break;
    }
    articulationBody.xDrive = drive;
  }
  public void RotateY(float delta)
  {
    var drive = articulationBody.yDrive;
    switch (articulationBody.swingYLock)
    {
      case ArticulationDofLock.LimitedMotion:
        drive.target = Mathf.Clamp(drive.target + delta, drive.lowerLimit, drive.upperLimit);
        break;

      case ArticulationDofLock.FreeMotion:
        drive.target = Mathf.Repeat(drive.target + delta, 359);
        break;
    }
    articulationBody.yDrive = drive;
  }
  public void RotateZ(float delta)
  {
    var drive = articulationBody.zDrive;
    switch (articulationBody.swingYLock)
    {
      case ArticulationDofLock.LimitedMotion:
        drive.target = Mathf.Clamp(drive.target + delta, drive.lowerLimit, drive.upperLimit);
        break;

      case ArticulationDofLock.FreeMotion:
        drive.target = Mathf.Repeat(drive.target + delta, 359);
        break;
    }
    articulationBody.zDrive = drive;
  }
}
