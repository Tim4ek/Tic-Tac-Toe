using UnityEngine;
namespace Company.Runtime.Gameplay {
  public class ItemViewBehaviour : MonoBehaviour {
    [SerializeField] private SpriteRenderer content;

    public void SetSprite(Sprite sprite) {
      content.sprite = sprite;
    }

    public void DestroyView() {
      Destroy(this.gameObject);
    }
  }
}