using System;
using UnityEngine;
using Reversible.Unity;

namespace Witch.Motion {
  public class SoraBullet : ReversibleBehaviour {

    protected override void OnStart() {
    }

    protected override void OnForward() {
    }

    private void OnCollisionEnter2D(Collision2D other) {
      Destroy();
    }

    private void OnCollisionStay2D(Collision2D other) {
      Destroy();
    }
  }
}
