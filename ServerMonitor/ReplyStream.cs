using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor
{
    class ReplyStream
    {
        private BinaryReader _stream;

        public ReplyStream(Stream stream)
        {
            _stream = new BinaryReader(stream);
        }

        public byte ReadByte()
        {
            return _stream.ReadByte();
        }

        public int ReadUnsignedByte()
        {
            int data = _stream.ReadByte();
            if (data < 0)
            {
                data += 256;
            }
            return data;
        }


        public float ReadFloat()
        {
            return _stream.ReadSingle();
        }

        public int ReadInt()
        {
            return _stream.ReadInt32();
        }

        public short ReadShort()
        {
            return _stream.ReadInt16();
        }

        public String ReadString()
        {
            byte[] buff = new byte[1400];

            int x;
            for (x = 0; x < buff.Length; ++x)
            {
                buff[x] = _stream.ReadByte();
                if (buff[x] == 0)
                {
                    break;
                }
            }

            var str = Encoding.UTF8.GetString(buff, 0, x);
            return str;
        }

        public new String ToString()
        {
            return "";
        }
        


    }
}
