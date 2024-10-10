using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ShadowEffect : MonoBehaviour
{
    public Vector3 _offset = new Vector3(0.2f, -0.2f);
    public Material _mat;
    private GameObject _shadow;

    private void Start()
    {
        _shadow = new GameObject("Shadow");
        _shadow.transform.parent = transform;

        _shadow.transform.localPosition = _offset;
        _shadow.transform.localRotation = Quaternion.identity;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        SpriteRenderer sr = _shadow.AddComponent<SpriteRenderer>();

        sr.sprite = renderer.sprite;
        sr.material = _mat;

        sr.sortingLayerName = renderer.sortingLayerName;
        sr.sortingOrder = renderer.sortingOrder -1;
    }

    private void LateUpdate()
    {
        _shadow.transform.localPosition = _offset;
    }
}