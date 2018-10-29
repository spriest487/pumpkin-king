using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGlow : MonoBehaviour {
    [SerializeField]
    private Vector2 eyeBounds;

    [SerializeField]
    private Transform referencePoint;

    private void Update() {
        if (Input.GetMouseButton(0)) {
            var touchPos = Input.mousePosition;

            var lookDir = (Vector2)(Camera.main.ScreenToWorldPoint(touchPos) - referencePoint.position);

            var eyePos = eyeBounds * lookDir.normalized;
            transform.localPosition = eyePos;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(transform.position, eyeBounds);
    }
}
