using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenURLOnClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public string URL = "URL";

	/// <summary>
	///     Method must be called on OnMouseDown,and never be used in OnMouseUp.
	/// </summary>
	public void ClickedLogo_NewTab(){

//		#if UNITY_EDITOR || UNITY_WEBPLAYER
//		Application.OpenURL(URL);
//		#else
//		Idnet.I.DomInjection(URL);
//		#endif

	}

	public void OnPointerDown(PointerEventData eventData){

//		#if UNITY_EDITOR || UNITY_WEBPLAYER
//		Application.OpenURL(URL);
//		#else
//		Idnet.I.DomInjection(URL);
//		#endif

		Application.OpenURL(URL);

	}

	public void OnPointerUp(PointerEventData eventData){



	}

	public void OpenURL1 () {

		Application.ExternalEval ("window.open(\"https://www.crazygames.com\")");
		
	}

	public void OpenURL2 () {

		Application.ExternalEval ("window.open(\"https://itunes.apple.com/us/app/burnout-drift/id1192830009?mt=8\")");

	}

	public void OpenURL3 () {

		Application.ExternalEval ("window.open(\"http://y8.com/games_for_your_website\")");

	}

	public void OpenURL4 () {

		Application.ExternalEval ("window.open(\"http://y8.com/games_for_your_website\")");

	}

}
