using GraphNeuralNetworks;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
  public GraphNeuralNetwork NeuralNetwork => neuralNetwork;

  protected GraphNeuralNetwork neuralNetwork;
  protected GraphNeuralNetworkRunner networkRunner;

  protected virtual void Awake()
  {
    neuralNetwork = new GraphNeuralNetwork();
    networkRunner = new GraphNeuralNetworkRunner(neuralNetwork);
  }

  public virtual void TeleportAgent(Vector3 position, Quaternion rotation)
  {
    transform.SetPositionAndRotation(position, rotation);
  }

  public virtual void ResetAgent()
  {
    foreach (var neuron in neuralNetwork.GetNeurons())
    {
      neuron.Value = 0f;
    }
    networkRunner.ClearQueue();
  }
}
