using UnityEngine;
using System.Collections;

public class MainMenuSaws : MonoBehaviour {
	
	public GameObject sawObject;
	public SawTrap sawtrap;
	private UITexture textureComponent;
	public Texture2D e_playImage;
	public Texture2D e_achieveImage;
	public Texture2D e_unlocksImage;
	public Texture2D d_playImage;
	public Texture2D d_achieveImage;
	public Texture2D d_unlocksImage;
	
	private Quaternion originalPosition;
	
	public GameObject projectionPoint;
	
	[SerializeField] protected GameObject dismemberedPanda;
	
	
	void Start () {
		
		
		OnEnable ();
		
		
	}
	
	public void OnPress(bool isDown)
	{
	    if(isDown)
	    {
	
	       sawtrap.ActivateTrap();
			
		   //Instantiate(dismemberedPanda, projectionPoint.transform.position, projectionPoint.transform.rotation);
			
	    }
	 
	    if(!isDown)
	    {
	       	sawtrap.DeactivateTrap();
			//sawObject.transform.rotation = originalPosition;

	    }
	} 
	
	void OnEnable()
	{
		
		sawtrap.Reset();
		
		//get textureComponent
		textureComponent = sawObject.GetComponent<UITexture>();
		
		if (Localization.instance.currentLanguage == "English")
	    {
			if(this.gameObject.name == "Play Button")
			{
				textureComponent.mainTexture = e_playImage;
			}
			else if(this.gameObject.name == "Achievements Button")
			{
				textureComponent.mainTexture = e_achieveImage;
				sawObject.transform.rotation = Quaternion.Euler(0, 0, 10);
			}
			else if(this.gameObject.name == "Unlocks Button")
			{
				textureComponent.mainTexture = e_unlocksImage;
			}

	    }
	    else
	    {
      		//selected language is Danish..
			if(this.gameObject.name == "Play Button")
			{
				textureComponent.mainTexture = d_playImage;
			}
			else if(this.gameObject.name == "Achievements Button")
			{
				textureComponent.mainTexture = d_achieveImage;
				sawObject.transform.rotation = Quaternion.Euler(0, 0, -10);
			}
			else if(this.gameObject.name == "Unlocks Button")
			{
				textureComponent.mainTexture = d_unlocksImage;
			}
			
	    }
		
		//save orginial rotational-position
		//originalPosition = sawObject.transform.rotation;
		//add deggress:
		//originalPosition = Quaternion.Euler(0, 0, -45);
		

	}
	
}
