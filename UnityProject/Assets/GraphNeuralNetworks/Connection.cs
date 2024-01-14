namespace GraphNeuralNetworks
{
  public class Connection : IConnectionReadOnly
  {
    internal GraphNeuralNetwork parent = null;
    internal int id = -1;

    protected float weight;

    public GraphNeuralNetwork Parent { get => parent; }
    public int Id { get => id; }

    public float Weight { get => weight; set => weight = value; }

    public Connection() : this(0) { }
    public Connection(float weight)
    {
      this.weight = weight;
    }

    internal void Invalidate()
    {
      parent = null;
      id = -1;
    }
  }
}
