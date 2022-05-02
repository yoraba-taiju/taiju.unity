using UnityEngine;

namespace Util {
  public static class VecUtil {
    public static Vector3 RotateByAngleDeg(Vector3 direction, float angleToRotate) {
      return Quaternion.Euler(0.0f, 0.0f, angleToRotate) * direction;
    }
    public static float AngleDegOf(Vector2 direction) {
      return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
  }
}
