using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[AddComponentMenu("Rendering/SetRenderQueue")]

public class MaskShader : MonoBehaviour
{
    public int[] m_queues = new int[]{3000};
    Material[] materials;
    private void Start()
    {
        materials = GetComponent<Renderer>().materials;
    }

    private void Update()
    {
        for(int index = 0; index < materials.Length && index < m_queues.Length; ++index)
        {
            materials[index].renderQueue = m_queues[index];
        }
    }
}
