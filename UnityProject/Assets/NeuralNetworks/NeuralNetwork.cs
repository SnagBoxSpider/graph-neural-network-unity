using System;

namespace NeuralNetworks
{

  public delegate float ActivationFunction(float value);

  [Serializable]
  public class NeuralNetwork
  {
    public static ActivationFunction DefaultActivationFunction { get; } = (value) => (float)Math.Tanh(value);

    public ActivationFunction ActivationFunction
    {
      get { return activationFunction; }
    }
    public float[][][] Weights
    {
      get { return weights; }
    }
    public float[][] Neurons
    {
      get { return neurons; }
    }

    private float[][] neurons;
    private float[][][] weights;
    private ActivationFunction activationFunction;

    public NeuralNetwork() { }

    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
      Copy(copyNetwork);
    }

    public NeuralNetwork(int[] layers)
    {
      Create(layers, DefaultActivationFunction);
    }

    public NeuralNetwork(int[] layers, ActivationFunction activationFunction)
    {
      Create(layers, activationFunction);
    }

    #region Create

    public void Create(int[] layers)
    {
      Create(layers, DefaultActivationFunction);
    }
    public void Create(int[] layers, ActivationFunction activationFunction)
    {
      this.activationFunction = activationFunction;
      CreateNeurons(layers);
      CreateWeights();
      SetRandomWeights();
    }

    private void CreateNeurons(int[] layers)
    {
      neurons = new float[layers.Length][];
      for (int i = 0; i < layers.Length; i++)
        neurons[i] = new float[layers[i]];
    }

    private void CreateWeights()
    {
      weights = new float[neurons.Length][][];

      for (int i = 1; i < neurons.Length; i++)
      {
        int prevLayerNeuronsCount = neurons[i - 1].Length;

        float[][] layerWeights = new float[neurons[i].Length][];
        for (int j = 0; j < layerWeights.Length; j++)
        {
          layerWeights[j] = new float[prevLayerNeuronsCount];
        }

        weights[i] = layerWeights;
      }
    }

    private void SetRandomWeights()
    {
      for (int i = 1; i < weights.Length; i++)
      {
        float[][] layerWeights = weights[i];
        for (int j = 0; j < layerWeights.Length; j++)
        {
          float[] neuronWeights = layerWeights[j];
          for (int k = 0; k < neuronWeights.Length; k++)
          {
            //Рандомно выставляем вес от -0.5 до 0.5
            neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
          }
        }
      }
    }

    #endregion

    #region Copy

    public void Copy(NeuralNetwork copyNetwork)
    {
      this.activationFunction = copyNetwork.activationFunction;
      CopyNeurons(copyNetwork.neurons);
      CopyWeights(copyNetwork.weights);
    }
    private void CopyNeurons(float[][] neurons)
    {
      this.neurons = new float[neurons.Length][];
      for (int i = 0; i < neurons.Length; i++)
      {
        this.neurons[i] = new float[neurons[i].Length];
        for (int j = 0; j < neurons[i].Length; j++)
        {
          this.neurons[i][j] = neurons[i][j];
        }
      }
    }
    private void CopyWeights(float[][][] weights)
    {
      this.weights = new float[weights.Length][][];
      for (int i = 0; i < weights.Length; i++)
      {
        this.weights[i] = new float[weights[i].Length][];
        for (int j = 0; j < weights[i].Length; j++)
        {
          this.weights[i][j] = new float[weights[j].Length];
          for (int k = 0; k < weights[i][j].Length; k++)
          {
            this.weights[i][j][k] = weights[i][j][k];
          }
        }
      }
    }

    #endregion

    public float[] Run(float[] inputs)
    {
      for (int i = 0; i < inputs.Length; i++)
      {
        neurons[0][i] = inputs[i];
      }

      for (int i = 1; i < neurons.Length; i++)
      {
        for (int j = 0; j < neurons[i].Length; j++)
        {
          float value = 0f;

          for (int k = 0; k < neurons[i - 1].Length; k++)
          {
            value += weights[i][j][k] * neurons[i - 1][k];
          }

          //Активация гиперболического тангенса
          neurons[i][j] = (float)Math.Tanh(value);
        }
      }

      return neurons[neurons.Length - 1];
    }

    /// <summary>
    /// Мутация весов
    /// </summary>
    public void Mutate(float probability = 0.2f)
    {
      float min = 1f - probability;
      float max = 1f + probability;

      for (int i = 0; i < weights.Length; i++)
      {
        for (int j = 0; j < weights[i].Length; j++)
        {
          for (int k = 0; k < weights[i][j].Length; k++)
          {
            float weight = weights[i][j][k];

            float factor = UnityEngine.Random.Range(min, max);
            weight *= factor;

            weights[i][j][k] = weight;
          }
        }
      }
    }
  }
}
