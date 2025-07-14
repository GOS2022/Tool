using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOSTool
{
    public enum SvlMdiVariableType
    {
        Byte,
        Word,
        LWord,
        Float
    }

    public class SvlMdiVariable
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public SvlMdiVariableType Type { get; private set; }

        public const UInt16 ExpectedSize = 21;

        public void GetFromBytes(byte[] bytes)
        {
            if ((bytes != null) && (bytes.Length >= ExpectedSize))
            {
                int idx = 0;
                Name = Helper.GetString(bytes, 16, ref idx);
                Type = (SvlMdiVariableType)Enum.Parse(typeof(SvlMdiVariableType), Helper<byte>.GetVariable(bytes, ref idx).ToString());

                switch (Type)
                {
                    case SvlMdiVariableType.Byte:
                    case SvlMdiVariableType.Word:
                    case SvlMdiVariableType.LWord:
                        {
                            Value = Helper<UInt32>.GetVariable(bytes, ref idx).ToString();
                            break;
                        }
                    case SvlMdiVariableType.Float:
                        {
                            Value = BitConverter.ToSingle(bytes, idx).ToString();
                            break;
                        }
                    default: break;
                }
            }
        }
    }
}
