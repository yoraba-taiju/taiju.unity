using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Level {
  public class Spawner : MonoBehaviour, INotificationReceiver {
    [SerializeField]
    public GameObject parent;
    public void OnNotify(Playable origin, INotification notification, object context) {
      if (notification is not SpawnEmitter emitter) {
        return;
      }
      var obj = Instantiate(emitter.prefab, parent.transform);
      obj.transform.localPosition = emitter.position;
    }
  }
}
