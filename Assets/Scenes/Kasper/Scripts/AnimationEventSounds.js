#pragma strict

var sound1 : AudioClip[];
public var volume1 : float = 1.0; 
var sound2 : AudioClip[];
public var volume2 : float = 1.0; 
var sound3 : AudioClip[];
public var volume3 : float = 1.0; 
var sound4 : AudioClip[];
public var volume4 : float = 1.0;
var sound5 : AudioClip[];
public var volume5 : float = 1.0; 
var sound6 : AudioClip[];
public var volume6 : float = 1.0; 
var sound7 : AudioClip[];
public var volume7 : float = 1.0; 
var sound8 : AudioClip[];
public var volume8 : float = 1.0; 
var sound9 : AudioClip[];
public var volume9 : float = 1.0; 
var sound10 : AudioClip[];
public var volume10 : float = 1.0; 



function AnimSound1(){ 
   if (audio.isPlaying) return; // don't play a new sound while the last hasn't finished
   audio.clip = sound1[Random.Range(0,sound1.length)];  
   audio.pitch = 0.9 + 0.5*Random.value; // randomize pitch in the range 1 +/- 0.1 
   audio.volume = volume1;
   audio.Play();
}

function AnimSound2(){ 
   // if (audio.isPlaying) return; // don't play a new sound while the last hasn't finished
    audio.clip = sound2[Random.Range(0,sound2.length)];
    audio.pitch = 0.9 + 0.5*Random.value; // randomize pitch in the range 1 +/- 0.1 
    audio.volume = volume2;
	audio.Play();
}

function AnimSound3(){ 
   // if (audio.isPlaying) return; // don't play a new sound while the last hasn't finished
    audio.clip = sound3[Random.Range(0,sound3.length)];
    audio.pitch = 0.9 + 0.5*Random.value; // randomize pitch in the range 1 +/- 0.1 
	audio.volume = volume3;
	audio.Play();
}

function AnimSound4(){ 
   // if (audio.isPlaying) return; // don't play a new sound while the last hasn't finished
    audio.clip = sound4[Random.Range(0,sound4.length)];
    audio.pitch = 0.9 + 0.5*Random.value; // randomize pitch in the range 1 +/- 0.1 
	audio.volume = volume4;
	audio.Play();
}
function AnimSound5(){ 
   // if (audio.isPlaying) return; // don't play a new sound while the last hasn't finished
    audio.clip = sound5[Random.Range(0,sound5.length)];
    audio.pitch = 0.9 + 0.5*Random.value; // randomize pitch in the range 1 +/- 0.1 
	audio.volume = volume5;
	audio.Play();
}
function AnimSound6(){ 
   // if (audio.isPlaying) return; // don't play a new sound while the last hasn't finished
    audio.clip = sound6[Random.Range(0,sound6.length)];
    audio.pitch = 0.9 + 0.5*Random.value; // randomize pitch in the range 1 +/- 0.1 
	audio.volume = volume6;
	audio.Play();
}
function AnimSound7(){ 
   // if (audio.isPlaying) return; // don't play a new sound while the last hasn't finished
    audio.clip = sound7[Random.Range(0,sound7.length)];
    audio.pitch = 0.9 + 0.5*Random.value; // randomize pitch in the range 1 +/- 0.1 
	audio.volume = volume7;
	audio.Play();
}
function AnimSound8(){ 
   // if (audio.isPlaying) return; // don't play a new sound while the last hasn't finished
    audio.clip = sound8[Random.Range(0,sound8.length)];
    audio.pitch = 0.9 + 0.5*Random.value; // randomize pitch in the range 1 +/- 0.1 
	audio.volume = volume8;
	audio.Play();
}
function AnimSound9(){ 
   // if (audio.isPlaying) return; // don't play a new sound while the last hasn't finished
    audio.clip = sound9[Random.Range(0,sound9.length)];
    audio.pitch = 0.9 + 0.5*Random.value; // randomize pitch in the range 1 +/- 0.1 
	audio.volume = volume9;
	audio.Play();
}
function AnimSound10(){ 
   // if (audio.isPlaying) return; // don't play a new sound while the last hasn't finished
    audio.clip = sound10[Random.Range(0,sound10.length)];
    audio.pitch = 0.9 + 0.5*Random.value; // randomize pitch in the range 1 +/- 0.1 
	audio.volume = volume10;
	audio.Play();
}