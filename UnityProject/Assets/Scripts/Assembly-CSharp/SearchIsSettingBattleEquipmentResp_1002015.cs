using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SearchIsSettingBattleEquipmentResp_1002015 : IMessage<SearchIsSettingBattleEquipmentResp_1002015>, IMessage, IEquatable<SearchIsSettingBattleEquipmentResp_1002015>, IDeepCloneable<SearchIsSettingBattleEquipmentResp_1002015>
{
	private static readonly MessageParser<SearchIsSettingBattleEquipmentResp_1002015> _parser = new MessageParser<SearchIsSettingBattleEquipmentResp_1002015>(() => new SearchIsSettingBattleEquipmentResp_1002015());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int IsSettingEquipmentBattleFieldNumber = 3;

	private bool isSettingEquipmentBattle_;

	[DebuggerNonUserCode]
	public static MessageParser<SearchIsSettingBattleEquipmentResp_1002015> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[18];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Code
	{
		get
		{
			return code_;
		}
		set
		{
			code_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsSuccess
	{
		get
		{
			return isSuccess_;
		}
		set
		{
			isSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsSettingEquipmentBattle
	{
		get
		{
			return isSettingEquipmentBattle_;
		}
		set
		{
			isSettingEquipmentBattle_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchIsSettingBattleEquipmentResp_1002015()
	{
	}

	[DebuggerNonUserCode]
	public SearchIsSettingBattleEquipmentResp_1002015(SearchIsSettingBattleEquipmentResp_1002015 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		isSettingEquipmentBattle_ = other.isSettingEquipmentBattle_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchIsSettingBattleEquipmentResp_1002015 Clone()
	{
		return new SearchIsSettingBattleEquipmentResp_1002015(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchIsSettingBattleEquipmentResp_1002015);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchIsSettingBattleEquipmentResp_1002015 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Code != other.Code)
		{
			return false;
		}
		if (IsSuccess != other.IsSuccess)
		{
			return false;
		}
		if (IsSettingEquipmentBattle != other.IsSettingEquipmentBattle)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
		if (IsSettingEquipmentBattle)
		{
			num ^= IsSettingEquipmentBattle.GetHashCode();
		}
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	public void WriteTo(CodedOutputStream output)
	{
		if (Code != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Code);
		}
		if (IsSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsSuccess);
		}
		if (IsSettingEquipmentBattle)
		{
			output.WriteRawTag(24);
			output.WriteBool(IsSettingEquipmentBattle);
		}
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		if (IsSettingEquipmentBattle)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchIsSettingBattleEquipmentResp_1002015 other)
	{
		if (other != null)
		{
			if (other.Code != 0)
			{
				Code = other.Code;
			}
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
			}
			if (other.IsSettingEquipmentBattle)
			{
				IsSettingEquipmentBattle = other.IsSettingEquipmentBattle;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 8u:
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 24u:
				IsSettingEquipmentBattle = input.ReadBool();
				break;
			}
		}
	}
}
