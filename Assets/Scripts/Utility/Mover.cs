using UnityEngine;

namespace Utility {
  public static class Mover {
    public static Vector2 Follow(Vector2 target, Vector2 currentVelocity) {
      return target.normalized * currentVelocity.magnitude;
    }

    public static Vector2 Follow(Vector2 target, Vector2 currentVelocity, float maxAngle) {
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
    
  }
}

