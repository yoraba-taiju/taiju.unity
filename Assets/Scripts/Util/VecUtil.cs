using UnityEngine;

namespace Util {
  public static class VecUtil {
    public static Vector3 RotateByAngleDeg(Vector3 direction, float angleToRotate) {
      return Quaternion.Euler(0.0f, 0.0f, angleToRotate) * direction;
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

