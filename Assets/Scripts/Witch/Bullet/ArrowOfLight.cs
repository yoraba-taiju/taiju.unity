using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Witch.Bullet {
  public class ArrowOfLight: ReversibleBehaviour {
    private Rigidbody2D rigidbody_;
    private Transform body_;
    [SerializeField] private float bodyRotationSpeed = 360.0f;
    private Dense<float> bodyRotation_;
    protected override void OnStart() {
      bodyRotation_ = new Dense<float>(clock, 0.0f);
      rigidbody_ = GetComponent<Rigidbody2D>();
      rigidbody_.AddForce(Vector2.right * 7.0f, ForceMode2D.Impulse);
      body_ = transform.Find("Body");
    }

    protected override void OnForward() {
      var dt = Time.deltaTime;
      ref var bodyRotation = ref bodyRotation_.Mut;
      bodyRotation += bodyRotationSpeed * dt;
      body_.rotation = Quaternion.Euler(bodyRotation, 0, 0);
    }
    protected override void OnReverse() {
      ref readonly var bodyRotation = ref bodyRotation_.Ref;
      body_.rotation = Quaternion.Euler(bodyRotation, 0, 0);
    }
  }
}
