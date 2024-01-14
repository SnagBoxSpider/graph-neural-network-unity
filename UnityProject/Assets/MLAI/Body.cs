using System.Collections.Generic;
using UnityEngine;

public abstract class Body : MonoBehaviour
{
  public abstract IReadOnlyList<BodyPart> BodyParts { get; }
}
