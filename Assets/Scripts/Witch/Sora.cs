using System;
using UnityEngine;
using Reversible.Unity;

namespace Witch {
  public class Sora : ReversibleBehaviour {
    [SerializeField] public GameObject bullet;

    private GameObject field_;
    private int damagedLayers_;
    private float toFire_;
    protected override void OnStart() {
      //playerInput_.Player.Move.performed += context => Debug.Log($"{context.ReadValue<Vector2>()}");
      field_ = GameObject.FindGameObjectWithTag("Field");
      damagedLayers_ = LayerMask.GetMask("EnemyBullet", "Enemy");
      toFire_ = 0.0f;
    }

    protected override void OnForward() {
      var player = playerInput.Player;
      var move = player.Move.ReadValue<Vector2>() * Time.deltaTime * 7;
      var trans = transform;
      var pos = trans.position;
      pos.x += move.x;
      pos.y += move.y;
      trans.position = pos;
      var fire = player.Fire;
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
      trans.localPosition = transform.localPosition + Vector3.right * 1f;
      b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 30, ForceMode2D.Impulse);
    }
    
    private void Fire2() {
      var b1 = Instantiate(bullet, field_.transform);
      var b2 = Instantiate(bullet, field_.transform);
      var pos = transform.localPosition;
      b1.transform.localPosition = pos + Vector3.right * 1f + Vector3.up * 0.25f;
      b2.transform.localPosition = pos + Vector3.right * 1f + Vector3.down * 0.25f;
      b1.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 30, ForceMode2D.Impulse);
      b2.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 30, ForceMode2D.Impulse);
    }

    private void OnCollisionStay2D(Collision2D other) {
      if(other.otherCollider.IsTouchingLayers(damagedLayers_)) {
        // TODO(ledyba): take some action
        Debug.Log("Sora: damaged");
      }
    }
  }
}
