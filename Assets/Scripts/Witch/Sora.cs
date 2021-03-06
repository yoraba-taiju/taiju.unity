using System;
using Reversible;
using UnityEngine;
using Witch.Bullet;

namespace Witch {
  public class Sora : WitchBehaviour {
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject arrowOfLight;
    private static readonly Vector2 BulletSpeed = Vector2.right * 60.0f;

    private PlayerInput.PlayerActions playerActions_;
    private Transform field_;
    private Rigidbody2D rigidbody_;
    private float toFire_;
    private bool fired_;

    protected override void OnStart() {
      playerActions_ = playerInput.Player;
      rigidbody_ = GetComponent<Rigidbody2D>();
      //playerInput_.Player.Move.performed += context => Debug.Log($"{context.ReadValue<Vector2>()}");
      field_ = GameObject.FindGameObjectWithTag("Field").transform;
      toFire_ = 0.0f;
      fired_ = false;
    }

    private void FixedUpdate() {
      rigidbody_.velocity = playerActions_.Move.ReadValue<Vector2>() * 15.0f;
    }

    protected override void OnForward() {
      {
        var trans = transform;
        var pos = trans.localPosition;
        trans.localPosition = new Vector3(
          Math.Clamp(pos.x, -17.5f, 17.5f),
          Math.Clamp(pos.y, -9.5f, 9.5f),
          0.0f
        );
      }
      var fire = playerActions_.Fire;
      if (toFire_ <= 0.0f) {
        if (fire.IsPressed()) {
          if (!fired_) {
            Fire1();
            toFire_ += 120.0f / 1000.0f;
          } else {
            Fire2();
            toFire_ += 50.0f / 1000.0f;
          }
          fired_ = true;
        } else {
          fired_ = false;
        }
      } else {
        toFire_ -= Time.deltaTime;
      }

      var spell = playerActions_.Spell;
      if (spell.triggered) {
        var s = Instantiate(arrowOfLight, field_);
        s.GetComponent<ArrowOfLight>().Track(GameObject.Find("Drone0"));
        s.transform.localPosition = transform.localPosition + Vector3.right * 2f;
      }
    }

    protected override void OnReverse() {
      toFire_ = 0.0f;
      fired_ = false;
    }

    private void Fire1() {
      var b = Instantiate(bullet, field_);
      b.transform.localPosition = transform.localPosition + Vector3.right * 2f;
      b.GetComponent<Rigidbody2D>().velocity = BulletSpeed;
    }
    
    private void Fire2() {
      var b1 = Instantiate(bullet, field_);
      var b2 = Instantiate(bullet, field_);
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
