using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Witch.Bullet {
  public class ArrowOfLight: ReversibleBehaviour {
    private Rigidbody2D rigidbody_;
    protected override void OnStart() {
      rigidbody_ = GetComponent<Rigidbody2D>();
      rigidbody_.AddForce(Vector2.right * 7.0f, ForceMode2D.Impulse);
    }

    protected override void OnForward() {
    }
  }
}
