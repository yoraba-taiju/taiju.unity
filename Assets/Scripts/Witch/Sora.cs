using System;
using UnityEngine;
using Reversible.Unity;

namespace Witch {
  public class Sora : WitchBehaviour {
    [SerializeField] public GameObject bullet;

    private GameObject field_;
    private Rigidbody2D rigidbody_;
    private float toFire_;
    private static readonly Vector2 BulletSpeed = Vector2.right * 60.0f;

    protected override void OnStart() {
      rigidbody_ = GetComponent<Rigidbody2D>();
      //playerInput_.Player.Move.performed += context => Debug.Log($"{context.ReadValue<Vector2>()}");
      field_ = GameObject.FindGameObjectWithTag("Field");
      toFire_ = 0.0f;
    }

    private void FixedUpdate() {
      var player = playerInput.Player;
      rigidbody_.velocity = player.Move.ReadValue<Vector2>() * 15.0f;
    }

    protected override void OnForward() {
      var fire = playerInput.Player.Fire;
      if (toFire_ <= 0.0f) {
        if (fire.IsPressed()) {
          if (fire.WasPressedThisFrame()) {
            Fire1();
            toFire_ += 120.0f / 1000.0f;
          } else {
            Fire2();
            toFire_ += 50.0f / 1000.0f;
          }
        }
      } else {
        toFire_ -= Time.deltaTime;
      }
    }

    protected override void OnReverse() {
      toFire_ = 0.0f;
    }

    private void Fire1() {
      var b = Instantiate(bullet, field_.transform);
      var trans = b.transform;
      trans.localPosition = transform.localPosition + Vector3.right * 2f;
      b.GetComponent<Rigidbody2D>().velocity = BulletSpeed;
    }
    
    private void Fire2() {
      var b1 = Instantiate(bullet, field_.transform);
      var b2 = Instantiate(bullet, field_.transform);
      var pos = transform.localPosition;
      b1.transform.localPosition = pos + Vector3.right * 2f + Vector3.up * 0.5f;
      b2.transform.localPosition = pos + Vector3.right * 2f + Vector3.down * 0.5f;
      b1.GetComponent<Rigidbody2D>().velocity = BulletSpeed;
      b2.GetComponent<Rigidbody2D>().velocity = BulletSpeed;
    }

    protected override void OnCollide(GameObject other) {
      // TODO(ledyba): take some action
      Debug.Log("Sora: damaged");
    }
  }
}
