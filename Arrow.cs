//Script responsible to set up the white arrow used to choose the direction of the ball before throwing it.
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Arrow : MonoBehaviour {
    public Material m_LineMaterial;
    public float m_StartWidth;
    public float m_EndWidth;

    private LineRenderer m_Line;

  
    private void Start() {
        m_Line = GetComponent<LineRenderer>();
        m_Line.SetPosition(0, transform.position);
        m_Line.SetPosition(1, transform.position);
        m_Line.material = m_LineMaterial;
    }

    private void Update() {
        m_Line.startWidth = m_StartWidth;
        m_Line.endWidth = m_EndWidth;
    }

    public void SetPosition(int index, Vector3 position) {
        m_Line.SetPosition(index, position);
    }

    public void SetTextureOffset(Vector3 offset){
        m_LineMaterial.SetTextureOffset("_MainTex", offset);
    }
}
