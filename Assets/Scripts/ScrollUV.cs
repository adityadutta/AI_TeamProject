using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUV : MonoBehaviour
{
    public float parallax;
    MeshRenderer meshRenderer;
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mat = meshRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = mat.mainTextureOffset;

        offset.y += Time.deltaTime / 10f / parallax;

        mat.mainTextureOffset = offset;
    }
}
