using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class TrainingField<TAgent> : MonoBehaviour where TAgent : Agent
{
  public TAgent Agent => agent;

  public bool IsStarted => isStarted;

  public int Episode => episode;
  public int MaxEpisodes => maxEpisodes;

  public float EpisodeTime => episodeTime;
  public float MaxEpisodeTime => maxEpisodeTime;

  public float Reward
  {
    get => reward;
    set
    {
      if (reward == value)
        return;

      reward = value;
    }
  }
  public float BestReward => bestReward;

  [Space]
  [SerializeField] private TAgent agent;

  [Space, Header("Episodes")]
  [SerializeField] protected int maxEpisodes = 100000;
  [SerializeField] protected float maxEpisodeTime = 30f;

  [Space, Header("Mutation")]
  [SerializeField] protected float chance = 0.15f;
  [SerializeField] protected float rate = 0.05f;

  [Space, Header("Current Stats")]
  [SerializeField] private int episode = 0;
  [SerializeField] private float episodeTime = 0;
  [SerializeField] private float reward = 0;

  [Space, Header("Best Stats")]
  [SerializeField] private float bestReward = float.NegativeInfinity;

  private Dictionary<int, float> bestNeuralNetworkWeights = new Dictionary<int, float>();

  private bool isStarted = false;

  protected void Awake()
  {
    StartTraining();
  }

  protected void FixedUpdate()
  {
    if (!isStarted)
      return;

    episodeTime += Time.deltaTime;
    try
    {
      OnEpisodeUpdate();
    }
    catch (Exception e) 
    {
      Debug.LogException(e);
    }

    if (CanNextEpisode())
    {
      NextEpisode();
    }
  }

  protected abstract TAgent CreateAgent();

  public void StartTraining()
  {
    if (isStarted)
      return;

    if (agent)
      Destroy(agent.gameObject);

    agent = CreateAgent();

    episode = 0;
    reward = 0;
    episodeTime = 0;

    isStarted = true;

    OnStartTraining();
    NextEpisode();
  }
  public void EndTraining()
  {
    if (!isStarted)
      return;

    isStarted = false;

    OnEndTraining();
  }

  protected void NextEpisode()
  {
    if (!isStarted)
      return;

    OnEpisodeEnd();

    if (reward > bestReward)
    {
      bestReward = reward;
      foreach (var connection in agent.NeuralNetwork.GetConnections())
      {
        bestNeuralNetworkWeights[connection.Id] = connection.Weight;
      }
    }
    //Save
    SaveToFile();

    episode++;
    reward = 0;
    episodeTime = 0;

    if (episode >= maxEpisodes)
    {
      EndTraining();
      return;
    }

    if (agent)
      Destroy(agent.gameObject);

    agent = CreateAgent();

    foreach (var weight in bestNeuralNetworkWeights)
    {
      agent.NeuralNetwork.GetConnection(weight.Key).Weight = weight.Value;
    }
    agent.NeuralNetwork.MutateWeights(chance, rate);

    OnEpisodeBegin();
  }


  protected virtual void OnStartTraining()
  {

  }

  protected virtual bool CanNextEpisode()
  {
    return maxEpisodeTime <= episodeTime;
  }

  protected virtual void OnEpisodeEnd()
  {

  }
  protected virtual void OnEpisodeUpdate()
  {

  }

  protected virtual void OnEpisodeBegin()
  {

  }


  protected virtual void OnEndTraining()
  {

  }


  [ContextMenu("Save To File")]
  public void SaveToFile()
  {
    try
    {
      SaveData data = new SaveData()
      {
        episode = episode,

        bestReward = bestReward,
        bestNeuralNetworkWeights = bestNeuralNetworkWeights
      };

      string json = JsonConvert.SerializeObject(data);
      File.WriteAllText($"{Application.dataPath}/TrainingField.txt", json);
    }
    catch (Exception e)
    {
      Debug.LogError("SaveToFile: " + e.Message);
    }
  }
  [ContextMenu("Load From File")]
  public void LoadFromFile()
  {
    try
    {
      if (!File.Exists(Application.dataPath))
        return;

      string json = File.ReadAllText($"{Application.dataPath}/TrainingField.txt");
      SaveData data = JsonConvert.DeserializeObject<SaveData>(json);

      episode = data.episode;
      bestReward = data.bestReward;
      bestNeuralNetworkWeights = data.bestNeuralNetworkWeights;
    }
    catch (Exception e)
    {
      Debug.LogError("LoadFromFile: " + e.Message);
    }
  }

  [System.Serializable]
  private class SaveData
  {
    public int episode = 0;
    public float bestReward;
    public Dictionary<int, float> bestNeuralNetworkWeights;
  }
}
