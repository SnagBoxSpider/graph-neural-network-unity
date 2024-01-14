using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeuronView : MonoBehaviour
{
  public string Name
  {
    get => textMesh.text;
    set => textMesh.text = value;
  }

  public float Value
  {
    get => value;
    set
    {
      this.value = value;
      meshRenderer.material.color = Color.Lerp(colorNegative, colorPositive, (value + 1f) * 0.5f);
    }
  }

  public Rigidbody Rigidbody => rigidbody;

  [SerializeField]
  private Color colorPositive = Color.white;
  [SerializeField]
  private Color colorNegative = Color.white * 0.1f;

  [SerializeField]
  private MeshRenderer meshRenderer;

  [SerializeField]
  private TextMeshPro textMesh;

  [SerializeField]
  private new Rigidbody rigidbody;

  private float value;

  private Dictionary<NeuronView, SpringJoint> springJoints = new Dictionary<NeuronView, SpringJoint>();

  public void UpdateNameRotation()
  {
    textMesh.transform.rotation = Quaternion.LookRotation(textMesh.transform.position - Camera.current.transform.position, Vector3.up);
  }

  public void Attach(NeuronView neuronView, float minDistance = 5f)
  {
    if (rigidbody == null || neuronView.rigidbody == null)
      return;

    SpringJoint springJoint = gameObject.AddComponent<SpringJoint>();
    springJoint.anchor = Vector3.zero;
    springJoint.minDistance = minDistance;
    springJoint.maxDistance = minDistance * 1.5f;
    springJoint.connectedBody = neuronView.rigidbody;
    springJoints.Add(neuronView, springJoint);
  }

  public void Detach(NeuronView neuronView)
  {
    if (springJoints.Remove(neuronView, out SpringJoint springJoint))
    {
      Destroy(springJoint);
    }
  }
}
