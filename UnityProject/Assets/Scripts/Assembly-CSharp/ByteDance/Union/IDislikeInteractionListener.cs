namespace ByteDance.Union
{
	public interface IDislikeInteractionListener
	{
		void OnSelected(int var1, string var2, bool enforce);

		void OnCancel();

		void OnShow();
	}
}
