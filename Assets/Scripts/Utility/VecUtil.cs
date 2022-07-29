using UnityEngine;

namespace Utility {
  public static class VecUtil {
    public static Vector2 Rotate(Vector2 direction, float angleToRotate) {
      var x = direction.x;
      var y = direction.y;
      var c = Mathf.Cos(angleToRotate * Mathf.Deg2Rad);
      var s = Mathf.Sin(angleToRotate * Mathf.Deg2Rad);
      return new Vector2(x * c - s * y, x * s + y * c);
      //return Quaternion.Euler(0.0f, 0.0f, angleToRotate) * direction;
    }

    public static Vector2 Follow(Vector2 currentVelocity, Vector2 target) {
      return target.normalized * currentVelocity.magnitude;
    }

    public static Vector2 Follow(Vector2 currentVelocity, Vector2 target, float maxAngle) {
      target = target.normalized;
      var speed = currentVelocity.magnitude;
      var direct = target * speed;
      var tx = target.x;
      var ty = target.y;
      var c = Mathf.Cos(maxAngle * Mathf.Deg2Rad);
      var s = Mathf.Sin(maxAngle * Mathf.Deg2Rad);
      var limited1 = new Vector2(tx * c - s * ty, tx * s + ty * c);
      if (Vector2.Dot(currentVelocity, direct) >= Vector2.Dot(currentVelocity, limited1)) {
        return direct;
      }
      s = -s;
      var limited2 = new Vector2(tx * c - s * ty, tx * s + ty * c);
      return Vector2.Dot(target, limited1) >= Vector2.Dot(target, limited2) ? limited1 : limited2;
    }

    public static float Atan2(float x, float y) {
      return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
    }

    public static float Atan2(Vector2 v) {
      return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public static float DeltaAngle(Vector2 from, Vector2 to, float maxAngle) {
      return Mathf.Clamp(DeltaAngle(from, to), -maxAngle, maxAngle);
    }

    public static float DeltaAngle(float fromDegree, Vector2 to, float maxAngle) {
      return Mathf.Clamp(DeltaAngle(fromDegree, to), -maxAngle, maxAngle);
    }

    public static float DeltaAngle(Vector2 from, Vector2 to) {
      return DeltaAngle(Atan2(from), to);
    }

    public static float DeltaAngle(float fromDegree, Vector2 to) {
      return Normalize(Atan2(to) - fromDegree);
    }

    private static float Normalize(float deg) {
      deg %= 360.0f;
      return deg switch {
        < -180.0f => deg + 360.0f,
        > 180.0f => deg - 360.0f,
        _ => deg
      };
    }
  }
}

