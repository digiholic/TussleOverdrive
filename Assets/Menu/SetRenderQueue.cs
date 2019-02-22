using UnityEngine;

public class SetRenderQueue : MonoBehaviour
{
    public int renderQueue = 3000;

    Material mMat;

    private int oldRenderQueue;

    void Start()
    {
        oldRenderQueue = renderQueue;
        UpdateRenderer();
    }

    void Update()
    {
        if (renderQueue != oldRenderQueue)
        {
            UpdateRenderer();
            oldRenderQueue = renderQueue;
            Debug.Log("Updating renderer");
        }
    }

    void UpdateRenderer()
    {
        Renderer ren = GetComponent<Renderer>();

        if (ren == null)
        {
            ParticleSystem sys = GetComponent<ParticleSystem>();
            if (sys != null) ren = sys.GetComponent<Renderer>();
        }

        if (ren != null)
        {
            mMat = new Material(ren.sharedMaterial);
            mMat.renderQueue = renderQueue;
            ren.material = mMat;
        }
    }

    void OnDestroy() { if (mMat != null) Destroy(mMat); }
}