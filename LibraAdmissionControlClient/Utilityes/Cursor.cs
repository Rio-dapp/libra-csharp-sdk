using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.Utilityes
{
    public class Cursor
    {
        public byte[] RawByte { get; }
        public int CursorPosition { get; set; }

        public Cursor(byte[] rawByte)
        {
            RawByte = rawByte;
        }

        public byte[] GetNextU32()
        {
            return new byte[] { };
        }

    }
}
