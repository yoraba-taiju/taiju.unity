using Enemy.Bullet;
using Reversible.Unity;
using UnityEngine;

namespace Enemy.Attack {
  public class NWayBullet: ReversibleBehaviour {
    [SerializeField] public float speed = 10.0f;
    [SerializeField] public float angle = 5.0f;
    [SerializeField] public uint clockInterval = 10;
    [SerializeField] public GameObject bulletForOdd;
    [SerializeField] public uint numBulletForOdd = 3;
    [SerializeField] public GameObject bulletForEven;
    [SerializeField] public uint numBulletForEven = 4;
    private uint bornAt_;
    private uint lastTick_;
    private GameObject sora_;
    private bool soraMissing_;

    protected override void OnStart() {
      bornAt_ = clock.CurrentTick;
      lastTick_ = clock.CurrentTick;
      sora_ = GameObject.FindWithTag("Player");
      soraMissing_ = sora_ == null;
    }

    protected override void OnForward() {
      if (!clockHolder.Ticked) {
        return;
      }
      var currentTick = clock.CurrentTick;
      if (currentTick == lastTick_) {
        return;
      }
      lastTick_ = currentTick;
      var t = currentTick - bornAt_;
      if (t == 0 || (t % clockInterval) != 0) {
        return;
      }
      if ((t / clockInterval) % 2 == 0) {
        for (var i = 0; i < numBulletForEven; ++i) {
          var obj = Instantiate(bulletForEven, transform);
          obj.gameObject.gameObject.name = "EvenBullet";
          var aim = obj.GetComponent<FixedSpeedBullet>();
          if (soraMissing_) {
            aim.Velocity = Vector2.left * speed;
            return;
          }
          var vec = (sora_.transform.localPosition - transform.localPosition).normalized * speed;
          var angleToRotate = (i % 2 == 0) ? angle * (i >> 1) + (angle / 2) : -angle * (i >> 1) - (angle / 2);
          aim.Velocity = Quaternion.AngleAxis(angleToRotate, Vector3.forward) * vec;
        }
      } else {
        for (var i = 0; i < numBulletForOdd; ++i) {
          var obj = Instantiate(bulletForOdd, transform);
          obj.gameObject.gameObject.name = "OddBullet";
          var aim = obj.GetComponent<FixedSpeedBullet>();
          var vec = (sora_.transform.localPosition - transform.localPosition).normalized * speed;
          var angleToRotate = (i % 2 == 0) ? angle * ((i+1) >> 1) : -angle * ((i+1) >> 1);
          aim.Velocity = Quaternion.AngleAxis(angleToRotate, Vector3.forward) * vec;
        }
      }
    }
  }
}
