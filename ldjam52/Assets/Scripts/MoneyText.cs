using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MoneyText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Awake()
    {
        text.text = InventoryManager.Instance.money.ToString();
        InventoryManager.UpdatedMoneyEvent += UpdateText;
    }

    private void UpdateText()
    {
        text.text = InventoryManager.Instance.money.ToString();
    }
}
