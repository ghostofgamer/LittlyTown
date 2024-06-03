using CountersContent;
using UnityEngine;
using Wallets;

public class BonusesStart : MonoBehaviour
{
    [SerializeField] private PossibilitiesCounter _possibilitiesCounterBulldozer;
    [SerializeField] private PossibilitiesCounter _possibilitiesCounterReplacer;
    [SerializeField] private GoldWallet _goldWallet;
    [SerializeField] private CrystalWallet _crystalWallet;

    private int _currentAmountBulldozers = 0;
    private int _currentAmountReplacers = 0;
    private int _currentAmountGolds = 0;
    private int _currentAmountPrice = 0;

    public void IncreaseAmountBulldozers(int value, int price)
    {
        _currentAmountBulldozers += value;
        _currentAmountPrice += price;
    }

    public void IncreaseAmountReplaces(int value, int price)
    {
        _currentAmountReplacers += value;
        _currentAmountPrice += price;
    }

    public void IncreaseAmountGolds(int value, int price)
    {
        _currentAmountGolds += value;
        _currentAmountPrice += price;
    }

    public void ApplyBonuses()
    {
        _possibilitiesCounterBulldozer.IncreaseCount(_currentAmountBulldozers);
        _possibilitiesCounterReplacer.IncreaseCount(_currentAmountReplacers);
        _goldWallet.IncreaseValue(_currentAmountGolds);
        _currentAmountPrice = 0;
    }

    public void ResetValue()
    {
        _currentAmountBulldozers = 0;
        _currentAmountReplacers = 0;
        _currentAmountGolds = 0;
        _currentAmountPrice = 0;
    }

    public void ReturnCrystal()
    {
        if (_currentAmountPrice > 0)
            _crystalWallet.IncreaseValue(_currentAmountPrice);
    }
}