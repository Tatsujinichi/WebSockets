using System;

namespace ClientServerWpf
{
    public class ByteArrayEventArgs : EventArgs
    {
        public byte[] BufferBytes { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }

        public ByteArrayEventArgs(byte[] bytes, int offset, int length)
        {
            BufferBytes = bytes;
            Offset = offset;
            Length = length;
        }
    }
}