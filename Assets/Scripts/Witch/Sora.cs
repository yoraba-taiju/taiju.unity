using UnityEngine;
using Reversible.Unity;

namespace Witch {
  public class Sora : ReversibleBehaviour {
    [SerializeField] public GameObject bullet;
    private GameObject field_;
    protected override void OnStart() {
      //playerInput_.Player.Move.performed += context => Debug.Log($"{context.ReadValue<Vector2>()}");
      field_ = GameObject.FindGameObjectWithTag("Field");
    }

    private float toFire_ = 0.0f;
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
            Fire();
            toFire_ += 100.0f / 1000.0f;
          } else {
            Fire();
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

    private void Fire() {
      var b = Instantiate(bullet, field_.transform);
      var bt = b.transform;
      bt.localPosition = transform.localPosition + Vector3.right * 1f;
      bt.rotation = Quaternion.identity;
      b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 30, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col) {
      Debug.Log("Collided");
    }
  }
}