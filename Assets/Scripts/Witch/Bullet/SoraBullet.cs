using Enemy;
using Reversible.Unity;
using UnityEngine;

namespace Witch.Bullet {
  public class SoraBullet : WitchBullet {
    public Vector2 velocity;

    protected override void OnStart() {
    }

    protected override void OnForward() {
      var trans = transform;
      var localPosition = trans.localPosition;
      var delta = velocity * Time.deltaTime;
      localPosition.x += delta.x;
      localPosition.y += delta.y;
      trans.localPosition = localPosition;
    }

    public override void OnCollision2D(GameObject other) {
      Destroy();
    }
  }
}