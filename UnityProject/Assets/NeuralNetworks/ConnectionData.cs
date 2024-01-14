namespace NeuralNetworks
{
  public class ConnectionData
  {
    public readonly static ConnectionData Default = new ConnectionData(0);

    private float weight;

    public float Weight { get => weight; set => weight = value; }

    public ConnectionData() : this(0) { }
    public ConnectionData(float weight)
    {
      this.weight = weight;
    }

    public static ConnectionData CreateConnectionRandonWeight(float min = -0.5f, float max = 0.5f)
    {
      return new ConnectionData(UnityEngine.Random.Range(min, max));
    }

    public void Set(ConnectionData other)
    {
      weight = other.weight;
    }

    public ConnectionData Clone()
    {
      return new ConnectionData(weight);
    }
  }
}
