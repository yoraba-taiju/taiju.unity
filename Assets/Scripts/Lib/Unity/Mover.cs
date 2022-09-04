#nullable enable
using UnityEngine;

namespace Lib.Unity {
  public static class Mover {
    public static Vector2 Follow(in Vector2 targetDirection, in Vector2 desiredVelocity) {
      return targetDirection.normalized * desiredVelocity.magnitude;
    }

    public static Vector2 Follow(Vector2 targetDirection, in Vector2 desiredVelocity, float maxAngle) {
      targetDirection = targetDirection.normalized;
      var speed = desiredVelocity.magnitude;
      var direct = targetDirection * speed;
      var dx = targetDirection.x;
      var dy = targetDirection.y;
      var c = Mathf.Cos(maxAngle * Mathf.Deg2Rad);
      var s = Mathf.Sin(maxAngle * Mathf.Deg2Rad);
      var limited1 = new Vector2(dx * c - s * dy, dx * s + dy * c);
      if (Vector2.Dot(desiredVelocity, direct) >= Vector2.Dot(desiredVelocity, limited1)) {
        return direct;
      }

      s = -s;
      var limited2 = new Vector2(dx * c - s * dy, dx * s + dy * c);
      return Vector2.Dot(targetDirection, limited1) >= Vector2.Dot(targetDirection, limited2) ? limited1 : limited2;
    }

    public static Vector3 TrackingForce(in Vector3 fromPos, in Vector3 fromVel, in Vector3 toPos, in Vector3 toVel,
      float leftPeriod) {
      return 2 * ((toPos - fromPos) + ((toVel - fromVel) * leftPeriod)) / (leftPeriod * leftPeriod);
    }
  }
}