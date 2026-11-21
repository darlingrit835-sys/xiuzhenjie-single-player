namespace ByteDance.Union
{
	public interface INativeAdListener
	{
		void OnError(int code, string message);

		void OnNativeAdLoad(NativeAd[] ads);
	}
}
