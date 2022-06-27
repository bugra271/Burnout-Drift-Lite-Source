using UnityEngine;
using UnityEngine.UI;

namespace EndlessCarChase
{
    /// <summary>
    /// Plays a sound from an audio source.
    /// </summary>
    public class ECCPlaySound : MonoBehaviour
    {

		private AudioSource source;

        [Tooltip("The sound to play")]
        public AudioClip[] sounds;

        [Tooltip("The tag of the sound source")]
        public string soundSourceTag = "Sound";

        [Tooltip("Play the sound immediately when the object is activated")]
        public bool playOnStart = true;

        [Tooltip("Play the sound when clicking on this button")]
        public bool playOnClick = false;

        [Tooltip("A random range for the pitch of the audio source, to make the sound more varied")]
        public Vector2 pitchRange = new Vector2(0.9f, 1.1f);

		[Range(0f, 1f)]public float volume = 1f;

        /// <summary>
        /// Start is only called once in the lifetime of the behaviour.
        /// The difference between Awake and Start is that Start is only called if the script instance is enabled.
        /// This allows you to delay any initialization code, until it is really needed.
        /// Awake is always called before any Start functions.
        /// This allows you to order initialization of scripts
        /// </summary>
        void Awake()
        {

			source = GetComponent<AudioSource> ();

			if (!source)
				source = gameObject.AddComponent<AudioSource> ();

//			source.spatialBlend = 0f;

            if (playOnStart == true)
				PlayCurrentSound();

            // Listen for a click to play a sound
            if (playOnClick && GetComponent<Button>())
				GetComponent<Button>().onClick.AddListener(delegate { PlayCurrentSound(); });

        }

        /// <summary>
        /// Plays the sound
        /// </summary>
        public void PlaySound(AudioClip sound){

			AudioClip sound1 = sounds[Random.Range(0, sounds.Length - 1)];

//            // If there is a sound source tag and audio to play, play the sound from the audio source based on its tag
//			if (soundSourceTag != string.Empty && sound1)
//            {
//                // Give the sound a random pitch limited by the time scale of the game
//                GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().pitch = Random.Range(pitchRange.x, pitchRange.y) * Time.timeScale;
//
//                // Play the sound
//				GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(sound1);
//            }
//            else if (GetComponent<AudioSource>())
//            {
//                // Give the sound a random pitch limited by the time scale of the game
//                GetComponent<AudioSource>().pitch = Random.Range(pitchRange.x, pitchRange.y) * Time.timeScale;
//
//                // Play the sound
//				GetComponent<AudioSource>().PlayOneShot(sound1);
//            }

			// If there is a sound source tag and audio to play, play the sound from the audio source based on its tag
			if (sound1){
				
				// Give the sound a random pitch limited by the time scale of the game
				source.pitch = Random.Range(pitchRange.x, pitchRange.y) * Time.timeScale;
				source.volume = volume;

				// Play the sound
				source.PlayOneShot(sound1);

			}

        }


        /// <summary>
        /// Plays the sound
        /// </summary>
		public void PlayCurrentSound(){

			AudioClip sound = sounds[Random.Range(0, sounds.Length)];

//            // If there is a sound source tag and audio to play, play the sound from the audio source based on its tag
//			if (soundSourceTag != string.Empty && sound)
//            {
//                // Give the sound a random pitch limited by the time scale of the game
//                GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().pitch = Random.Range(pitchRange.x, pitchRange.y) * Time.timeScale;
//
//                // Play the sound
//				GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(sound);
//            }
//            else if (GetComponent<AudioSource>())
//            {
//                // Give the sound a random pitch limited by the time scale of the game
//                GetComponent<AudioSource>().pitch = Random.Range(pitchRange.x, pitchRange.y) * Time.timeScale;
//
//                // Play the sound
//				GetComponent<AudioSource>().PlayOneShot(sound);
//            }

			// If there is a sound source tag and audio to play, play the sound from the audio source based on its tag
			if (sound){
				
				// Give the sound a random pitch limited by the time scale of the game
				source.pitch = Random.Range(pitchRange.x, pitchRange.y) * Time.timeScale;
				source.volume = volume;

				// Play the sound
				source.PlayOneShot(sound, volume);
			}

        }

    }
}