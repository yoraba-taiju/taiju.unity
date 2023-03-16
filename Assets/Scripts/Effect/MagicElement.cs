using Lib.Unity;
using Reversible.Unity;
using UnityEngine;
using Witch.Sora;

namespace Effect {
  public sealed class MagicElement : ReversibleBehaviour {
    public static readonly Color[] Colors = {
      Color.HSVToRGB(1.0f / 7.0f, 0.3f, 1.0f),
      Color.HSVToRGB(2.0f / 7.0f, 0.3f, 1.0f),
      Color.HSVToRGB(3.0f / 7.0f, 0.3f, 1.0f),
      Color.HSVToRGB(4.0f / 7.0f, 0.3f, 1.0f),
      Color.HSVToRGB(5.0f / 7.0f, 0.3f, 1.0f),
      Color.HSVToRGB(6.0f / 7.0f, 0.3f, 1.0f),
      Color.HSVToRGB(7.0f / 7.0f, 0.3f, 1.0f),
    };
    
    [SerializeField] public Color color = Color.white;
    [SerializeField] public Vector3 velocity;
    [SerializeField] public float period = 0.5f;
    private Transform sora_;
    private Rigidbody2D soraRigidBody_;
    private Transform sprite_;
    private float startFrom_;
    protected override void OnStart() {
      var trans = transform;
      sora_ = GameObject.FindGameObjectWithTag("Player").transform;
      soraRigidBody_ = sora_.GetComponent<Rigidbody2D>();
      sprite_ = trans.Find("Sprite");
      var spriteRenderer = sprite_.GetComponent<SpriteRenderer>();
      spriteRenderer.color = color;
      startFrom_ = IntegrationTime;
    }

    protected override void OnForward() {
      var leftPeriod = period - (IntegrationTime - startFrom_);
      if (leftPeriod < 0) {
        sora_.GetComponent<Sora>().OnMagicElementCollected();
        Deactivate();
      }

      var trans = transform;
      var localPosition = trans.localPosition;
      var force = Mover.TrackingForce(
        localPosition, 
        velocity, 
        sora_.localPosition, 
        soraRigidBody_.velocity,
        leftPeriod);
      var dt = Time.deltaTime;
      velocity += force * dt;
      trans.localPosition = localPosition + velocity * dt;
    }
    protected override void OnReverse() {
      velocity = Vector3.zero;
    }
  }
}
