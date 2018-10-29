using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    [SerializeField]
    private Transform pumpking;

    private void LateUpdate() {
        transform.position = pumpking.position;
    }
}
