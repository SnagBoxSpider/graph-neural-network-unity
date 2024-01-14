using System.Collections.Generic;
using UnityEngine;

public class UnitBody : MonoBehaviour
{
  public Vision Vision
  {
    get { return vision; }
  }
  public IReadOnlyList<UnitSegment> Segments
  {
    get { return segments; }
  }

  [SerializeField]
  private Vision vision;
  [SerializeField]
  private List<UnitSegment> segments = new List<UnitSegment>();
}
