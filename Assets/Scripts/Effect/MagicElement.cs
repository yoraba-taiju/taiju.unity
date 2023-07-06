using Lib.Unity;
using Reversible.Unity;
using Reversible.Value;
using UnityEngine;
using UnityEngine.Serialization;
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
    [SerializeField] public float period = 0.5f;
    [SerializeField] public Vector3 initialVelocity = Vector3.zero;
    private Dense<Vector3> position_;
    private Dense<Vector3> velocity_;
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
      position_ = new Dense<Vector3>(clock, transform.localPosition);
      velocity_ = new Dense<Vector3>(clock, initialVelocity);
    }

    protected override void OnForward() {
      var leftPeriod = period - (IntegrationTime - startFrom_);
      if (leftPeriod < 0) {
        sora_.GetComponent<Sora>().OnMagicElementCollected();
        Deactivate();
      }

      var trans = transform;
      ref var localPosition = ref position_.Mut;
      ref var velocity = ref velocity_.Mut;
      var force = Mover.TrackingForce(
        localPosition, 
        velocity, 
        sora_.localPosition, 
        soraRigidBody_.velocity,
        leftPeriod);
      var dt = Time.deltaTime;
      velocity += force * dt;
      localPosition += velocity * dt;
      trans.localPosition = localPosition;
    }
  }
}
