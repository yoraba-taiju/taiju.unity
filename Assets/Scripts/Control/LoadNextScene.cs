using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Control {
  public class LoadNextScene : MonoBehaviour {
    private void Start() {
      StartCoroutine(StartLoad());
    }
    IEnumerator<Object> StartLoad() {
      var ticket = SceneManager.LoadSceneAsync("Stage");
      while (!ticket.isDone) {
        yield return null;
      }
    }
  }
}
