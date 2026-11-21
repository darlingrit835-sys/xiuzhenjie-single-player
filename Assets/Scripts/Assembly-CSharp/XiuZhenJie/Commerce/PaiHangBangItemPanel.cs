using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class PaiHangBangItemPanel : MonoBehaviour
	{
		public Image RankImage;

		public Text RankText;

		public Text Name;

		public Text Level;

		public Text Sect;

		public Text Floor;

		public void SetValue(int _index, string _name, string _level, string _sect, string _floor)
		{
			switch (_index)
			{
			case 1:
				RankText.gameObject.SetActive(value: false);
				RankImage.sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Rank_1.png");
				break;
			case 2:
				RankText.gameObject.SetActive(value: false);
				RankImage.sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Rank_2.png");
				break;
			case 3:
				RankText.gameObject.SetActive(value: false);
				RankImage.sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Rank_3.png");
				break;
			default:
				RankImage.gameObject.SetActive(value: false);
				RankText.text = _index.ToString();
				break;
			}
			Name.text = _name;
			Level.text = _level;
			Sect.text = _sect;
			Floor.text = _floor;
		}
	}
}
