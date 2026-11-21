namespace ByteDance.Union
{
	public interface IBUSlotABManagerListener
	{
		void onComplete(string slotId, AdSlotType type, int errorCode, string errorMsg);
	}
}
