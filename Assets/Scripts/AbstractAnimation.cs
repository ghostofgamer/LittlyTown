using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AbstractAnimation : MonoBehaviour
{
    private Animator _animator;

    protected Animator Animator => _animator;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}