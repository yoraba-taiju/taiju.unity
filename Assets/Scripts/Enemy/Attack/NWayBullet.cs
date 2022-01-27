using Donut.Unity;
using Enemy.Bullet;
using UnityEngine;

namespace Enemy.Attack {
  public class NWayBullet: DonutBehaviour {
    [SerializeField] public float speed = 10.0f;
    [SerializeField] public float angle = 5.0f;
    [SerializeField] public uint clockInterval = 3;
    [SerializeField] public GameObject bulletForOdd;
    [SerializeField] public uint numBulletForOdd = 3;
    [SerializeField] public GameObject bulletForEven;
    [SerializeField] public uint numBulletForEven = 4;
    private uint bornAt_;
    private uint lastTick_;

    protected override void OnStart() {
      bornAt_ = clock.CurrentTick;
      lastTick_ = clock.CurrentTick;
    }

    protected override void OnUpdate() {
      var currentTick = clock.CurrentTick;
      if (currentTick == lastTick_) {
        return;
      }
      lastTick_ = currentTick;

      var t = currentTick - bornAt_;
      if (t == 0 || (t % clockInterval) > 0) {
        return;
      }
      if ((t / clockInterval) % 2 == 0) {
        for (var i = 0; i < numBulletForEven; ++i) {
          var obj = Instantiate(bulletForEven, transform);
          obj.gameObject.gameObject.name = "EvenBullet";
          var aim = obj.GetComponent<FixedSpeedAim>();
          aim.speed = speed;
          if (i % 2 == 0) {
            aim.angle = angle * (i >> 1) + (angle/2);
          } else {
            aim.angle = -angle * (i >> 1) - (angle/2);
          }
        }
      } else {
        for (var i = 0; i < numBulletForOdd; ++i) {
          var obj = Instantiate(bulletForOdd, transform);
          obj.gameObject.gameObject.name = "OddBullet";
          var aim = obj.GetComponent<FixedSpeedAim>();
          aim.speed = speed;
          if (i % 2 == 0) {
            aim.angle = angle * ((i+1) >> 1);
          } else {
            aim.angle = -angle * ((i+1) >> 1);
          }
        }
      }
    }

    private void Fire(GameObject prefab, uint num, float offset) {
    }
  }
}