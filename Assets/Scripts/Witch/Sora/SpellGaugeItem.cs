using UnityEngine;

namespace Witch.Sora {
  public class SpellGaugeItem : MonoBehaviour {
    private Material material_;
    private static readonly int EmissionColorKey = Shader.PropertyToID("_EmissionColor");
    private static readonly Color EmissionColor = new Color(0.0f, 0.0f, 1.0f) * 120.0f;

    private void Start() {
      material_ = gameObject.GetComponent<MeshRenderer>().material;
      SetCount(0);
    }

    public void SetCount(uint count) {
      var offset = material_.mainTextureOffset;
      offset.x = 0.5f * count / 10.0f;
      material_.mainTextureOffset = offset;
      if (count == 10) {
        material_.SetColor(EmissionColorKey, EmissionColor);
      } else {
        material_.SetColor(EmissionColorKey, Color.black);
      }
    }
  }
}
