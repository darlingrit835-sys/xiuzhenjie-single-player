using System;
using UnityEngine;

namespace XiuZhenJie
{
	internal interface IWindowContent
	{
		GameObject Instantiate(string assetPrefix, string windowName, bool builtIn);
		void Release(GameObject view, bool builtIn, bool destroy);
		void Preload(string assetPath);
		Sprite LoadSprite(string path);
		void LoadSpriteAsync(string path, Action<Sprite> onLoaded);
	}
}
