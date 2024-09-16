using UnityEngine;
using System.Collections;

/// <summary>
/// Can play one music and array of effects at same time.
/// </summary>
public class AudioManager : MonoBehaviour {

	#region Readonly fileds
	#endregion

	#region Serialize fields
	[SerializeField]
	private AudioSource musicSource;
	[SerializeField]
	private AudioSource[] effectsSource = new AudioSource[10];
	#endregion

	#region Private fields
	#endregion

	#region Properties
	#endregion

	#region Events
	//[Header("Events")]
	//[Tooltip("")]
	#endregion

	#region Unity methods
	#endregion

	#region Public methods
	public void PlayMusic(AudioClip clip) {
        PlayAudioSource(musicSource, clip, true);
    }

	public void PlayMusic(AudioClip clip, bool loop) {
		PlayAudioSource(musicSource, clip, loop);
	}

	public void PlayEffect(AudioClip clip) {
        PlayEffect(clip, transform.position, false);
    }

    public void PlayEffect(AudioClip clip, bool loop) {
        PlayEffect(clip, transform.position, loop);
    }

    public void PlayEffect(AudioClip clip, Vector3 point, bool loop = false) {
		bool isPlayed = false;
        
		for (int i = 0; i < effectsSource.Length && !isPlayed; i++) {
            if (!effectsSource[i].isPlaying) {
				isPlayed = true;

				effectsSource[i].transform.position = point;

				PlayAudioSource(effectsSource[i], clip, loop );
            }
        }
    }

	public void MuteMusic(bool mute) {
		musicSource.mute = mute; 
	}

	public void MuteEffects(bool mute) {
		foreach (var effectSource in effectsSource) {
			effectSource.mute = mute;
		}
	}

	public void PlayMarkTileSound(Tile tile) {
		if (tile.IsMarked) {
			PlayEffect(tile.MarkSound, tile.transform.position);
		}
		else {
			PlayEffect(tile.DismarkSound, tile.transform.position);
		}
	}

	public void PlayExposeTileSound(Tile tile) {
		PlayEffect(tile.ExposeSound, tile.transform.position);
	}
	#endregion

	#region Protected methods
	#endregion

	#region Private methods
	private void PlayAudioSource(AudioSource audioSource, AudioClip clip, bool loop) {
		if (audioSource.isPlaying) {
			audioSource.Stop();
		}

        audioSource.loop = loop;
		audioSource.clip = clip;

		audioSource.Play();
        //audioSource.PlayOneShot(clip);
    }
    #endregion

    #region Coroutines
    #endregion
}