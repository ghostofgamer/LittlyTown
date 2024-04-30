using System;
using UnityEngine;

[RequireComponent(typeof(Item), typeof(Animator))]
public class ItemAnimation : MonoBehaviour
{
    private Item _item;
    private Animator _animator;
    private static readonly int Active = Animator.StringToHash("Active");

    private void Awake()
    {
        _item = GetComponent<Item>();
        _animator = GetComponent<Animator>();

        if (!_item.IsActive)
            _animator.SetBool(Active, false);
    }

    private void OnEnable()
    {
        _item.Activated += StopAnimation;
    }

    private void OnDisable()
    {
        _item.Activated -= StopAnimation;
    }
    
    private void Start()
    {
        /*_item = GetComponent<Item>();
        _animator = GetComponent<Animator>();

        if (!_item.IsActive)
            _animator.SetBool(ItemAnim, false);*/
    }

    private void StopAnimation()
    {
        Debug.Log("Анимацию в тру");
        _animator.SetBool(Active, true);
    }
}