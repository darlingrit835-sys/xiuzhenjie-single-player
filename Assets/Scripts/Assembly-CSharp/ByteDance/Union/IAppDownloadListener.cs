namespace ByteDance.Union
{
	public interface IAppDownloadListener
	{
		void OnIdle();

		void OnDownloadActive(long totalBytes, long currBytes, string fileName, string appName);

		void OnDownloadPaused(long totalBytes, long currBytes, string fileName, string appName);

		void OnDownloadFailed(long totalBytes, long currBytes, string fileName, string appName);

		void OnDownloadFinished(long totalBytes, string fileName, string appName);

		void OnInstalled(string fileName, string appName);
	}
}
