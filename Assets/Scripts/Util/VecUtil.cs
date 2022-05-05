using UnityEngine;

namespace Util {
  public static class VecUtil {
    public static Vector2 RotateByAngleDegree(Vector2 direction, float angleToRotate) {
      var x = direction.x;
      var y = direction.y;
      var c = Mathf.Cos(angleToRotate);
      var s = Mathf.Sin(angleToRotate);
      return new Vector2(x * c - s * y, x * s + y + c);
      //return Quaternion.Euler(0.0f, 0.0f, angleToRotate) * direction;
    }

    public static Quaternion RotateToTarget(Vector2 from, Vector2 to, float maxAngleDegree) {
      return RotateToTarget(AngleDegreeOf(from), to, maxAngleDegree);
    }
    
    public static Quaternion RotateToTarget(float fromDegree, Vector2 to, float maxAngleDegree) {
      var toDegree = AngleDegreeOf(to);
      var delta = NormalizeAngleDegree(toDegree - fromDegree);
      return Quaternion.Euler(0, 0, Mathf.Clamp(delta, -maxAngleDegree, maxAngleDegree));
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

