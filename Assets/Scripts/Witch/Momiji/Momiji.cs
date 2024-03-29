using Reversible;
using UnityEngine;

namespace Witch.Momiji {
  public class Momiji : WitchBehaviour {
    [SerializeField] public GameObject bullet;
    private static readonly Vector2 BulletSpeed = Vector2.right * 60.0f;

    private PlayerInput.PlayingActions actions_;
    private GameObject field_;
    private Rigidbody2D rigidbody_;
    private float toFire_;
    private bool fired_;

    protected override void OnStart() {
      actions_ = Input.Playing;
      rigidbody_ = GetComponent<Rigidbody2D>();
      //playerInput_.Player.Move.performed += context => Debug.Log($"{context.ReadValue<Vector2>()}");
      field_ = GameObject.FindGameObjectWithTag("Field");
      toFire_ = 0.0f;
      fired_ = false;
    }

    private void FixedUpdate() {
      rigidbody_.velocity = actions_.Move.ReadValue<Vector2>() * 15.0f;
    }

    protected override void OnForward() {
      var fire = actions_.Fire;
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
    }

    protected override void OnReverse() {
      toFire_ = 0.0f;
      fired_ = false;
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

    protected override void OnCollision2D(GameObject other) {
      // TODO(ledyba): take some action
      Debug.Log("Sora: damaged");
    }
  }
}