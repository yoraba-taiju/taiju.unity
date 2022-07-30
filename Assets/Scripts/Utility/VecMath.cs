using UnityEngine;

namespace Utility {
  public static class VecMath {
    public static Vector2 Rotate(Vector2 direction, float angleToRotate) {
      var x = direction.x;
      var y = direction.y;
      var c = Mathf.Cos(angleToRotate * Mathf.Deg2Rad);
      var s = Mathf.Sin(angleToRotate * Mathf.Deg2Rad);
      return new Vector2(x * c - s * y, x * s + y * c);
      //return Quaternion.Euler(0.0f, 0.0f, angleToRotate) * direction;
    }
    
    public static float Atan2(float x, float y) {
      return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
    }
    public static float Atan2(Vector2 v) {
      return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
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