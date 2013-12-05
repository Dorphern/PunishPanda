using System.Collections.Generic;
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

    [SerializeField] [EventHookAttribute("On Start Spin")]
    private List<AudioEvent> startSpinEvents = new List<AudioEvent>(1);

    [SerializeField]
    [EventHookAttribute("On End Spin")]
    private List<AudioEvent> endSpinEvents = new List<AudioEvent>(1); 
	
	
	void Start () {
		
		
		OnEnable ();
		
		//save orginial rotational-position
		originalPosition = sawObject.transform.rotation;
		//add deggress:
		originalPosition = Quaternion.Euler(0, 0, -45);
		
		//adjusting orginal position so it spins perfectly back in place
		//NOTE: this works with acceleration = 20
		if (Localization.instance.currentLanguage == "English")
	    {
			if(this.gameObject.name == "Achievements Button")
			{
			 	//45 to 19
				originalPosition = Quaternion.Euler(0, 0, -26);
			}
		}
		else
		{
			if(this.gameObject.name == "Achievements Button")
			{
				//45 to 7.47
				originalPosition = Quaternion.Euler(0, 0, -37.53f);
			}
		}
			
	}
	
	public void OnPress(bool isDown)
	{
	    if(isDown)
	    {
	       	sawtrap.ActivateTrap();
            HDRSystem.PostEvents(gameObject, startSpinEvents);
			StartCoroutine("emmitDismembered");				
	    }
	 
	    if(!isDown)
	    {
			
			//also do all this the object gets disabled
			StopCoroutine("emmitDismembered");
            HDRSystem.PostEvents(gameObject, endSpinEvents);
	       	sawtrap.DeactivateTrap();
			sawObject.transform.rotation = originalPosition;

	    }
	} 
	
	
	IEnumerator emmitDismembered()
	{
		//start delay before Instantiate
		yield return new WaitForSeconds(0.4f);
		
		Instantiate(dismemberedPanda, projectionPoint.transform.position, projectionPoint.transform.rotation);
		yield return new WaitForSeconds(Random.Range(0.2F, 0.5F));
		Instantiate(dismemberedPanda, projectionPoint.transform.position, projectionPoint.transform.rotation);
		yield return new WaitForSeconds(Random.Range(0.3F, 0.7F));
		Instantiate(dismemberedPanda, projectionPoint.transform.position, projectionPoint.transform.rotation);
		StartCoroutine("emmitDismembered");
	}
	
	//fix for multitouching saws
	void OnDisable()
	{
			StopCoroutine("emmitDismembered");
			sawtrap.DeactivateTrap();
			sawObject.transform.rotation = originalPosition;
			sawtrap.Reset ();
		
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
				sawObject.transform.rotation = Quaternion.Euler(0, 0, 19);
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
				sawObject.transform.rotation = Quaternion.Euler(0, 0, 7.47f);
			}
			else if(this.gameObject.name == "Unlocks Button")
			{
				textureComponent.mainTexture = d_unlocksImage;
				sawObject.transform.rotation = Quaternion.Euler(0, 0, 357);
			}
			
	    }
		
	}
	
}
