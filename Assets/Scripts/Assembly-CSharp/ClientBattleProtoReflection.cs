using System;
using Google.Protobuf.Reflection;

public static class ClientBattleProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static ClientBattleProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChdDbGllbnRCYXR0bGVQcm90by5wcm90byJVChpCYXR0bGVSb2xlRW50ZXJSZXFf" + "ODAwMTAwMRIRCglkdW5nZW9uSWQYASABKAkSDgoGcm9sZUlkGAIgASgJEhQKDHdv" + "cmxkVmVyc2lvbhgDIAEoBSLVDgobQmF0dGxlUm9sZUVudGVyUmVzcF84MDAxMDAx" + "EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEg8KB21hcE1pblgYAyAB" + "KAESDwoHbWFwTWF4WBgEIAEoARIPCgdtYXBNaW5ZGAUgASgBEg8KB21hcE1heFkY" + "BiABKAESDAoEcG9zWBgHIAEoARIMCgRwb3NZGAggASgBEg0KBWFuZ2xlGAkgASgB" + "EkQKB29iamVjdHMYCiADKAsyMy5CYXR0bGVSb2xlRW50ZXJSZXNwXzgwMDEwMDEu" + "T2JqZWN0SW5mb1Jlc3BfODAwMTAwMhJGCgZwbGFudHMYCyADKAsyNi5CYXR0bGVS" + "b2xlRW50ZXJSZXNwXzgwMDEwMDEuRHJvcEdvb2RzSW5mb1Jlc3BfODAwMTAxMxJI" + "CghtaW5lcmFscxgMIAMoCzI2LkJhdHRsZVJvbGVFbnRlclJlc3BfODAwMTAwMS5E" + "cm9wR29vZHNJbmZvUmVzcF84MDAxMDEzEhEKCXJvbGVJbmRleBgNIAEoBRJKCgpz" + "ZWNyZXRBcmVhGA4gASgLMjYuQmF0dGxlUm9sZUVudGVyUmVzcF84MDAxMDAxLkRy" + "b3BHb29kc0luZm9SZXNwXzgwMDEwMTMSEAoIb3ZlclRpbWUYDyABKAMSEgoKd29y" + "bGRUaWxlcxgQIAEoDBIQCghyZWdpb25JZBgRIAEoCRIQCghleHBsb3JlZBgSIAEo" + "DBIRCglsYW5kbWFya3MYEyABKAwazggKFk9iamVjdEluZm9SZXNwXzgwMDEwMDIS" + "EwoLb2JqZWN0SW5kZXgYASABKAUSDAoEcG9zWBgCIAEoARIMCgRwb3NZGAMgASgB" + "Eg0KBWFuZ2xlGAQgASgBEhMKC2ZpdmVFbGVtZW50GAUgASgFEhIKCm9iamVjdFR5" + "cGUYBiABKAUSDAoEbmFtZRgHIAEoCRINCgVsZXZlbBgIIAEoBRIeChZjdXJyZW50" + "UGh5c2lxdWVEZWZlbnNlGAkgASgDEh8KF2N1cnJlbnRQdXJlQmxvb2REZWZlbnNl" + "GAogASgFEhcKD2N1cnJlbnRTdXBlckFpchgLIAEoAxIZChFjdXJyZW50UGVyY2Vw" + "dGlvbhgMIAEoBRIYChBjdXJyZW50TW92ZVNwZWVkGA0gASgBEgwKBGljb24YDiAB" + "KAUSHAoUc3VwZXJBaXJSZWNvdmVyVmFsdWUYDyABKAESGwoTZGVmZW5zZVJlY292" + "ZXJWYWx1ZRgQIAEoARIkChxzdXBlckFpclJlY292ZXJWYWx1ZU9uQmF0dGxlGBEg" + "ASgBEiMKG2RlZmVuc2VSZWNvdmVyVmFsdWVPbkJhdHRsZRgSIAEoARIVCg1zdXBl" + "ckFpclBvd2VyGBMgASgBEl8KBnNraWxscxgUIAMoCzJPLkJhdHRsZVJvbGVFbnRl" + "clJlc3BfODAwMTAwMS5PYmplY3RJbmZvUmVzcF84MDAxMDAyLk9iamVjdEluZm9T" + "a2lsbFJlc3BfODAwMTAwMxIWCg5hdHRhY2tEaXN0YW5jZRgVIAEoARIUCgx1bnVz" + "dWFsTGV2ZWwYFiABKAUa5AMKG09iamVjdEluZm9Ta2lsbFJlc3BfODAwMTAwMxIM" + "CgRpY29uGAEgASgFEgwKBHR5cGUYAiABKAUSEQoJc2tpbGxUeXBlGAwgASgFEgwK" + "BG5hbWUYAyABKAkSEgoKdGVtcGxhdGVJZBgEIAEoCRIQCghjYXN0VGltZRgFIAEo" + "BRIUCgxjb250aW51ZVRpbWUYBiABKAUSEAoIY29vbERvd24YByABKAUSEwoLZml2" + "ZUVsZW1lbnQYCCABKAUSFgoOY29udmVyc2lvblJhdGUYCSABKAESEgoKYmFzZURh" + "bWFnZRgKIAEoARIWCg51c2VkUGVyY2VwdGlvbhgLIAEoBRINCgVsZXZlbBgNIAEo" + "BRISCgpza2lsbEluZGV4GA4gASgFEhQKDHVzZWRTdXBlckFpchgPIAEoAxITCgtj" + "YW5Vc2VDb3VudBgQIAEoBRIMCgRyYW5rGBEgASgFEhAKCHVuaXRVc2VkGBIgASgB" + "EhUKDWZpcmVEYW1hZ2VBZGQYEyABKAESFQoNd29vZERhbWFnZUFkZBgUIAEoARIW" + "Cg53YXRlckRhbWFnZUFkZBgVIAEoARIVCg1nb2xkRGFtYWdlQWRkGBYgASgBEhYK" + "DmVhcnRoRGFtYWdlQWRkGBcgASgBGsABChlEcm9wR29vZHNJbmZvUmVzcF84MDAx" + "MDEzEhMKC29iamVjdEluZGV4GAEgASgFEgwKBHBvc1gYAiABKAESDAoEcG9zWRgD" + "IAEoARITCgtmaXZlRWxlbWVudBgFIAEoBRISCgpvYmplY3RUeXBlGAYgASgFEgwK" + "BG5hbWUYByABKAkSDQoFbGV2ZWwYCCABKAUSDAoEcmFuaxgJIAEoBRIPCgdnb29k" + "c0lkGAogASgJEg0KBWNvdW50GAsgASgFIlkKGUJhdHRsZVJvbGVNb3ZlUmVxXzgw" + "MDEwMDQSEQoJcm9sZUluZGV4GAEgASgFEgwKBHBvc1gYAiABKAESDAoEcG9zWRgD" + "IAEoARINCgVhbmdsZRgEIAEoASJhCh1CYXR0bGVSb2xlVXNlU2tpbGxSZXFfODAw" + "MTAwNRIRCglyb2xlSW5kZXgYASABKAUSEgoKc2tpbGxJbmRleBgCIAEoBRIZChF0" + "YXJnZXRPYmplY3RJbmRleBgDIAEoBSJRCh5CYXR0bGVSb2xlVXNlU2tpbGxSZXNw" + "XzgwMDEwMDUSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSDgoGZGFt" + "YWdlGAMgASgDImoKG01vbnN0ZXJHZXREYW1hZ2VSZXFfODAwMTAwNhIUCgxtb25z" + "dGVySW5kZXgYASABKAUSEQoJcm9sZUluZGV4GAIgASgFEhIKCnNraWxsSW5kZXgY" + "AyABKAUSDgoGZGFtYWdlGAQgASgFIj8KHE1vbnN0ZXJHZXREYW1hZ2VSZXNwXzgw" + "MDEwMDYSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgiLwoYUm9sZVBp" + "Y2tHb29kc1JlcV84MDAxMDA3EhMKC29iamVjdEluZGV4GAEgASgFIjwKGVJvbGVQ" + "aWNrR29vZHNSZXNwXzgwMDEwMDcSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MY" + "AiABKAgiQwobUm9sZUxlYXZlRHVuZ2VvblJlcV84MDAxMDA4EhEKCWR1bmdlb25J" + "ZBgBIAEoCRIRCglyb2xlSW5kZXgYAiABKAUiPwocUm9sZUxlYXZlRHVuZ2VvblJl" + "c3BfODAwMTAwOBIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCCIgCh5F" + "bnRlclRvd2VyTmV4dExldmVsUmVxXzgwMDEwMDki4AkKH0VudGVyVG93ZXJOZXh0" + "TGV2ZWxSZXNwXzgwMDEwMDkSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiAB" + "KAgSDwoHbWFwTWluWBgDIAEoARIPCgdtYXBNYXhYGAQgASgBEg8KB21hcE1pblkY" + "BSABKAESDwoHbWFwTWF4WRgGIAEoARIMCgRwb3NYGAcgASgBEgwKBHBvc1kYCCAB" + "KAESDQoFYW5nbGUYCSABKAESSAoHb2JqZWN0cxgKIAMoCzI3LkVudGVyVG93ZXJO" + "ZXh0TGV2ZWxSZXNwXzgwMDEwMDkuT2JqZWN0SW5mb1Jlc3BfODAwMTAxMBIRCgly" + "b2xlSW5kZXgYCyABKAUSEAoIb3ZlclRpbWUYDCABKAMavQcKFk9iamVjdEluZm9S" + "ZXNwXzgwMDEwMTASEwoLb2JqZWN0SW5kZXgYASABKAUSDAoEcG9zWBgCIAEoARIM" + "CgRwb3NZGAMgASgBEg0KBWFuZ2xlGAQgASgBEhMKC2ZpdmVFbGVtZW50GAUgASgF" + "EhIKCm9iamVjdFR5cGUYBiABKAUSDAoEbmFtZRgHIAEoCRINCgVsZXZlbBgIIAEo" + "BRIeChZjdXJyZW50UGh5c2lxdWVEZWZlbnNlGAkgASgDEh8KF2N1cnJlbnRQdXJl" + "Qmxvb2REZWZlbnNlGAogASgFEhcKD2N1cnJlbnRTdXBlckFpchgLIAEoAxIZChFj" + "dXJyZW50UGVyY2VwdGlvbhgMIAEoBRIYChBjdXJyZW50TW92ZVNwZWVkGA0gASgB" + "EgwKBGljb24YDiABKAUSHAoUc3VwZXJBaXJSZWNvdmVyVmFsdWUYDyABKAESGwoT" + "ZGVmZW5zZVJlY292ZXJWYWx1ZRgQIAEoARIkChxzdXBlckFpclJlY292ZXJWYWx1" + "ZU9uQmF0dGxlGBEgASgBEiMKG2RlZmVuc2VSZWNvdmVyVmFsdWVPbkJhdHRsZRgS" + "IAEoARIVCg1zdXBlckFpclBvd2VyGBMgASgBEmMKBnNraWxscxgUIAMoCzJTLkVu" + "dGVyVG93ZXJOZXh0TGV2ZWxSZXNwXzgwMDEwMDkuT2JqZWN0SW5mb1Jlc3BfODAw" + "MTAxMC5PYmplY3RJbmZvU2tpbGxSZXNwXzgwMDEwMTISFgoOYXR0YWNrRGlzdGFu" + "Y2UYFSABKAESFAoMdW51c3VhbExldmVsGBYgASgFGs8CChtPYmplY3RJbmZvU2tp" + "bGxSZXNwXzgwMDEwMTISDAoEaWNvbhgBIAEoBRIMCgR0eXBlGAIgASgFEgwKBG5h" + "bWUYAyABKAkSEgoKdGVtcGxhdGVJZBgEIAEoCRIQCghjYXN0VGltZRgFIAEoBRIU" + "Cgxjb250aW51ZVRpbWUYBiABKAUSEAoIY29vbERvd24YByABKAUSEwoLZml2ZUVs" + "ZW1lbnQYCCABKAUSFgoOY29udmVyc2lvblJhdGUYCSABKAESEgoKYmFzZURhbWFn" + "ZRgKIAEoARINCgVsZXZlbBgNIAEoBRIRCglza2lsbFR5cGUYDCABKAUSFgoOdXNl" + "ZFBlcmNlcHRpb24YCyABKAUSEgoKc2tpbGxJbmRleBgOIAEoBRIUCgx1c2VkU3Vw" + "ZXJBaXIYDyABKAMSEwoLY2FuVXNlQ291bnQYECABKAUiXAoYUm9sZUdldERhbWFn" + "ZVJlcV84MDAxMDE0Eg4KBmJlZm9yZRgBIAEoAxINCgVhZnRlchgCIAEoAxIOCgZk" + "YW1hZ2UYAyABKAMSEQoJcm9sZUluZGV4GAQgASgFIiYKEVJldml2ZVJlcV84MDAx" + "MDExEhEKCXJvbGVJbmRleBgBIAEoBSJsChJSZXZpdmVSZXNwXzgwMDEwMTESDAoE" + "Y29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSGQoRdGlTaGVuS3VpTGVpQ291" + "bnQYAyABKAUSDAoEcG9zWBgEIAEoARIMCgRwb3NZGAUgASgBIswBCh5CYXR0bGVC" + "cm9hZGNhc3RQb3NSZXNwXzgwMDIwMDESTgoMb2JqZWN0UG9zaW5zGAEgAygLMjgu" + "QmF0dGxlQnJvYWRjYXN0UG9zUmVzcF84MDAyMDAxLk9iamVjdFBvc2lvblJlc3Bf" + "ODAwMjAwMhpaChhPYmplY3RQb3Npb25SZXNwXzgwMDIwMDISEwoLb2JqZWN0SW5k" + "ZXgYASABKAUSDAoEcG9zWBgCIAEoARIMCgRwb3NZGAMgASgBEg0KBWFuZ2xlGAQg" + "ASgBIk4KFk1vbnN0ZXJEZWFkUmVxXzgwMDIwMDMSGAoQZGVhZE1vbnN0ZXJJbmRl" + "eBgBIAEoBRIMCgRwb3NYGAIgASgBEgwKBHBvc1kYAyABKAEi1wEKF01vbnN0ZXJE" + "ZWFkUmVzcF84MDAyMDAzEhgKEGRlYWRNb25zdGVySW5kZXgYASABKAUSDwoHZ29v" + "ZHNJZBgCIAEoCRIMCgRwb3NYGAMgASgBEgwKBHBvc1kYBCABKAESEwoLZml2ZUVs" + "ZW1lbnQYBSABKAUSEQoJZ29vZHNUeXBlGAYgASgFEgwKBG5hbWUYByABKAkSDQoF" + "bGV2ZWwYCCABKAUSDAoEcmFuaxgJIAEoBRITCgtvYmplY3RJbmRleBgKIAEoBRIN" + "CgVjb3VudBgLIAEoBUIwChtjb20ubG9uZ21vLmJhdHRsZS5uZXQucHJvdG9CEUNs" + "aWVudEJhdHRsZVByb3RvYgZwcm90bzM="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[19]
		{
			new GeneratedClrTypeInfo(typeof(BattleRoleEnterReq_8001001), BattleRoleEnterReq_8001001.Parser, new string[3] { "DungeonId", "RoleId", "WorldVersion" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BattleRoleEnterResp_8001001), BattleRoleEnterResp_8001001.Parser, new string[19]
			{
				"Code", "IsSuccess", "MapMinX", "MapMaxX", "MapMinY", "MapMaxY", "PosX", "PosY", "Angle", "Objects",
				"Plants", "Minerals", "RoleIndex", "SecretArea", "OverTime", "WorldTiles", "RegionId", "Explored", "Landmarks"
			}, null, null, new GeneratedClrTypeInfo[2]
			{
				new GeneratedClrTypeInfo(typeof(BattleRoleEnterResp_8001001.Types.ObjectInfoResp_8001002), BattleRoleEnterResp_8001001.Types.ObjectInfoResp_8001002.Parser, new string[22]
				{
					"ObjectIndex", "PosX", "PosY", "Angle", "FiveElement", "ObjectType", "Name", "Level", "CurrentPhysiqueDefense", "CurrentPureBloodDefense",
					"CurrentSuperAir", "CurrentPerception", "CurrentMoveSpeed", "Icon", "SuperAirRecoverValue", "DefenseRecoverValue", "SuperAirRecoverValueOnBattle", "DefenseRecoverValueOnBattle", "SuperAirPower", "Skills",
					"AttackDistance", "UnusualLevel"
				}, null, null, new GeneratedClrTypeInfo[1]
				{
					new GeneratedClrTypeInfo(typeof(BattleRoleEnterResp_8001001.Types.ObjectInfoResp_8001002.Types.ObjectInfoSkillResp_8001003), BattleRoleEnterResp_8001001.Types.ObjectInfoResp_8001002.Types.ObjectInfoSkillResp_8001003.Parser, new string[23]
					{
						"Icon", "Type", "SkillType", "Name", "TemplateId", "CastTime", "ContinueTime", "CoolDown", "FiveElement", "ConversionRate",
						"BaseDamage", "UsedPerception", "Level", "SkillIndex", "UsedSuperAir", "CanUseCount", "Rank", "UnitUsed", "FireDamageAdd", "WoodDamageAdd",
						"WaterDamageAdd", "GoldDamageAdd", "EarthDamageAdd"
					}, null, null, null)
				}),
				new GeneratedClrTypeInfo(typeof(BattleRoleEnterResp_8001001.Types.DropGoodsInfoResp_8001013), BattleRoleEnterResp_8001001.Types.DropGoodsInfoResp_8001013.Parser, new string[10] { "ObjectIndex", "PosX", "PosY", "FiveElement", "ObjectType", "Name", "Level", "Rank", "GoodsId", "Count" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(BattleRoleMoveReq_8001004), BattleRoleMoveReq_8001004.Parser, new string[4] { "RoleIndex", "PosX", "PosY", "Angle" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BattleRoleUseSkillReq_8001005), BattleRoleUseSkillReq_8001005.Parser, new string[3] { "RoleIndex", "SkillIndex", "TargetObjectIndex" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BattleRoleUseSkillResp_8001005), BattleRoleUseSkillResp_8001005.Parser, new string[3] { "Code", "IsSuccess", "Damage" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MonsterGetDamageReq_8001006), MonsterGetDamageReq_8001006.Parser, new string[4] { "MonsterIndex", "RoleIndex", "SkillIndex", "Damage" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MonsterGetDamageResp_8001006), MonsterGetDamageResp_8001006.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RolePickGoodsReq_8001007), RolePickGoodsReq_8001007.Parser, new string[1] { "ObjectIndex" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RolePickGoodsResp_8001007), RolePickGoodsResp_8001007.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleLeaveDungeonReq_8001008), RoleLeaveDungeonReq_8001008.Parser, new string[2] { "DungeonId", "RoleIndex" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleLeaveDungeonResp_8001008), RoleLeaveDungeonResp_8001008.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterTowerNextLevelReq_8001009), EnterTowerNextLevelReq_8001009.Parser, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterTowerNextLevelResp_8001009), EnterTowerNextLevelResp_8001009.Parser, new string[12]
			{
				"Code", "IsSuccess", "MapMinX", "MapMaxX", "MapMinY", "MapMaxY", "PosX", "PosY", "Angle", "Objects",
				"RoleIndex", "OverTime"
			}, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterTowerNextLevelResp_8001009.Types.ObjectInfoResp_8001010), EnterTowerNextLevelResp_8001009.Types.ObjectInfoResp_8001010.Parser, new string[22]
				{
					"ObjectIndex", "PosX", "PosY", "Angle", "FiveElement", "ObjectType", "Name", "Level", "CurrentPhysiqueDefense", "CurrentPureBloodDefense",
					"CurrentSuperAir", "CurrentPerception", "CurrentMoveSpeed", "Icon", "SuperAirRecoverValue", "DefenseRecoverValue", "SuperAirRecoverValueOnBattle", "DefenseRecoverValueOnBattle", "SuperAirPower", "Skills",
					"AttackDistance", "UnusualLevel"
				}, null, null, new GeneratedClrTypeInfo[1]
				{
					new GeneratedClrTypeInfo(typeof(EnterTowerNextLevelResp_8001009.Types.ObjectInfoResp_8001010.Types.ObjectInfoSkillResp_8001012), EnterTowerNextLevelResp_8001009.Types.ObjectInfoResp_8001010.Types.ObjectInfoSkillResp_8001012.Parser, new string[16]
					{
						"Icon", "Type", "Name", "TemplateId", "CastTime", "ContinueTime", "CoolDown", "FiveElement", "ConversionRate", "BaseDamage",
						"Level", "SkillType", "UsedPerception", "SkillIndex", "UsedSuperAir", "CanUseCount"
					}, null, null, null)
				})
			}),
			new GeneratedClrTypeInfo(typeof(RoleGetDamageReq_8001014), RoleGetDamageReq_8001014.Parser, new string[4] { "Before", "After", "Damage", "RoleIndex" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ReviveReq_8001011), ReviveReq_8001011.Parser, new string[1] { "RoleIndex" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ReviveResp_8001011), ReviveResp_8001011.Parser, new string[5] { "Code", "IsSuccess", "TiShenKuiLeiCount", "PosX", "PosY" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BattleBroadcastPosResp_8002001), BattleBroadcastPosResp_8002001.Parser, new string[1] { "ObjectPosins" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(BattleBroadcastPosResp_8002001.Types.ObjectPosionResp_8002002), BattleBroadcastPosResp_8002001.Types.ObjectPosionResp_8002002.Parser, new string[4] { "ObjectIndex", "PosX", "PosY", "Angle" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(MonsterDeadReq_8002003), MonsterDeadReq_8002003.Parser, new string[3] { "DeadMonsterIndex", "PosX", "PosY" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MonsterDeadResp_8002003), MonsterDeadResp_8002003.Parser, new string[11]
			{
				"DeadMonsterIndex", "GoodsId", "PosX", "PosY", "FiveElement", "GoodsType", "Name", "Level", "Rank", "ObjectIndex",
				"Count"
			}, null, null, null)
		}));
	}
}
