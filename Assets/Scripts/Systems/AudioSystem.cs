using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class sound : System.Object
{

	public string name;
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = 0.7f;

	[Range(0.5f, 1.5f)]
	public float pitch = 1.0f;


	[Range(0f, 0.5f)]
	public float randomVolume = 0.1f;

	[Range(0f, 0.5f)]
	public float randomPitch = 0.1f;

	private AudioSource source;



	public void SetSource(AudioSource _source)
	{
		source = _source;
		source.clip = clip;
	}


	public void Play()
	{
		source.volume = volume * (1 + Random.Range (-randomVolume / 2f, randomVolume / 2f));
		source.pitch = pitch * (1 + Random.Range (-randomPitch / 2f, randomPitch / 2f));
		source.Play ();
	}


}




public class AudioSystem : MonoBehaviour {

	public static AudioSystem instance;

	[SerializeField]
	public sound[] sounds = new sound[10];

	void Awake()
	{
		if (instance != null)
			Debug.LogError ("More than one AudioSystem in the Scene.");
		else
			instance = this;
	}

	void Start(){

		for (int i = 0; i < sounds.Length; i++) {
			GameObject _go = new GameObject ("Sound_" + i + "_" + sounds [i].name);
			AudioSource _audiosource = _go.AddComponent<AudioSource> ();
			sounds [i].SetSource (_audiosource);
		}
		//PlaySound ("Atmo_Loop"); nur zum testen ob überhaupt was abspielt
	}


	public void PlaySound(string _name){

			for (int i = 0; i < sounds.Length; i++) {
			
				if(sounds[i].name == _name)
				{
					sounds[i].Play();
					return;
				}
			}

		//no sound with _name found
		Debug.LogWarning("AudioSystem: Sound not found in list: "+ _name);
	}


}
