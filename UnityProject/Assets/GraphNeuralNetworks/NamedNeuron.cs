namespace GraphNeuralNetworks
{
  public class NamedNeuron : Neuron
  {
    protected string name;

    public string Name { get => name; }

    public NamedNeuron(string name) : this(name, 0, DefaultActivationFunction) { }
    public NamedNeuron(string name, ActivationFunction activationFunction) : this(name, 0, activationFunction) { }
    public NamedNeuron(string name, float value, ActivationFunction activationFunction) 
      : base(name.GetHashCode(), value, activationFunction)
    {
      this.name = name;
    }
  }
}
