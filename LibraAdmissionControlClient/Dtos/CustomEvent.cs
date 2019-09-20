using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient
{
    public class CustomEvent
    {
        byte[] _rawBytes;
        public CustomEvent()
        {
        }

        public CustomEvent(byte[] rawBytes)
        {
            _rawBytes = rawBytes;
            DeserializeEvent(_rawBytes);
        }

        private void DeserializeEvent(byte[] rawBytes)
        {
           
        }
    }
}
