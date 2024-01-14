using System;

namespace NeuralNetworks
{
  public class NeuronData
  {
    public readonly static NeuronData Default = new NeuronData(DefaultActivationFunction);
    public readonly static ActivationFunction DefaultActivationFunction = (value) => (float)Math.Tanh(value);

    internal float value;
    internal ActivationFunction activationFunction;

    public string Name;

    public float Value
    {
      get => value;
      set => this.value = value;
    }

    public ActivationFunction ActivationFunction 
    { 
      get => activationFunction ?? DefaultActivationFunction;
      set => activationFunction = value; 
    }

    public NeuronData() : this("None", 0, DefaultActivationFunction) { }
    public NeuronData(string name) : this(name, 0, DefaultActivationFunction) { }
    public NeuronData(ActivationFunction activationFunction) : this("None", 0, activationFunction) { }
    public NeuronData(string name, ActivationFunction activationFunction) : this(name, 0, activationFunction) { }
    public NeuronData(string name, float value, ActivationFunction activationFunction)
    {
      this.Name = name;
      this.value = value;
      this.activationFunction = activationFunction;
    }

    public void SetValueUsingActivator(float value)
    {
      this.value = ActivationFunction.Invoke(value);
    }

    public void Set(NeuronData other)
    {
      value = other.value;
      activationFunction = other.activationFunction;
    }

    public NeuronData Clone()
    {
      return new NeuronData(Name, value, activationFunction);
    }
  }
}
