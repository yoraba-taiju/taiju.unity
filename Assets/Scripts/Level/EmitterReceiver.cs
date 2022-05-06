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
          var obj = Instantiate(groupEmitter.groupPrefab, parentTransform);
          var trans = obj.transform;
          trans.localPosition = groupEmitter.position;
          var childrenTransforms = new List<Transform>();
          foreach (Transform childTransform in obj.transform) {
            childrenTransforms.Add(childTransform);
          }
          
          foreach (Transform childTransform in childrenTransforms) {
            childTransform.SetParent(parentTransform);
          }
          trans.DetachChildren();
          Destroy(obj);
          break;
        }
        default:
          Debug.LogWarning($"Unknown notification: {notification}");
          break;
      }
    }
  }
}
