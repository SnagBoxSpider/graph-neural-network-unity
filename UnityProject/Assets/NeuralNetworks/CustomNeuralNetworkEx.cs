namespace NeuralNetworks
{
  public static class CustomNeuralNetworkEx
  {
    public static void AddAllConnections(this CustomNeuralNetwork neuralNetwork, int input, int[] output)
    {
      for (int j = 0; j < output.Length; j++)
      {
        neuralNetwork.AddConnection(input, output[j]);
      }
    }
    public static void AddAllConnections(this CustomNeuralNetwork neuralNetwork, int[] input, int output)
    {
      for (int i = 0; i < input.Length; i++)
      {
        neuralNetwork.AddConnection(input[i], output);
      }
    }
    public static void AddAllConnections(this CustomNeuralNetwork neuralNetwork, int[] input, int[] output)
    {
      for (int i = 0; i < input.Length; i++)
      {
        for (int j = 0; j < output.Length; j++)
        {
          neuralNetwork.AddConnection(input[i], output[j]);
        }
      }
    }
    public static void AddConnectionWithAttenuation(this CustomNeuralNetwork neuralNetwork, int input, int output, ConnectionData connectionData)
    {
      neuralNetwork.AddConnection(input, output, connectionData);

      var neuronAttenuationId = neuralNetwork.AddNeuron(new NeuronData($"Attenuation (Hidden)"));
      neuralNetwork.AddConnection(input, neuronAttenuationId, new ConnectionData(0.25f));
      neuralNetwork.AddConnection(neuronAttenuationId, neuronAttenuationId, new ConnectionData(0.75f));
      neuralNetwork.AddConnection(neuronAttenuationId, output, new ConnectionData(-1f));
    }
  }
}
