using UnityEngine;

namespace Utility {
  public static class Mover {
    public static Vector2 Follow(Vector2 targetDirection, Vector2 currentVelocity) {
      return targetDirection.normalized * currentVelocity.magnitude;
    }

    public static Vector2 Follow(Vector2 targetDirection, Vector2 currentVelocity, float maxAngle) {
      targetDirection = targetDirection.normalized;
      var speed = currentVelocity.magnitude;
      var direct = targetDirection * speed;
      var dx = targetDirection.x;
      var dy = targetDirection.y;
      var c = Mathf.Cos(maxAngle * Mathf.Deg2Rad);
      var s = Mathf.Sin(maxAngle * Mathf.Deg2Rad);
      var limited1 = new Vector2(dx * c - s * dy, dx * s + dy * c);
      if (Vector2.Dot(currentVelocity, direct) >= Vector2.Dot(currentVelocity, limited1)) {
        return direct;
      }
      s = -s;
      var limited2 = new Vector2(dx * c - s * dy, dx * s + dy * c);
      return Vector2.Dot(targetDirection, limited1) >= Vector2.Dot(targetDirection, limited2) ? limited1 : limited2;
    }
  }
}

