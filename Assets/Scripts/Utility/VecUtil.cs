using UnityEngine;

namespace Utility {
  public static class VecUtil {
    public static Vector2 RotateByAngleDegree(Vector2 direction, float angleToRotate) {
      var x = direction.x;
      var y = direction.y;
      var c = Mathf.Cos(angleToRotate * Mathf.Deg2Rad);
      var s = Mathf.Sin(angleToRotate * Mathf.Deg2Rad);
      return new Vector2(x * c - s * y, x * s + y * c);
      //return Quaternion.Euler(0.0f, 0.0f, angleToRotate) * direction;
    }

    public static float DeltaDegreeToTarget(Vector2 from, Vector2 to, float maxAngleDegree) {
      return Mathf.Clamp(DeltaDegreeToTarget(from, to), -maxAngleDegree, maxAngleDegree);
    }

    public static float DeltaDegreeToTarget(float fromDegree, Vector2 to, float maxAngleDegree) {
      return Mathf.Clamp(DeltaDegreeToTarget(fromDegree, to), -maxAngleDegree, maxAngleDegree);
    }

    public static float DeltaDegreeToTarget(Vector2 from, Vector2 to) {
      return DeltaDegreeToTarget(AngleDegreeOf(from), to);
    }
    public static float DeltaDegreeToTarget(float fromDegree, Vector2 to) {
      var toDegree = AngleDegreeOf(to);
      var delta = NormalizeAngleDegree(toDegree - fromDegree);
      return delta;
    }

    public static float AngleDegreeOf(Vector2 direction) {
      return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    public static float NormalizeAngleDegree(float deg) {
      deg %= 360.0f;
      return deg switch {
        < -180.0f => deg + 360.0f,
        > 180.0f => deg - 360.0f,
        _ => deg
      };
    }
  }
}

