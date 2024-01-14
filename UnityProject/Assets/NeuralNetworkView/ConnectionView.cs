using UnityEngine;

public class ConnectionView : MonoBehaviour
{
  public NeuronView FromNeuronView => fromNeuronView;

  public float Value
  {
    get => value;
    set
    {
      this.value = value;
      lineRenderer.material.color = Color.Lerp(colorNegative, colorPositive, (value + 1f) * 0.5f);
    }
  }

  [SerializeField]
  private Color colorPositive = Color.white;
  [SerializeField]
  private Color colorNegative = Color.white * 0.1f;

  [SerializeField]
  private LineRenderer lineRenderer;

  [SerializeField]
  private NeuronView fromNeuronView;
  [SerializeField]
  private NeuronView toNeuronView;

  private float value;

  public void UpdatePosition()
  {
    lineRenderer.SetPosition(0, fromNeuronView.transform.position);
    lineRenderer.SetPosition(1, toNeuronView.transform.position);
  }

  public void SetTargets(NeuronView fromNeuronView, NeuronView toNeuronView)
  {
    this.fromNeuronView = fromNeuronView;
    this.toNeuronView = toNeuronView;
  }
}
