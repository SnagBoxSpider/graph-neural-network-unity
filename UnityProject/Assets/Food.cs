using UnityEngine;
using UnityEngine.Events;

public class Food : MonoBehaviour
{
  [System.Serializable]
  public class FoodEvent : UnityEvent<Food>{}

  public float Value
  {
    get { return value; }
  }

  public FoodEvent OnEaten => onEaten;

  [SerializeField, Range(-1, 1)]
  private float value = 0.5f;

  [SerializeField]
  private Color safetyColor = Color.green;
  [SerializeField]
  private Color dangerColor = Color.red;

  [SerializeField]
  private MeshRenderer meshRenderer;

  [SerializeField]
  private FoodEvent onEaten;

  public void Awake()
  {
    meshRenderer.material.color = Color.Lerp(dangerColor, safetyColor, (value + 1));
  }

  public void Eat()
  {
    onEaten?.Invoke(this);
  }
}
