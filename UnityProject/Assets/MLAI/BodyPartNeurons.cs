using GraphNeuralNetworks;
using UnityEngine;

public class BodyPartNeurons
{
  private NamedNeuron groundNeuronOutput;

  private Vector3Neurons velocityNeuronOutput;
  private Vector3Neurons angularVelocityNeuronOutput;
  private Vector3Neurons positionNeuronOutput;
  private Vector3Neurons rotationNeuronOutput;
  private Vector3Neurons forceLimitNeuronOutput;

  private Vector3Neurons rotationNeuronInput;
  private Vector3Neurons forceLimitNeuronInput;

  public NamedNeuron GroundNeuronOutput => groundNeuronOutput;

  public Vector3Neurons VelocityNeuronOutput => velocityNeuronOutput;
  public Vector3Neurons AngularVelocityNeuronOutput => angularVelocityNeuronOutput;
  public Vector3Neurons PositionNeuronOutput => positionNeuronOutput;
  public Vector3Neurons RotationNeuronOutput => rotationNeuronOutput;
  public Vector3Neurons ForceLimitNeuronOutput => forceLimitNeuronOutput;

  public Vector3Neurons RotationNeuronInput => rotationNeuronInput;
  public Vector3Neurons ForceLimitNeuronInput => forceLimitNeuronInput;

  public BodyPartNeurons(string name)
  {
    groundNeuronOutput = new NamedNeuron($"{name}_output_ground");

    velocityNeuronOutput = new Vector3Neurons($"{name}_output_velocity");
    angularVelocityNeuronOutput = new Vector3Neurons($"{name}_output_angularVelocity");
    positionNeuronOutput = new Vector3Neurons($"{name}_output_position");
    rotationNeuronOutput = new Vector3Neurons($"{name}_output_rotation");
    forceLimitNeuronOutput = new Vector3Neurons($"{name}_output_forceLimit");

    rotationNeuronInput = new Vector3Neurons($"{name}_input_rotation");
    forceLimitNeuronInput = new Vector3Neurons($"{name}_input_forceLimit");
  }

  public void SetGround(bool ground)
  {
    groundNeuronOutput.Value = ground ? 1f : 0f;
  }

  public void SetVelocity(Vector3 velocity)
  {
    velocityNeuronOutput.SetValue(velocity);
  }
  public void SetAngularVelocity(Vector3 velocity)
  {
    angularVelocityNeuronOutput.SetValue(velocity);
  }
  public void SetPosition(Vector3 position)
  {
    positionNeuronOutput.SetValue(position);
  }
  public void SetRotation(Vector3 rotation)
  {
    rotationNeuronOutput.SetValue(rotation);
  }
  public void SetForceLimit(Vector3 rotation)
  {
    forceLimitNeuronOutput.SetValue(rotation);
  }

  public Vector3 GetRotation()
  {
    return rotationNeuronInput.GetValue();
  }
  public Vector3 GetForceLimit()
  {
    return forceLimitNeuronInput.GetValue();
  }
}
