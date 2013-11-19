var sound : AudioClip;
var meanTimeInterval : float;
var radius : float;

private var currentTime : float;
private var currentInterval : float;
private var nextTime : float;
private var position : Vector3;

function Start() {
	nextTime = 0;
}

function Update () {

	currentTime = Time.time;
	if ( currentTime >= nextTime ) {
	
		// Sets the position to be somewhere inside a sphere
		// with a given radius and the center at zero.
		position = Random.insideUnitSphere * radius;
		
		PlayAudioClip(sound, position, 1);
		
		// To calculate the time to the next playback the exponential
		// distribution is used
		currentInterval = - meanTimeInterval*Mathf.Log(Random.value);
		nextTime = nextTime + currentInterval;
	}

}

function PlayAudioClip (clip : AudioClip, position : Vector3, volume : float) {

	var go = new GameObject ("One shot audio");
	var source : AudioSource = go.AddComponent (AudioSource);
	
	go.transform.position = position;
	source.clip = clip;
	source.volume = volume;
	source.pitch = Random.Range(0.6, 1.4);
	source.Play ();
	Destroy (go, clip.length);

}