using System;
using UI.Buttons;
using UnityEngine;

public class CollectionScrollButton : AbstractButton
{
    [SerializeField] private int _value;
    [SerializeField] private CollectionMovement[] _collectionMovements;
    [SerializeField] private PlatformChecker _platformChecker;

    private CollectionMovement _currentMovement;

    protected override void OnEnable()
    {
        base.OnEnable();
        _platformChecker.PlatformSelected += SetCollectionMovement;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _platformChecker.PlatformSelected -= SetCollectionMovement;
    }

    protected override void OnClick()
    {
        AudioSource.PlayOneShot(AudioSource.clip);
        _currentMovement.ChangeValue(_value);
    }

    private void SetCollectionMovement(int index)
    {
        _currentMovement = _collectionMovements[index];
    }
}