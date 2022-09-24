using System;
using UnityEngine;

namespace Witch.Sora {
  public class SpellGauge: MonoBehaviour {
    private uint spellCount_;
    [SerializeField] private SpellGaugeItem[] items = new SpellGaugeItem[6];

    public bool Fetch(uint fetchToUse) {
      if (fetchToUse > spellCount_) {
        return false;
      }
      spellCount_ -= fetchToUse;
      UpdateGauge();
      return true;
    }
    public void CollectMagicElement() {
      if (spellCount_ < 10 * items.Length) {
        spellCount_++;
        UpdateGauge();
      }
    }

    private void UpdateGauge() {
      var spell = spellCount_;
      for (var i = 0; i < items.Length; ++i) {
        if (spell > 10) {
          items[i].SetCount(10);
          spell -= 10;
        } else {
          items[i].SetCount(spell);
          spell = 0;
        }
      }
    }
  }
}