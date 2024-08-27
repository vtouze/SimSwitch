using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{ 
    private Button _button;
    [SerializeField] private Vector3 _sizeUp = new Vector3(1.2f, 1.2f, 1);

    private void Awake()
    {
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(DoAnimation);
    }

    private void DoAnimation()
    {
        LeanTween.scale(gameObject, _sizeUp, .1f);
        LeanTween.scale(gameObject, Vector3.one, .1f).setDelay(.1f);
    }
}