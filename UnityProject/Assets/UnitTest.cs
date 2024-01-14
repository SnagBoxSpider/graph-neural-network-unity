using System.Linq;
using UnityEngine;

public class UnitTest : MonoBehaviour
{
  [SerializeField]
  private UnitBody body;

  [SerializeField]
  private Vector3[] neurons;

  [SerializeField]
  private bool random = false;


  private void Awake()
  {
    neurons = new Vector3[body.Segments.Count];
  }
  private void FixedUpdate()
  {
    for (int i = 0; i < body.Segments.Count; i++)
    {
      if (random)
        neurons[i] = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
      var segment = body.Segments[i];
      segment.RotateX(neurons[i].x);
      segment.RotateY(neurons[i].y);
      segment.RotateZ(neurons[i].z);
    }
  }
}
