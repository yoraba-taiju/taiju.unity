using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Level {
  public class EmitterReceiver : MonoBehaviour, INotificationReceiver {
    [SerializeField]
    public GameObject parent;
    public void OnNotify(Playable origin, INotification notification, object context) {
      var parentTransform = parent.transform;
      switch (notification) {
        case PrefabEmitter prefabEmitter: {
          var obj = Instantiate(prefabEmitter.prefab, parentTransform);
          obj.transform.localPosition = prefabEmitter.position;
          break;
        }
        case GroupEmitter groupEmitter: {
          foreach (Transform childTransform in groupEmitter.groupPrefab.transform) {
            var obj = Instantiate(childTransform.gameObject, parentTransform);
            obj.transform.localPosition += (Vector3)groupEmitter.position;
          }
          break;
        }
        default:
          Debug.LogWarning($"Unknown notification: {notification}");
          break;
      }
    }
  }
}
