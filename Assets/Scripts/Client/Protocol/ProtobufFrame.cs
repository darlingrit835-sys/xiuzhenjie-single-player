using Google.Protobuf;

namespace XiuZhenJie.Client.Protocol
{
    public static class ProtobufFrame
    {
        // The Go and Java servers expect a varint32 length before the protobuf envelope.
        public static byte[] Encode(IMessage message)
        {
            int bodyLength = message.CalculateSize();
            int prefixLength = CodedOutputStream.ComputeRawVarint32Size((uint)bodyLength);
            byte[] frame = new byte[checked(prefixLength + bodyLength)];

            var output = new CodedOutputStream(frame);
            output.WriteUInt32((uint)bodyLength);
            message.WriteTo(output);
            output.CheckNoSpaceLeft();
            return frame;
        }
    }
}
