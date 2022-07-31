using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Witch.Bullet {
  public class ArrowOfLight: ReversibleBehaviour {
    [SerializeField] private GameObject arrow;
    private Dense<Vector2> direction_;
    protected override void OnStart() {
      direction_.Mut = Quaternion.Euler(0, 0, Random.Range(-180, 180)) * Vector2.left;
    }

    protected override void OnForward() {
      ref var direction = ref direction_.Mut;
      arrow.transform.localPosition += (Vector3)direction * 20.0f;
    }
  }
}
