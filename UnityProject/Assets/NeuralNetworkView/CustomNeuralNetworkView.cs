using GraphNeuralNetworks;
using UnityEngine;
public abstract class CustomNeuralNetworkView : MonoBehaviour
{
  public abstract GraphNeuralNetwork NeuralNetwork { get; set; }
}
