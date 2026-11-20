using UnityEngine;

namespace XiuZhenJie
{
	internal static class MethodBookIcon
	{
		internal static Sprite Resolve(object source, Sprite fallback)
		{
			string methodId;
			switch (source)
			{
			case GongFaEntity equipped:
				methodId = equipped.Id;
				break;
			case CanWuEntity study when study.Type == (int)CanWuTitleType.GongFa:
				methodId = study.Id;
				break;
			case BaseNaJieGoodEntity book when book.Type == (int)GoodsTypeEnum.BOOK_ACHIEVEMENT:
				methodId = book.TemplateId;
				break;
			case DJSD_GongFaEntity exchange:
				methodId = exchange.Id;
				break;
			case DST_BuQuanEntity registration:
				methodId = registration.Id;
				break;
			default:
				return fallback;
			}
			Sprite cover = CatalogIconArt.Method(methodId);
			return cover != null ? cover : fallback;
		}
	}
}
