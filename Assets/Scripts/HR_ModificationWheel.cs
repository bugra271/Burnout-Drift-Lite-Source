//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2016 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HR_ModificationWheel : MonoBehaviour {

    public int wheelIndex;
    public int wheelPrice { get { return SelectableWheels.Instance.wheels[wheelIndex].price; } }

    private Button button;
    private Text priceLabel;
    private Image priceImage;

    void Start() {

        button = GetComponent<Button>();
        priceLabel = GetComponentInChildren<Text>();
        priceImage = priceLabel.GetComponentInParent<Image>();

    }

    public void OnClick() {

        if (!PlayerPrefs.HasKey("OwnedWheel" + wheelIndex)) {

            BuyWheel();
            return;

        }

        HR_ModHandler handler = GameObject.FindObjectOfType<HR_ModHandler>();

        handler.ChangeWheels(wheelIndex);

    }

    void Update() {

        if (wheelPrice <= 0)
            PlayerPrefs.SetInt("OwnedWheel" + wheelIndex, 1);

        if (PlayerPrefs.HasKey("OwnedWheel" + wheelIndex)) {

            if (priceImage.gameObject.activeSelf)
                priceImage.gameObject.SetActive(false);
            if (priceLabel.text != "UNLOCKED")
                priceLabel.text = "UNLOCKED";

        } else {

            if (!priceImage.gameObject.activeSelf)
                priceImage.gameObject.SetActive(true);
            if (priceLabel.text != wheelPrice.ToString())
                priceLabel.text = wheelPrice.ToString();

        }

    }

    private void BuyWheel() {

        if (BurnoutAPI.GetCurrency() >= wheelPrice) {

            HR_ModHandler handler = GameObject.FindObjectOfType<HR_ModHandler>();
            handler.BuyProperty(wheelPrice, "OwnedWheel" + wheelIndex);

        } else {

            HR_InfoDisplayer.Instance.ShowInfo("Not Enough Coins", "You have to earn " + wheelPrice.ToString() + " more coins to buy this wheel", HR_InfoDisplayer.InfoType.NotEnoughMoney);

        }

    }

}
