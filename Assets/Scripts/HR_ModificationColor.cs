//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2016 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class HR_ModificationColor : MonoBehaviour {

	public int colorIndex = 0;

	public pickedColor _pickedColor;
	public enum pickedColor{Orange, Red, Green, Blue, Black, White, Cyan, Magenta, Pink}

	public int colorPrice;
	public bool unlocked = false;

	private Text priceLabel;
	private Image priceImage;

	void Start(){

		priceLabel = GetComponentInChildren<Text>();
		priceImage = priceLabel.GetComponentInParent<Image>();

		var qwe = Enum.GetValues (typeof(pickedColor));

		int i = 0;

		foreach (object item in qwe) {

			if (((pickedColor)item).Equals (_pickedColor))
				break;

			i++;
			
		}

		colorIndex = i;

	}

	public void OnClick () {

		if(!unlocked){
			BuyColor();
			return;
		}

		HR_ModHandler handler = GameObject.FindObjectOfType<HR_ModHandler>();
		Color selectedColor = new Color();

		switch(_pickedColor){

		case pickedColor.Orange:
			selectedColor = Color.red + (Color.green / 2f);
			break;

		case pickedColor.Red:
			selectedColor = Color.red;
			break;

		case pickedColor.Green:
			selectedColor = Color.green;
			break;

		case pickedColor.Blue:
			selectedColor = Color.blue;
			break;

//		case pickedColor.Yellow:
//			selectedColor = Color.yellow;
//			break;

		case pickedColor.Black:
			selectedColor = Color.black;
			break;

		case pickedColor.White:
			selectedColor = Color.white;
			break;

		case pickedColor.Cyan:
			selectedColor = Color.cyan;
			break;

		case pickedColor.Magenta:
			selectedColor = Color.magenta;
			break;

		case pickedColor.Pink:
			selectedColor = new Color(1, 0f, .5f);
			break;

		}

		handler.ChangeChassisColor(selectedColor);
	
	}
	
	void Update(){

		if(colorPrice <= 0 && !unlocked){

			PlayerPrefs.SetInt("OwnedColor" + colorIndex, 1);
			unlocked = true;

		}

		if(PlayerPrefs.HasKey("OwnedColor" + colorIndex))
			unlocked = true;
		else
			unlocked = false;

		if(!unlocked){
			if(!priceImage.gameObject.activeSelf)
				priceImage.gameObject.SetActive(true);
			if(priceLabel.text != colorPrice.ToString())
				priceLabel.text = colorPrice.ToString();
		}else{
			if(priceImage.gameObject.activeSelf)
				priceImage.gameObject.SetActive(false);
			if(priceLabel.text != "UNLOCKED")
				priceLabel.text = "UNLOCKED";
		}

	}

	void BuyColor(){

		if (BurnoutAPI.GetCurrency() >= colorPrice) {
			
			HR_ModHandler handler = GameObject.FindObjectOfType<HR_ModHandler> ();
			handler.BuyProperty (colorPrice, "OwnedColor" + colorIndex);

		} else {

			HR_InfoDisplayer.Instance.ShowInfo ("Not Enough Coins", "You have to earn " + colorPrice.ToString() + " more coins to buy this color", HR_InfoDisplayer.InfoType.NotEnoughMoney);

		}

	}

}
