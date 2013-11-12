#pragma strict

var sounds: AudioClip[]; // set the array size and fill the elements with the sounds
var minTime : float = 5.0; // the shortest time between two sounds (seconds)
var maxTime : float = 10.0; // the longest time between two sounds (seconds)
var minPitch : float = 1.0; // the lowest pitch that will play
var maxPitch : float = 1.0; // the highest pitch that will play
private var minPitchConv : float; 
private var maxPitchConv : float;

function Start(){

minPitchConv = Mathf.Pow(1.05946,minPitch); // convert semitone values to unity pitch values
maxPitchConv = Mathf.Pow(1.05946,maxPitch);

Invoke ( "PlayRandom", minTime+(maxTime*Random.value)); 
}


function PlayRandom(){ // call this function to play a random sound
    
    if (audio.isPlaying) return; // don't play a new sound while the last hasn't finished
    
    audio.clip = sounds[Random.Range(0,sounds.length)];
    audio.pitch = minPitchConv + ((maxPitchConv-minPitchConv)*Random.value); 
    audio.Play();
    
    Invoke( "PlayRandom", minTime+(maxTime*Random.value));

}


