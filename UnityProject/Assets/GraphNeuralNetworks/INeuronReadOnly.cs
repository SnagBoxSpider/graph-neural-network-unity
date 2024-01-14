namespace GraphNeuralNetworks
{
  public interface INeuronReadOnly
  {
    public GraphNeuralNetwork Parent { get; }
    public int Id { get; }
    public float Value { get; }
    public ActivationFunction ActivationFunction { get; }

  }
}
