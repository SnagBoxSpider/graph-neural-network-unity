using UnityEngine;
using UnityEngine.Events;

public class Vision : MonoBehaviour
{
  public Vector2Int Size => size;
  public Color[] Colors => tex.GetPixels();

  public UnityEvent DataChanged => dataChanged;

  [SerializeField]
  private Vector2Int size = new Vector2Int(15, 10);

  [SerializeField]
  private new Camera camera;

  [SerializeField]
  private Texture2D tex;

  [SerializeField]
  private UnityEvent dataChanged = new UnityEvent();

  private Rect rect;
  private RenderTexture renderTexture;

  private void Awake()
  {
    tex = new Texture2D(size.x, size.y); 
    rect = new Rect(0, 0, size.x, size.y);
    renderTexture = new RenderTexture(size.x, size.y, 16);

    camera.targetTexture = renderTexture;
    camera.forceIntoRenderTexture = true;
  }

  private void Update()
  {
    RenderTexture.active = renderTexture;
    tex.ReadPixels(rect, 0, 0);
    tex.Apply();
    RenderTexture.active = null;

    dataChanged.Invoke();
  }
}
