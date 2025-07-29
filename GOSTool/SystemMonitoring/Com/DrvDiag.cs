using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOSTool.SystemMonitoring.Com
{
    public class DrvDiag
    {
        public DrvUartDiag UartDiag { get; set; } = new DrvUartDiag();
        public DrvTmrDiag TmrDiag { get; set; } = new DrvTmrDiag();
        public DrvSpiDiag SpiDiag { get; set; } = new DrvSpiDiag();
        public DrvI2cDiag I2cDiag { get; set; } = new DrvI2cDiag();
        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;

            if (!(bytes is null))
            {
                UartDiag.GetFromBytes(bytes.Skip(idx).Take(DrvUartDiag.ExpectedSize).ToArray());
                idx += DrvUartDiag.ExpectedSize;
                TmrDiag.GetFromBytes(bytes.Skip(idx).Take(DrvTmrDiag.ExpectedSize).ToArray());
                idx += DrvTmrDiag.ExpectedSize;
                SpiDiag.GetFromBytes(bytes.Skip(idx).Take(DrvSpiDiag.ExpectedSize).ToArray());
                idx += DrvSpiDiag.ExpectedSize;
                I2cDiag.GetFromBytes(bytes.Skip(idx).Take(DrvI2cDiag.ExpectedSize).ToArray());
            }
        }

    }

    public class DrvUartDiag
    {
        public const int ExpectedSize = 4 + 6 * 1 + 6 * 4 * 5;
        public UInt32 GlobalErrorFlags { get; set; }
        public List<bool> InstanceInitialized { get; set; } = new List<bool>();
        public List<UInt32> InstanceErrorFlags { get; set; } = new List<UInt32>();
        public List<UInt32> ParityErrorCntr { get; set; } = new List<UInt32>();
        public List<UInt32> NoiseErrorCntr { get; set; } = new List<UInt32>();
        public List<UInt32> OverrunErrorCntr { get; set; } = new List<UInt32>();
        public List<UInt32> FramingErrorCntr { get; set; } = new List<UInt32>();

        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;

            if (!(bytes is null))
            {
                GlobalErrorFlags = Helper<UInt32>.GetVariable(bytes, ref idx);
                InstanceInitialized = new List<bool>();
                for (int i = 0; i < 6; i++)
                {
                    InstanceInitialized.Add(Helper<bool>.GetVariable(bytes, ref idx));
                }
                InstanceErrorFlags = new List<UInt32>();
                for (int i = 0; i < 6; i++)
                {
                    InstanceErrorFlags.Add(Helper<UInt32>.GetVariable(bytes, ref idx));
                }
                ParityErrorCntr = new List<UInt32>();
                for (int i = 0; i < 6; i++)
                {
                    ParityErrorCntr.Add(Helper<UInt32>.GetVariable(bytes, ref idx));
                }
                NoiseErrorCntr = new List<UInt32>();
                for (int i = 0; i < 6; i++)
                {
                    NoiseErrorCntr.Add(Helper<UInt32>.GetVariable(bytes, ref idx));
                }
                OverrunErrorCntr = new List<UInt32>();
                for (int i = 0; i < 6; i++)
                {
                    OverrunErrorCntr.Add(Helper<UInt32>.GetVariable(bytes, ref idx));
                }
                FramingErrorCntr = new List<UInt32>();
                for (int i = 0; i < 6; i++)
                {
                    FramingErrorCntr.Add(Helper<UInt32>.GetVariable(bytes, ref idx));
                }
            }
        }
    }

    public class DrvTmrDiag
    {
        public const int ExpectedSize = 4 + 14 * 1 + 14 * 4;
        public UInt32 GlobalErrorFlags { get; set; }
        public List<bool> InstanceInitialized { get; set; } = new List<bool>();
        public List<UInt32> InstanceErrorFlags { get; set; } = new List<UInt32>();
        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;

            if (!(bytes is null))
            {
                GlobalErrorFlags = Helper<UInt32>.GetVariable(bytes, ref idx);
                InstanceInitialized = new List<bool>();
                for (int i = 0; i < 14; i++)
                {
                    InstanceInitialized.Add(Helper<bool>.GetVariable(bytes, ref idx));
                }
                InstanceErrorFlags = new List<UInt32>();
                for (int i = 0; i < 14; i++)
                {
                    InstanceErrorFlags.Add(Helper<UInt32>.GetVariable(bytes, ref idx));
                }
            }
        }
    }

    public class DrvSpiDiag
    {
        public const int ExpectedSize = 4 + 4 * 1 + 4 * 4;
        public UInt32 GlobalErrorFlags { get; set; }
        public List<bool> InstanceInitialized { get; set; } = new List<bool>();
        public List<UInt32> InstanceErrorFlags { get; set; } = new List<UInt32>();
        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;

            if (!(bytes is null))
            {
                GlobalErrorFlags = Helper<UInt32>.GetVariable(bytes, ref idx);
                InstanceInitialized = new List<bool>();
                for (int i = 0; i < 4; i++)
                {
                    InstanceInitialized.Add(Helper<bool>.GetVariable(bytes, ref idx));
                }
                InstanceErrorFlags = new List<UInt32>();
                for (int i = 0; i < 4; i++)
                {
                    InstanceErrorFlags.Add(Helper<UInt32>.GetVariable(bytes, ref idx));
                }
            }
        }
    }

    public class DrvI2cDiag
    {
        public const int ExpectedSize = 4 + 3 * 1 + 3 * 4;
        public UInt32 GlobalErrorFlags { get; set; }
        public List<bool> InstanceInitialized { get; set; } = new List<bool>();
        public List<UInt32> InstanceErrorFlags { get; set; } = new List<UInt32>();
        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;

            if (!(bytes is null))
            {
                GlobalErrorFlags = Helper<UInt32>.GetVariable(bytes, ref idx);
                InstanceInitialized = new List<bool>();
                for (int i = 0; i < 3; i++)
                {
                    InstanceInitialized.Add(Helper<bool>.GetVariable(bytes, ref idx));
                }
                InstanceErrorFlags = new List<UInt32>();
                for (int i = 0; i < 3; i++)
                {
                    InstanceErrorFlags.Add(Helper<UInt32>.GetVariable(bytes, ref idx));
                }
            }
        }
    }
}
