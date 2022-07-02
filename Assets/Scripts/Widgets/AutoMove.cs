using UnityEngine;

namespace eggsgd.UiFramework.Examples.Widgets
{
    public class AutoMove : MonoBehaviour
    {
        [SerializeField] private Vector2 minMaxPosition = Vector2.zero;
        [SerializeField] private float speed;
        [SerializeField] private Vector2 minMaxPositionY = Vector2.zero;
        [SerializeField] private float speedY;
        [SerializeField] private Vector3 rotationSpeed = Vector3.zero;

        private void Update()
        {
            var trans = transform;
            trans.Rotate(rotationSpeed * Time.deltaTime, Space.Self);

            var t = (Mathf.Sin(Time.time * speed) + 1) * 0.5f;
            var x = Mathf.Lerp(minMaxPosition.x, minMaxPosition.y, t);

            t = (Mathf.Sin(Time.time * speedY) + 1) * 0.5f;
            var y = Mathf.Lerp(minMaxPositionY.x, minMaxPositionY.y, t);
            trans.localPosition = new Vector3(x, y, trans.position.z);
        }
    }
}