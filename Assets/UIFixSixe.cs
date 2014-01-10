using UnityEngine;
using System.Collections;

public class UIFixSixe : MonoBehaviour {

		public int targetHeight = 720;

		UIRoot mRoot;

		void Awake () { mRoot = GetComponent<UIRoot>(); }

		void Update ()
		{
				if (Screen.width > Screen.height)
				{
						mRoot.manualHeight = targetHeight;
				}
				else
				{
						mRoot.manualHeight = Mathf.RoundToInt(targetHeight * ((float)Screen.height / Screen.width));
				}
		}
}
