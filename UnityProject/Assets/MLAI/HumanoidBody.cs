using System.Collections.Generic;
using UnityEngine;

public sealed class HumanoidBody : Body
{
  public override IReadOnlyList<BodyPart> BodyParts => bodyParts;

  public BodyPart Head => head;

  public BodyPart Chest => chest;
  public BodyPart Belly => belly;
  public BodyPart Pelvis => pelvis;

  public BodyPart ThighL => thighL;
  public BodyPart ShinL => shinL;

  public BodyPart ThighR => thighR;
  public BodyPart ShinR => shinR;

  public BodyPart ArmL => armL;
  public BodyPart ForearmL => forearmL;

  public BodyPart ArmR => armR;
  public BodyPart ForearmR => forearmR;

  [SerializeField] private BodyPart head;

  [SerializeField] private BodyPart chest;
  [SerializeField] private BodyPart belly;
  [SerializeField] private BodyPart pelvis;

  [SerializeField] private BodyPart thighL;
  [SerializeField] private BodyPart shinL;

  [SerializeField] private BodyPart thighR;
  [SerializeField] private BodyPart shinR;

  [SerializeField] private BodyPart armL;
  [SerializeField] private BodyPart forearmL;

  [SerializeField] private BodyPart armR;
  [SerializeField] private BodyPart forearmR;

  private List<BodyPart> bodyParts = new List<BodyPart>();

  private void Awake()
  {
    bodyParts.Add(head);

    bodyParts.Add(chest);
    bodyParts.Add(belly);
    bodyParts.Add(pelvis);

    bodyParts.Add(thighL);
    bodyParts.Add(shinL);

    bodyParts.Add(thighR);
    bodyParts.Add(shinR);

    bodyParts.Add(armL);
    bodyParts.Add(forearmL);

    bodyParts.Add(armR);
    bodyParts.Add(forearmR);
  }

  public Vector3 GetAvgVelocity()
  {
    Vector3 velocity = Vector3.zero;

    foreach (var item in bodyParts)
    {
      velocity += item.ArticulationBody.velocity;
    }

    velocity /= bodyParts.Count;
    return velocity;
  }

  public void ResetBody()
  {
    foreach (var bodyPart in bodyParts)
    {
      bodyPart.ResetBody();
    }
  }
}
