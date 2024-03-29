using UnityEngine;

namespace Witch.Sora {
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

    protected override void OnReverse() {
    }

    protected override void OnLeap() {
    }

    protected override void OnCollision2D(GameObject other) {
      Deactivate();
    }
  }
}