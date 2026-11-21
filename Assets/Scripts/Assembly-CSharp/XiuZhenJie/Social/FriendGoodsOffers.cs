using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XiuZhenJie
{
	internal static class FriendGoodsOffers
	{
		private const int CapabilityField = 49100;
		private sealed class Offer
		{
			internal string Id;
			internal string Sender;
			internal string SenderId;
			internal bool Accepted;
			internal readonly List<string> Items = new List<string>();
		}

		private static readonly List<Offer> pending = new List<Offer>();
		private static readonly List<Offer> receipts = new List<Offer>();
		private static string owner;
		private static int generation;
		private static float nextCheck;
		private static float nextPrompt;
		private static bool refreshing;
		private static bool deciding;

		internal static bool Enabled => owner != null && Singleton<RoleManager>.Instance.currentRoleId == owner;

		internal static void Configure(string roleId, ByteString login)
		{
			++generation;
			pending.Clear();
			receipts.Clear();
			owner = null;
			refreshing = deciding = false;
			nextCheck = nextPrompt = 0f;
			var input = login.CreateCodedInput();
			uint tag;
			while ((tag = input.ReadTag()) != 0)
			{
				if (tag == WireFormat.MakeTag(CapabilityField, WireFormat.WireType.Varint))
				{
					if (input.ReadBool())
						owner = roleId;
					return;
				}
				input.SkipLastField();
			}
		}

		internal static void AttachToken(RemoteSendGoodsReq_1014005 request, string token)
		{
			byte[] extra = new byte[1 + CodedOutputStream.ComputeStringSize(token)];
			var output = new CodedOutputStream(extra);
			output.WriteTag(3, WireFormat.WireType.LengthDelimited);
			output.WriteString(token);
			output.Flush();
			request.MergeFrom(extra);
		}

		internal static void RequestRefresh() => nextCheck = 0f;

		internal static void Update()
		{
			if (!Enabled || SceneManager.GetActiveScene().name != "MainScene")
				return;
			var ui = Singleton<UIManager>.Instance;
			var chat = ui.FindWndByName<DaoYouChatWnd>("DaoYouChatWnd.prefab");
			var roster = ui.FindWndByName<DaoYouWnd>("DaoYouWnd.prefab");
			bool insideChat = ui.IsFrontmost(chat);
			if (!insideChat && !ui.IsFrontmost(roster))
				return;
			float now = Time.unscaledTime;
			if (now >= nextCheck && !refreshing)
			{
				nextCheck = now + 25f;
				int session = generation;
				refreshing = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.FriendOfferList, null, data =>
				{
					if (session != generation)
						return;
					refreshing = false;
					if (data != null && Enabled)
						ReadList(data);
				});
			}

			if (deciding || now < nextPrompt ||
				ui.FindWndByName<Window>("AttentionWnd.prefab") != null)
				return;

			string target = insideChat ? chat.FriendRoleId : null;
			Offer receipt = receipts.Find(value => target == null || value.SenderId == target);
			if (receipt != null)
			{
				PresentReceipt(receipt);
				return;
			}

			Offer offer = pending.Find(value => target == null || value.SenderId == target);
			if (offer == null)
				return;
			var game = MonoSingleton<GameStart>.Instance;
			string body = offer.Sender + " 向你传来以下物品\n" + string.Join("  ·  ", offer.Items);
			game.PopUpAttention(body, () => Decide(offer, true), title: "道友传物");
			ui.FindWndByName<AttentionWnd>("AttentionWnd.prefab")?.SetChoices("接受", "放弃", () => Decide(offer, false));
		}

		private static void PresentReceipt(Offer receipt)
		{
			string result = receipt.Accepted ? "已接受你的传物" : "已放弃你的传物";
			string change = receipt.Accepted ? "确认后从纳戒扣除已送出的数量。" : "确认后解除纳戒中的物品锁定。";
			string body = receipt.Sender + " " + result + "\n" + string.Join("  ·  ", receipt.Items) + "\n" + change;
			MonoSingleton<GameStart>.Instance.PopUpAttention(body, () => Acknowledge(receipt),
				() => nextPrompt = Time.unscaledTime + 20f, title: "传物回执");
			var window = Singleton<UIManager>.Instance.FindWndByName<AttentionWnd>("AttentionWnd.prefab");
			if (window != null)
				window.Panel.ContentText.supportRichText = false;
		}

		private static void Decide(Offer offer, bool accept)
		{
			if (deciding || !Enabled)
				return;
			int session = generation;
			var request = new SendMessageRoleReq_4001007 { RoleId = offer.Id, Msg = accept ? "accept" : "decline" };
			deciding = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.FriendOfferDecide, request, data =>
			{
				if (session != generation)
					return;
				deciding = false;
				if (!Enabled)
					return;
				var response = data == null ? null : ActionBoolResp_1001001.Parser.ParseFrom(data);
				if (response?.IsSuccess == true)
				{
					pending.RemoveAll(value => value.Id == offer.Id);
					if (accept)
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					Show(accept ? "已接受传物，物品已放入纳戒。" : "已放弃这份传物。");
					RequestRefresh();
					return;
				}
				if (response?.Code == 44901)
				{
					pending.RemoveAll(value => value.Id == offer.Id);
					RequestRefresh();
				}
				nextPrompt = Time.unscaledTime + 20f;
				Show(response?.Code == 4527 ? "纳戒空间不足，整理后可以再接受。" :
					response?.Code == 44901 ? "这份传物已处理或已过期。" :
					response?.Code == 4396 ? "道友的物品已发生变动，可放弃这份传物。" :
					"暂时无法完成传物，请稍后重试。");
			});
			if (!deciding)
			{
				nextPrompt = Time.unscaledTime + 10f;
				Show("连接暂不可用，稍后会重新显示这份传物。");
			}
		}

		private static void Acknowledge(Offer receipt)
		{
			if (deciding || !Enabled)
				return;
			int session = generation;
			var request = new SendMessageRoleReq_4001007 { RoleId = receipt.Id };
			deciding = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.FriendOfferAck, request, data =>
			{
				if (session != generation)
					return;
				deciding = false;
				if (!Enabled)
					return;
				var response = data == null ? null : RemoteSendGoodsResp_1014005.Parser.ParseFrom(data);
				if (response?.IsSuccess != true)
				{
					nextPrompt = Time.unscaledTime + 20f;
					Show("暂时无法确认传物回执，请稍后再试。");
					return;
				}
				receipts.RemoveAll(value => value.Id == receipt.Id);
				Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = response.CurrentShinyStone;
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
				Show(receipt.Accepted ? "已清点纳戒，送出的物品已移除。" : "传物已结束，物品已解锁。");
				RequestRefresh();
			});
			if (!deciding)
			{
				nextPrompt = Time.unscaledTime + 10f;
				Show("连接暂不可用，稍后会重新显示回执。");
			}
		}

		private static void Show(string text) => MonoSingleton<GameStart>.Instance.PopRollingTextShow(text);

		private static void ReadList(ByteString data)
		{
			var input = data.CreateCodedInput();
			var offers = new List<Offer>();
			var decisions = new List<Offer>();
			bool success = false;
			uint tag;
			while ((tag = input.ReadTag()) != 0)
			{
				if (tag == 8)
					success = input.ReadBool();
				else if (tag == 18)
					offers.Add(ReadOffer(input.ReadBytes().CreateCodedInput()));
				else if (tag == 26)
					decisions.Add(ReadOffer(input.ReadBytes().CreateCodedInput()));
				else
					input.SkipLastField();
			}
			if (!success)
				return;
			pending.Clear();
			pending.AddRange(offers.Where(offer => !string.IsNullOrEmpty(offer.Id) && offer.Items.Count > 0));
			receipts.Clear();
			receipts.AddRange(decisions.Where(offer => !string.IsNullOrEmpty(offer.Id) && offer.Items.Count > 0));
		}

		private static Offer ReadOffer(CodedInputStream input)
		{
			var offer = new Offer();
			uint tag;
			while ((tag = input.ReadTag()) != 0)
			{
				switch (tag)
				{
					case 10: offer.Id = input.ReadString(); break;
					case 18: offer.Sender = input.ReadString(); break;
					case 26: offer.Items.Add(ReadItem(input.ReadBytes().CreateCodedInput())); break;
					case 34: offer.SenderId = input.ReadString(); break;
					case 40: offer.Accepted = input.ReadBool(); break;
					default: input.SkipLastField(); break;
				}
			}
			return offer;
		}

		private static string ReadItem(CodedInputStream input)
		{
			string name = "";
			long count = 0;
			uint tag;
			while ((tag = input.ReadTag()) != 0)
			{
				if (tag == 10) name = input.ReadString();
				else if (tag == 16) count = input.ReadInt64();
				else input.SkipLastField();
			}
			return $"{name} ×{count}";
		}
	}
}
