using System.Collections.Generic;
using Reversible.Value;
using UnityEngine;

namespace Reversible.Unity {
  public sealed class MakeLineRendererAsReversibleTrail : RawReversibleBehaviour {
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
      lineRenderer_.useWorldSpace = true;
    }

    private void SetPoint(float current, bool setHead) {
      var limit = current - lifeTime;
      var node = points_.Last;
      if (setHead) {
        pointBuffer_[0] = transform.position;
      }
      var idx = setHead ? 1 : 0;
      while (node != null && idx < pointBuffer_.Length) {
        var (time, pt) = node.Value;
        if (time < limit) {
          break;
        }
        pointBuffer_[idx] = pt;
        node = node.Previous;
        idx++;
      }

      var nextPoints = pointBuffer_[..idx];
      lineRenderer_.positionCount = idx;
      lineRenderer_.SetPositions(nextPoints);
    }


    public new void Update() {
      if (clockHolder.IsLeaping) {
        OnReverse();
      } else {
        OnForward();
      }
    }

    private void OnForward() {
      ref var current = ref current_.Mut;
      current += Time.deltaTime;

      var trans = transform;
      var pos = trans.position;
      var last = points_.Last;
      if (last != null) {
        var (lastTime, lastPoint) = last.Value;
        if ((current - lastTime) < 0.01f || (pos - lastPoint).magnitude < 0.05) {
          SetPoint(current, true);
          return;
        }
      }
      points_.AddLast((current, pos));
      var limitTime = current - TimeLimit - lifeTime;
      var node = points_.First;
      while (node != null && node.Value.Item1 <= limitTime) {
        var toRemove = node;
        node = node.Next;
        points_.Remove(toRemove);
      }
      SetPoint(current, false);
    }

    private void OnReverse() {
      var current = current_.Ref;
      var node = points_.Last;
      while (node != null && node.Value.Item1 >= current) {
        var toRemove = node;
        node = node.Previous;
        points_.Remove(toRemove);
      }
      SetPoint(current, true);
    }

    protected override void OnTick() {}
    protected override void OnBack() {}
    protected override void OnLeap() {}
  }
}