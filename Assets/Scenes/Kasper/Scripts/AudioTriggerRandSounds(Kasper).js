#pragma strict

var sounds: AudioClip[]; // set the array size and fill the elements with the sounds
public var Trigger : AudioClip;

 
function OnCollisionEnter(c : Collision){

 if (sounds.length>0) {


    var idx : int = Random.Range(0,sounds.length-1);

    audio.clip = sounds[idx];

   audio.pitch = Random.Range(0.7, 1.0);
   audio.volume = Random.Range(0.4, 1.2);
     audio.Play();

}
   
}
//function OnTriggerExit(){
   //  audio.Stop();