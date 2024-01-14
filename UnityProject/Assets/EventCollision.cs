using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class EventCollision : MonoBehaviour
{
  [System.Serializable]
  public class CollisionEvent : UnityEvent<Collider, Collision> { }

  public CollisionEvent CollisionEnter => collisionEnter;
  public CollisionEvent CollisionStay => collisionStay;
  public CollisionEvent CollisionExit => collisionExit;

  private new Collider collider;

  [SerializeField]
  private CollisionEvent collisionEnter = new CollisionEvent();
  [SerializeField]
  private CollisionEvent collisionStay = new CollisionEvent();
  [SerializeField]
  private CollisionEvent collisionExit = new CollisionEvent();

  private void Awake()
  {
    collider = GetComponent<Collider>();
  }

  private void OnCollisionEnter(Collision collision)
  {
    collisionEnter.Invoke(collider, collision);
  }

  private void OnCollisionStay(Collision collision)
  {
    collisionStay.Invoke(collider, collision);
  }
  private void OnCollisionExit(Collision collision)
  {
    collisionExit.Invoke(collider, collision);
  }
}
