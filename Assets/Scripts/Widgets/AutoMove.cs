﻿using UnityEngine;

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
            transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
            var x = Mathf.Lerp(minMaxPosition.x, minMaxPosition.y, (Mathf.Sin(Time.time * speed) + 1) / 2f);
            var y = Mathf.Lerp(minMaxPositionY.x, minMaxPositionY.y, (Mathf.Sin(Time.time * speedY) + 1) / 2f);
            transform.localPosition = new Vector3(x, y, transform.position.z);
        }
    }
}