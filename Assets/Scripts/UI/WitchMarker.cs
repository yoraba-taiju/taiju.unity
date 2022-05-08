using System;
using UnityEngine;

namespace UI {
  public class WitchMarker: MonoBehaviour {
    [SerializeField] private GameObject witch;
    [SerializeField] private GameObject targetCanvas;
    [SerializeField] private GameObject targetCamera;

    private RectTransform canvasRect_;
    private RectTransform rectTransform_;
    private Camera camera_;

    private void Start() {
      canvasRect_ = targetCanvas.GetComponent<RectTransform>();
      rectTransform_ = GetComponent<RectTransform>();
      camera_ = targetCamera.GetComponent<Camera>();
    }

    private void LateUpdate() {
      var screenPos = RectTransformUtility.WorldToScreenPoint(
        camera_,
        witch.transform.position
      );
      RectTransformUtility.ScreenPointToLocalPointInRectangle(
        canvasRect_,
        screenPos,
        camera_,
        out var pos
      );
      // https://forum.unity.com/threads/rect-transform-position-not-updating-correctly-but-print-statements-show-correct-values.807261/
      rectTransform_.anchoredPosition = pos;
    }
  }
}
