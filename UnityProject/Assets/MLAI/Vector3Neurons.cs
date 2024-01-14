using GraphNeuralNetworks;
using UnityEngine;

public class QuaternionNeurons
{
  private NamedNeuron neuronW;
  private NamedNeuron neuronX;
  private NamedNeuron neuronY;
  private NamedNeuron neuronZ;

  public NamedNeuron NeuronW => neuronW;
  public NamedNeuron NeuronX => neuronX;
  public NamedNeuron NeuronY => neuronY;
  public NamedNeuron NeuronZ => neuronZ;

  public QuaternionNeurons(string name)
  {
    neuronW = new NamedNeuron($"{name}_W");
    neuronX = new NamedNeuron($"{name}_X");
    neuronY = new NamedNeuron($"{name}_Y");
    neuronZ = new NamedNeuron($"{name}_Z");
  }

  public void SetValue(Quaternion value)
  {
    neuronX.Value = value.x;
    neuronY.Value = value.y;
    neuronZ.Value = value.z;
  }
  public Quaternion GetValue()
  {
    return new Quaternion(neuronW.Value, neuronX.Value, neuronY.Value, neuronZ.Value);
  }
}
public class Vector3Neurons
{
  private NamedNeuron neuronX;
  private NamedNeuron neuronY;
  private NamedNeuron neuronZ;

  public NamedNeuron NeuronX => neuronX;
  public NamedNeuron NeuronY => neuronY;
  public NamedNeuron NeuronZ => neuronZ;

  public Vector3Neurons(string name)
  {
    neuronX = new NamedNeuron($"{name}_X");
    neuronY = new NamedNeuron($"{name}_Y");
    neuronZ = new NamedNeuron($"{name}_Z");
  }

  public void SetValue(Vector3 value)
  {
    neuronX.Value = value.x;
    neuronY.Value = value.y;
    neuronZ.Value = value.z;
  }
  public Vector3 GetValue()
  {
    return new Vector3(NeuronX.Value, NeuronY.Value, NeuronZ.Value);
  }
}
