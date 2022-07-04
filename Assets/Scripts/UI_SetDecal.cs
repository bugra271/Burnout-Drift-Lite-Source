using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetDecal : MonoBehaviour{

    public int index = 0;

    public int price = 5000;

    public bool purchased = false;
    public GameObject buyButton;
    public Text priceText;

    public AudioClip purchaseSound;

    private void OnEnable() {

        CheckPurchase();

    }

    public void CheckPurchase() {

        purchased = PlayerPrefs.HasKey("Decal_" + index.ToString());

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

    public void SetDecal() {

        VehicleUpgrade_DecalManager dm = GameObject.FindObjectOfType<VehicleUpgrade_DecalManager>();

        dm.SetDecalMaterial(index);

        CheckPurchase();

    }

    public void Buy() {

        if (BurnoutAPI.GetCurrency() >= price) {

            BurnoutAPI.ConsumeCurrency(price);
            PlayerPrefs.SetInt("Decal_" + index.ToString(), 1);
            SetDecal();

            if (purchaseSound)
                RCC_Core.NewAudioSource(gameObject, purchaseSound.name, 0f, 0f, 1f, purchaseSound, false, true, true);

        } else {

            HR_InfoDisplayer.Instance.ShowInfo("Not Enough Coins", "You have to earn " + (price - BurnoutAPI.GetCurrency()).ToString() + " more coins to purchase this decal", HR_InfoDisplayer.InfoType.NotEnoughMoney);
            return;

        }

    }

}
