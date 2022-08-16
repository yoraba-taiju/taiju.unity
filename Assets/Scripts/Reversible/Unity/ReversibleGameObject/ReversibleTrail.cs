using Lib;
using Reversible.Value;
using UnityEngine;

namespace Reversible.Unity.ReversibleGameObject {
  public sealed class ReversibleTrail : ComponentBase {
    private readonly Vector3[] pointBuffer_ = new Vector3[128];
    private Dense<float> currentTime_;
    private RingBuffer<float> times_ = new(16384);
    private RingBuffer<Vector3> points_ = new(16384);

    private LineRenderer lineRenderer_;
    [SerializeField] public float lifeTime = 1f;
    private const float TimeLimit = Clock.HISTORY_LENGTH * Player.SecondPerFrame;

    protected override void OnStart() {
      lineRenderer_ = gameObject.GetComponent<LineRenderer>();
      lineRenderer_.useWorldSpace = true;
      currentTime_ = new Dense<float>(clock, 0.0f);
    }

    private void SetPoint(float current, bool setHead) {
      var after = current - lifeTime;
      var idx = times_.Count - 1;
      var buffIdx = 0;
      if (setHead) {
        pointBuffer_[0] = transform.position;
        buffIdx++;
      }

      while (idx >= 0 && buffIdx < pointBuffer_.Length) {
        var time = times_[idx];
        if (time < after) {
          break;
        }
        pointBuffer_[buffIdx] = points_[idx];
        buffIdx++;
        idx--;
      }

      var nextPoints = pointBuffer_[..buffIdx];
      lineRenderer_.positionCount = buffIdx;
      lineRenderer_.SetPositions(nextPoints);
    }

    protected override void OnForward() {
      ref var currentTime = ref currentTime_.Mut;
      currentTime += Time.deltaTime;
      var trans = transform;
      var currentPosition = trans.position;
      {
        if (!times_.IsEmpty) {
          var lastPoint = points_.Last;
          var lastTime = times_.Last;
          if (currentTime - lastTime <= 0.01f || (currentPosition - lastPoint).magnitude <= 0.05) {
            SetPoint(currentTime, true);
            return;
          }
        }
      }
      points_.AddLast(currentPosition);
      times_.AddLast(currentTime);
      {
        var limitTime = currentTime - TimeLimit - lifeTime;
        while (times_.IsEmpty) {
          var pointTime = times_.First;
          if (pointTime > limitTime) {
            break;
          }
          times_.RemoveFirst();
          points_.RemoveFirst();
        }
      }

      SetPoint(currentTime, false);
    }

    protected override void OnReverse() {
      if (!player.Backed) {
        return;
      }
      var currentTime = currentTime_.Ref;
      while (!times_.IsEmpty) {
        var pointTime = times_.Last;
        if (pointTime <= currentTime) {
          break;
        }

        times_.RemoveLast();
        points_.RemoveLast();
      }
      SetPoint(currentTime, false);
    }
  }
}