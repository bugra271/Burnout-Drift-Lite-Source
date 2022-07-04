using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Neon : MonoBehaviour {

    private string key;

    public int index = 0;
    public int price = 5000;

    public bool purchased = false;
    public GameObject buyButton;
    public Text priceText;

    public AudioClip purchaseSound;

    private void OnEnable() {

        VehicleUpgrade_NeonManager dm = GameObject.FindObjectOfType<VehicleUpgrade_NeonManager>();

        switch (index) {

            case 0:
                key = dm.red;
                break;

            case 1:
                key = dm.green;
                break;

            case 2:
                key = dm.blue;
                break;

            case 3:
                key = dm.yellow;
                break;

            case 4:
                key = dm.orange;
                break;

            case 5:
                key = dm.white;
                break;

        }

        CheckPurchase();

    }

    public void CheckPurchase() {

        VehicleUpgrade_NeonManager dm = GameObject.FindObjectOfType<VehicleUpgrade_NeonManager>();

        purchased = PlayerPrefs.HasKey(dm.transform.root.name + key);

        if (purchased) {

            if (buyButton)
                buyButton.SetActive(false);

            if (priceText)
                priceText.text = "";

        } else {

            if (buyButton)
                buyButton.SetActive(true);

            if (priceText)
                priceText.text = price.ToString();

        }

    }

    public void Upgrade() {

        VehicleUpgrade_NeonManager dm = GameObject.FindObjectOfType<VehicleUpgrade_NeonManager>();

        dm.Upgrade(index);

        CheckPurchase();

    }

    public void Buy() {

        if (BurnoutAPI.GetCurrency() >= price) {

            BurnoutAPI.ConsumeCurrency(price);
            Upgrade();

            if (purchaseSound)
                RCC_Core.NewAudioSource(gameObject, purchaseSound.name, 0f, 0f, 1f, purchaseSound, false, true, true);

        } else {

            HR_InfoDisplayer.Instance.ShowInfo("Not Enough Coins", "You have to earn " + (price - BurnoutAPI.GetCurrency()).ToString() + " more coins to purchase this neon", HR_InfoDisplayer.InfoType.NotEnoughMoney);
            return;

        }

    }

}
