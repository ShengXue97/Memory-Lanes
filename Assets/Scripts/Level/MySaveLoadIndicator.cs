using UnityEngine;
using TMPro;

public class MySaveLoadIndicator : MonoBehaviour
{
    [Header("Mesh")]
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Material[] saveMaterials = new Material[3];
    [SerializeField] private Material[] loadMaterials = new Material[3];

    [Header("Particles")]
    [SerializeField] private ParticleSystem saveEffect;
    [SerializeField] private ParticleSystem loadEffect;

    [Header("Move Indicator")]
    [SerializeField] private TextMeshPro saveCounterText;

    public void Save()
    {
        meshRenderer.materials = saveMaterials;
        saveEffect.Play();
    }

    public void Load()
    {
        meshRenderer.materials = loadMaterials;
        loadEffect.Play();
    }

    public void SetSaveCount(int saveCount)
    {
        saveCounterText.text = saveCount.ToString();
    }
}
