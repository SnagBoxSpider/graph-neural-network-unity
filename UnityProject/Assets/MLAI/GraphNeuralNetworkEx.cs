using GraphNeuralNetworks;

public static class GraphNeuralNetworkEx
{
  public static bool AddNeurons(this GraphNeuralNetwork graphNeuralNetwork, QuaternionNeurons quaternionNeurons)
  {
    return graphNeuralNetwork.AddNeuron(quaternionNeurons.NeuronW) && 
      graphNeuralNetwork.AddNeuron(quaternionNeurons.NeuronX) &&
      graphNeuralNetwork.AddNeuron(quaternionNeurons.NeuronY) &&
      graphNeuralNetwork.AddNeuron(quaternionNeurons.NeuronZ);
  }
  public static bool AddNeurons(this GraphNeuralNetwork graphNeuralNetwork, Vector3Neurons vector3Neurons)
  {
    return graphNeuralNetwork.AddNeuron(vector3Neurons.NeuronX) && 
      graphNeuralNetwork.AddNeuron(vector3Neurons.NeuronY) && 
      graphNeuralNetwork.AddNeuron(vector3Neurons.NeuronZ);
  }
  public static bool AddNeurons(this GraphNeuralNetwork graphNeuralNetwork, BodyPartNeurons bodyPartNeurons)
  {
    return graphNeuralNetwork.AddNeuron(bodyPartNeurons.GroundNeuronOutput) &&
      graphNeuralNetwork.AddNeurons(bodyPartNeurons.VelocityNeuronOutput) &&
      graphNeuralNetwork.AddNeurons(bodyPartNeurons.AngularVelocityNeuronOutput) &&
      graphNeuralNetwork.AddNeurons(bodyPartNeurons.PositionNeuronOutput) &&
      graphNeuralNetwork.AddNeurons(bodyPartNeurons.RotationNeuronOutput) &&
      graphNeuralNetwork.AddNeurons(bodyPartNeurons.ForceLimitNeuronOutput) &&
      graphNeuralNetwork.AddNeurons(bodyPartNeurons.RotationNeuronInput) &&
      graphNeuralNetwork.AddNeurons(bodyPartNeurons.ForceLimitNeuronInput);
  }



  public static void Mutate(this GraphNeuralNetwork graphNeuralNetwork, GraphNeuralNetwork otherNetwork, float chance)
  {
    foreach (var connection in graphNeuralNetwork.GetConnections())
    {
      if (UnityEngine.Random.value >= chance)
        continue;

      connection.Weight = otherNetwork.GetConnection(connection.Id).Weight;
    }
  }

  public static void MutateWeights(this GraphNeuralNetwork graphNeuralNetwork, float chance, float rate)
  {
    foreach (var connection in graphNeuralNetwork.GetConnections())
    {
      if (UnityEngine.Random.value >= chance)
        continue;

      connection.Weight += UnityEngine.Random.Range(-rate, rate);
    }
  }
}
