using UnityEngine;

namespace Donut {
  public class Clock : MonoBehaviour {
    private uint leaps_;
    private uint current_;
    public Clock() {
      this.leaps_ = 0;
      this.current_ = 0;
    }

    public void tick() {
      this.current_++;
    }

    public void back() {
      this.current_--;
    }
  }
}
