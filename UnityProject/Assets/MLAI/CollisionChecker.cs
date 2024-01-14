using UnityEngine;
using UnityEngine.Events;

public class CollisionChecker : MonoBehaviour
{
  [System.Serializable]
  public class CollisionEvent : UnityEvent<CollisionChecker, Collision> { }

  public CollisionEvent CollisionEnter => collisionEnter;
  public CollisionEvent CollisionExit => collisionExit;

  public bool Thouching => thouching;

  [SerializeField] private string targetTag = "Ground";

  [SerializeField] private CollisionEvent collisionEnter = new CollisionEvent();
  [SerializeField] private CollisionEvent collisionExit = new CollisionEvent();

  private int countCollisions = 0;
  private bool thouching;

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.transform.CompareTag(targetTag))
    {
      countCollisions++;
      thouching = true;
      collisionEnter?.Invoke(this, collision);
    }
  }

  private void OnCollisionExit(Collision collision)
  {
    if (collision.transform.CompareTag(targetTag))
    {
      countCollisions--;
      thouching = countCollisions > 0;
      collisionExit?.Invoke(this, collision);
    }
  }
}
