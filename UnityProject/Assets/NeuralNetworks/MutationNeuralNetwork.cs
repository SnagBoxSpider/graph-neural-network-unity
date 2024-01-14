using System.Linq;

namespace NeuralNetworks
{
  public class MutationNeuralNetwork : CustomNeuralNetwork
  {
    public MutationNeuralNetwork() : base(0) { }
    public MutationNeuralNetwork(int neuronsCapacity) : base(neuronsCapacity) { }
    public MutationNeuralNetwork(CustomNeuralNetwork neuralNetwork) : base(neuralNetwork) { }

    public MutationNeuralNetwork(MutationNeuralNetwork a, MutationNeuralNetwork b, float rate) : base(a)
    {
      Mutate(b, rate);
    }

    public void ClearNeuronValues()
    {
      foreach (var neuronId in neurons.Keys)
      {
          neurons[neuronId].Value = 0;
      }
    }

    public void SetWeights(float[] weights)
    {
      int i = 0;
      foreach (var connectionid in GetAllConnectionIds())
      {
        var connection = GetConnection(connectionid);
        connection.Weight = weights[i];
        i++;
      }
    }

    public void SetRandomWeights(float min = -0.2f, float max = 0.2f)
    {
      foreach (var connectionid in GetAllConnectionIds())
      {
        var connection = GetConnection(connectionid);
        connection.Weight = UnityEngine.Random.Range(min, max);
      }
    }

    public void Mutate(MutationNeuralNetwork otherNetwork, float rate)
    {
      foreach (var neuronId in otherNetwork.neurons.Keys)
      {
        if(UnityEngine.Random.value < rate)
          neurons[neuronId].Set(otherNetwork.neurons[neuronId]);
      }
      foreach (var connectionId in otherNetwork.connections.Keys)
      {
        if (UnityEngine.Random.value < rate)
          connections[connectionId].Set(otherNetwork.connections[connectionId]);
      }
    }
    public void MutateWeights(float chance, float rate)
    {
      foreach (var connectionid in GetAllConnectionIds())
      {
        if (UnityEngine.Random.value >= chance)
          continue;
        
        var connection = GetConnection(connectionid);
        connection.Weight += UnityEngine.Random.Range(-rate, rate);
      }
    }
  }
}
