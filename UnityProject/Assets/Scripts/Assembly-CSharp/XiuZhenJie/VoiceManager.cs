using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class VoiceManager : Singleton<VoiceManager>
	{
		private Dictionary<VoiceType, AudioClip> m_VoiceClipDic;

		private Dictionary<BackGroundMusicType, AudioClip> m_BackMusicClipDic;

		private bool m_isPlayBackMusic;

		private bool m_isPlayVoice;

		private AudioSource m_MusicPlayer;

		private AudioSource m_VoicePlayer;

		public void Init(AudioSource musicPlayer, AudioSource voicePlayer)
		{
			m_MusicPlayer = musicPlayer;
			m_VoicePlayer = voicePlayer;
			m_VoiceClipDic = new Dictionary<VoiceType, AudioClip>();
			m_BackMusicClipDic = new Dictionary<BackGroundMusicType, AudioClip>();
			AudioClip value = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/MainCityBgMusic.mp3");
			AudioClip value2 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/DongFuBgMusic.mp3");
			AudioClip value3 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/BattleBgMusic.mp3");
			AudioClip value4 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/ButtonVoice.wav");
			AudioClip value5 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/Attack_JinVoice.wav");
			AudioClip value6 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/Attack_MuVoice.wav");
			AudioClip value7 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/Attack_ShuiVoice.wav");
			AudioClip value8 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/Attack_HuoVoice.wav");
			AudioClip value9 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/Attack_TuVoice.wav");
			AudioClip value10 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/Attack_AllVoice.wav");
			AudioClip value11 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/LianDanSuccess.wav");
			AudioClip value12 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/LianDanFail.wav");
			AudioClip value13 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/LianDanFire.wav");
			AudioClip value14 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/LianQiVoice.wav");
			AudioClip value15 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/LianQiBgMusic.wav");
			AudioClip value16 = Singleton<ResourceManager>.Instance.LoadResource<AudioClip>("Assets/GameData/Music/ZhenFaBgMusic.wav");
			if (!m_VoiceClipDic.ContainsKey(VoiceType.BtnClick))
			{
				m_VoiceClipDic.Add(VoiceType.BtnClick, value4);
			}
			if (!m_VoiceClipDic.ContainsKey(VoiceType.Attack_Jin))
			{
				m_VoiceClipDic.Add(VoiceType.Attack_Jin, value5);
			}
			if (!m_VoiceClipDic.ContainsKey(VoiceType.Attack_Mu))
			{
				m_VoiceClipDic.Add(VoiceType.Attack_Mu, value6);
			}
			if (!m_VoiceClipDic.ContainsKey(VoiceType.Attack_Shui))
			{
				m_VoiceClipDic.Add(VoiceType.Attack_Shui, value7);
			}
			if (!m_VoiceClipDic.ContainsKey(VoiceType.Attack_Huo))
			{
				m_VoiceClipDic.Add(VoiceType.Attack_Huo, value8);
			}
			if (!m_VoiceClipDic.ContainsKey(VoiceType.Attack_Tu))
			{
				m_VoiceClipDic.Add(VoiceType.Attack_Tu, value9);
			}
			if (!m_VoiceClipDic.ContainsKey(VoiceType.Attack_All))
			{
				m_VoiceClipDic.Add(VoiceType.Attack_All, value10);
			}
			if (!m_VoiceClipDic.ContainsKey(VoiceType.LianDanSuccess))
			{
				m_VoiceClipDic.Add(VoiceType.LianDanSuccess, value11);
			}
			if (!m_VoiceClipDic.ContainsKey(VoiceType.LianDanFail))
			{
				m_VoiceClipDic.Add(VoiceType.LianDanFail, value12);
			}
			if (!m_VoiceClipDic.ContainsKey(VoiceType.LianDanFire))
			{
				m_VoiceClipDic.Add(VoiceType.LianDanFire, value13);
			}
			if (!m_VoiceClipDic.ContainsKey(VoiceType.LianQiVoice))
			{
				m_VoiceClipDic.Add(VoiceType.LianQiVoice, value14);
			}
			if (!m_VoiceClipDic.ContainsKey(VoiceType.ZhenFaVoice))
			{
				m_VoiceClipDic.Add(VoiceType.ZhenFaVoice, value16);
			}
			if (!m_BackMusicClipDic.ContainsKey(BackGroundMusicType.Main))
			{
				m_BackMusicClipDic.Add(BackGroundMusicType.Main, value);
			}
			if (!m_BackMusicClipDic.ContainsKey(BackGroundMusicType.DongFu))
			{
				m_BackMusicClipDic.Add(BackGroundMusicType.DongFu, value2);
			}
			if (!m_BackMusicClipDic.ContainsKey(BackGroundMusicType.Battle))
			{
				m_BackMusicClipDic.Add(BackGroundMusicType.Battle, value3);
			}
			if (!m_BackMusicClipDic.ContainsKey(BackGroundMusicType.LianQi))
			{
				m_BackMusicClipDic.Add(BackGroundMusicType.LianQi, value15);
			}
			m_isPlayVoice = true;
			m_isPlayBackMusic = true;
			if (PlayerPrefs.GetString("SwitchYinYue") == "false")
			{
				m_isPlayBackMusic = false;
			}
			else
			{
				m_isPlayBackMusic = true;
			}
			if (PlayerPrefs.GetString("SwitchYinXiao") == "false")
			{
				m_isPlayVoice = false;
			}
			else
			{
				m_isPlayVoice = true;
			}
		}

		public void PlayVoice(VoiceType voiceType, bool isLoop = false)
		{
			if (m_isPlayVoice && m_VoiceClipDic.ContainsKey(voiceType))
			{
				m_VoicePlayer.Stop();
				m_VoicePlayer.clip = m_VoiceClipDic[voiceType];
				if (!isLoop)
				{
					m_VoicePlayer.loop = false;
					m_VoicePlayer.PlayOneShot(m_VoiceClipDic[voiceType]);
				}
				else
				{
					m_VoicePlayer.loop = true;
					m_VoicePlayer.Play();
				}
			}
		}

		public void SwitchBgMusic(BackGroundMusicType musicType)
		{
			if (!m_isPlayBackMusic || !m_BackMusicClipDic.ContainsKey(musicType))
			{
				return;
			}
			if (m_MusicPlayer.isPlaying)
			{
				StopBgMusic(() =>
				{
					PlayBgMusic(m_BackMusicClipDic[musicType]);
				});
			}
			else
			{
				PlayBgMusic(m_BackMusicClipDic[musicType]);
			}
		}

		public void OpenMusic()
		{
			m_isPlayBackMusic = true;
			if (m_MusicPlayer.isPlaying)
			{
				return;
			}
			if (m_MusicPlayer.clip == null)
			{
				switch (Singleton<GameMapManager>.Instance.CurrentMapName)
				{
				case "DongFuScene":
					SwitchBgMusic(BackGroundMusicType.DongFu);
					break;
				case "MainScene":
					SwitchBgMusic(BackGroundMusicType.Main);
					break;
				case "FightingScene":
					SwitchBgMusic(BackGroundMusicType.Battle);
					break;
				}
			}
			else
			{
				m_MusicPlayer.Play();
			}
		}

		public void PauseMusic()
		{
			m_isPlayBackMusic = false;
			m_MusicPlayer.Pause();
		}

		public void StopMusic()
		{
			m_isPlayBackMusic = false;
			m_MusicPlayer.Stop();
		}

		public void OpenVoice()
		{
			m_isPlayVoice = true;
		}

		public void StopVoice()
		{
			m_isPlayVoice = false;
		}

		private void PlayBgMusic(AudioClip clip)
		{
			m_MusicPlayer.clip = clip;
			m_MusicPlayer.Play();
			MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(PlayMusic());
		}

		private void StopBgMusic(UnityAction callBack)
		{
			MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(StopMusic(() =>
			{
				m_MusicPlayer.Stop();
				callBack?.Invoke();
			}));
		}

		private IEnumerator PlayMusic()
		{
			m_MusicPlayer.volume = 0f;
			while (m_MusicPlayer.volume < 1f)
			{
				m_MusicPlayer.volume += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator StopMusic(UnityAction callBack)
		{
			m_MusicPlayer.volume = 1f;
			while (m_MusicPlayer.volume > 0f)
			{
				m_MusicPlayer.volume -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			callBack?.Invoke();
		}
	}
}
