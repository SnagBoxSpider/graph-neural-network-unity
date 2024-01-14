namespace GraphNeuralNetworks
{
  public interface IConnectionReadOnly
  {
    public GraphNeuralNetwork Parent { get; }
    public int Id { get; }
    public float Weight { get; set; }
  }
}
