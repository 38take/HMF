using UnityEngine;
using System.Collections;

public class AudioSEScript : MonoBehaviour {
	
	public AudioClip[]	audioClip;
	private int			m_audioID;
	AudioSource	audioSource; 
	
	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.playOnAwake = false;
		m_audioID = 0;
		audioSource.clip = audioClip[m_audioID];
	}
	
	// Update is called once per frame
	void Update () {
		//audioSource.Play();
		//audioSource.PlayOneShot(audioSource.clip);
	}
	
	//音の切り替え 
	public void ChangeSound(int id)
	{
		if(id < audioClip.Length)
		{
			m_audioID = id;
			audioSource.clip = audioClip[m_audioID];
		}
	}
	
	//音再生 引数は鳴らしたい音の添え字
	public void PlaySE(int id)
	{
		if(id < audioClip.Length)
			audioSource.PlayOneShot(audioClip[id]);
	}
}
