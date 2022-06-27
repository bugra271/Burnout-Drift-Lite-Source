using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowInfoOnMouseEnter : MonoBehaviour, IPointerEnterHandler,  IPointerExitHandler{

	public GameObject info;

	public float offsetX = 135f;
	public float offsetY = -50f;

	void Start () {

		info.SetActive (false);
		
	}

	public void OnPointerEnter (PointerEventData data) {

		info.SetActive (true);


//
//		Vector2 pos;
//
//		RectTransformUtility.ScreenPointToLocalPointInRectangle(
//
//		pos.x = rectTransform.rect.width  * vpPosition.x - rectTransform.rect.width / 2f;
//		pos.y = rectTransform.rect.height * vpPosition.y - rectTransform.rect.height / 2f;
//
//		crosshair.transform.localPosition = pos;
		
	}

	public void OnPointerExit (PointerEventData data) {

		info.SetActive (false);

	}

	void Update(){

		if (!info.activeInHierarchy)
			return;

//		Vector3 vpPosition = Camera.main.WorldToViewportPoint(Input.mousePosition);
//		info.GetComponent<RectTransform> ().anchoredPosition3D = vpPosition;



		Vector3[] v = new Vector3[4];
		GetComponent<RectTransform>().GetWorldCorners (v);

//		float maxY = Mathf.Max (v [0].y, v [1].y, v [2].y, v [3].y);
//		float minY = Mathf.Min (v [0].y, v [1].y, v [2].y, v [3].y);
		//No need to check horizontal visibility: there is only a vertical scroll rect
		float maxX = Mathf.Max (v [0].x, v [1].x, v [2].x, v [3].x);
		float minX = Mathf.Min (v [0].x, v [1].x, v [2].x, v [3].x);

		if (maxX < 0 || minX > Screen.height) {
			info.transform.position = new Vector2(Input.mousePosition.x - offsetX, Input.mousePosition.y + offsetY);
		} else {
			info.transform.position = new Vector2(Input.mousePosition.x + offsetX, Input.mousePosition.y + offsetY);
		}

	}

}
