using System.Collections.Generic;

public class BaseNaJieGoodEntity
{
	public int FiveElement;

	public string GoodsId;

	public string TemplateId;

	public string BlankId;

	public double ConversionRate;

	public int Type;

	public int SecondType;

	public int Level;

	public int Rank;

	public bool IsBinding;

	public bool IsUsing;

	public string Name;

	public int RecyclePrice;

	public double GoodsVolume;

	public bool IsLock;

	public bool OfferPending;

	public List<string> ContentList;

	public int GoodsNum;

	public int GetGoodsStage()
	{
		return (Level - 1) / 10 + 1;
	}

	public int GetGoodsRank()
	{
		switch (Rank)
		{
		case 1:
		case 2:
		case 3:
		case 4:
			return 1;
		case 5:
		case 6:
		case 7:
		case 8:
			return 2;
		case 9:
		case 10:
		case 11:
		case 12:
			return 3;
		case 13:
		case 14:
		case 15:
		case 16:
			return 4;
		default:
			return 1;
		}
	}

	public string GetGoodsRankName()
	{
		switch (Rank)
		{
		case 1:
		case 2:
		case 3:
		case 4:
			return "黄";
		case 5:
		case 6:
		case 7:
		case 8:
			return "玄";
		case 9:
		case 10:
		case 11:
		case 12:
			return "地";
		case 13:
		case 14:
		case 15:
		case 16:
			return "天";
		default:
			return "黄";
		}
	}

	public string GetFiveElementName()
	{
		return FiveElement switch
		{
			1 => "金", 
			2 => "木", 
			3 => "水", 
			4 => "火", 
			5 => "土", 
			_ => "五行", 
		};
	}

	public BaseNaJieGoodEntity Copy()
	{
		return (BaseNaJieGoodEntity)MemberwiseClone();
	}
}
