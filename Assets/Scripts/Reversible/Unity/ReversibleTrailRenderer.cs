using System.Collections.Generic;
using Reversible.Value;
using UnityEngine;

namespace Reversible.Unity {
  public sealed class ReversibleTrailRenderer : RawReversibleBehaviour {
    private Dense<float> current_;
    private readonly LinkedList<(float, Vector3)> points_ = new();
    private readonly Vector3[] pointBuffer_ = new Vector3[128];
    private uint bornAt_;

    private LineRenderer lineRenderer_;
    [SerializeField] public float lifeTime = 1f;
    private const float TimeLimit = Clock.HISTORY_LENGTH * ClockHolder.SecondPerFrame;

    protected override void OnStart() {
      current_ = new Dense<float>(clock, 0.0f);
      lineRenderer_ = gameObject.GetComponent<LineRenderer>();
    }

    private void SetPoint() {
      ref readonly var current = ref current_.Ref;
      var limit = current - lifeTime;
      var node = points_.Last;
      var idx = pointBuffer_.Length;
      while (node != null && idx > 0) {
        var (time, pt) = node.Value;
        if (limit < time) {
          break;
        }
        idx--;
        pointBuffer_[idx] = pt;
        node = node.Previous;
      }
      lineRenderer_.SetPositions(pointBuffer_[idx..]);
    }

    protected override void OnTick() {
      var trans = transform;
      var dt = Time.deltaTime;
      ref var current = ref current_.Mut;
      current += dt;
      points_.AddLast((lifeTime, trans.localPosition));
      var limitTime = current - TimeLimit - lifeTime;
      while (points_.First.Value.Item1 <= limitTime) {
        points_.RemoveFirst();
      }
      SetPoint();
    }

    protected override void OnBack() {
      ref readonly var current = ref current_.Ref;
      while (points_.Last.Value.Item1 < current) {
        points_.RemoveLast();
      }
      SetPoint();
    }

    protected override void OnLeap() {}
  }
}