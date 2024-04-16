using UnityEngine;
namespace Company.Runtime.Gameplay {
  public class GridBehaviour : MonoBehaviour {
    public void DestroyView() {
      Destroy(this.gameObject);
    }
  }
}
