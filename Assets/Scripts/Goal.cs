using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    public MeshRenderer renderer;
    public Material active;
    public Material notActive;
    public Material disabled;

    public void ResetPos(float range) {
        float x = Random.Range(-range, range);
        float y = Random.Range(0f, range) + 1f;
        float z = Random.Range(-range, range);
        transform.localPosition = new Vector3(x, y, z);
    }

    public void Activated(bool isActive) {
        if (isActive) {
            renderer.material = active;
        } else {
            renderer.material = notActive;
        }
    }

    public void Disabled() {
        renderer.material = disabled;
    }
}
