using System.Collections.Generic;
using UnityEngine;

namespace Reversible.Unity.Components {
  public sealed class ReversibleTrail : RawReversibleBehaviour {
    private readonly LinkedList<(float, Vector3)> points_ = new();
    private readonly Vector3[] pointBuffer_ = new Vector3[128];
    private uint bornAt_;
    private float bornTime_;

    private LineRenderer lineRenderer_;
    [SerializeField] public float lifeTime = 1f;
    private const float TimeLimit = Clock.HISTORY_LENGTH * ClockController.SecondPerFrame;

    protected override void OnStart() {
      lineRenderer_ = gameObject.GetComponent<LineRenderer>();
      lineRenderer_.useWorldSpace = true;
      bornTime_ = CurrentTime;
    }

    private void SetPoint(float current, bool setHead) {
      var after = current - lifeTime;
      var node = points_.Last;
      var idx = 0;
      if (setHead) {
        pointBuffer_[0] = transform.position;
        idx++;
      }

      while (node != null && idx < pointBuffer_.Length) {
        var (time, pt) = node.Value;
        if (time < after) {
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

    protected override void OnForward() {
      var trans = transform;
      var currentTime = CurrentTime - bornTime_;
      var currentPosition = trans.position;
      {
        var node = points_.Last;
        if (node != null) {
          var (lastTime, lastPoint) = node.Value;
          if (currentTime - lastTime < 0.01f || (currentPosition - lastPoint).magnitude < 0.05) {
            SetPoint(currentTime, true);
            return;
          }
        }
      }
      points_.AddLast((currentTime, currentPosition));
      {
        var limitTime = currentTime - TimeLimit - lifeTime;
        var node = points_.First;
        while (node != null && node.Value.Item1 <= limitTime) {
          var toRemove = node;
          node = node.Next;
          points_.Remove(toRemove);
        }
      }

      SetPoint(currentTime, false);
    }

    protected override void OnReverse() {
      if (!clockController.Backed) {
        return;
      }
      var currentTime = CurrentTime - bornTime_;
      var node = points_.Last;
      while (node != null && node.Value.Item1 >= currentTime) {
        var toRemove = node;
        node = node.Previous;
        points_.Remove(toRemove);
      }

      //SetPoint(current, true);
      SetPoint(currentTime, false);
    }
  }
}