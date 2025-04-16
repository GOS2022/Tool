using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOSTool
{
    public enum SdhBinaryDownloadRequestResult
    {
        COMM_ERR = 0,
        OK = 1,
        DESCRIPTOR_SIZE_ERR = 2,
        FILE_SIZE_ERR = 4
    }
    public class SdhBinaryDescriptorMessage
    {
        public string Name { get; set; }
        public DateTime InstallDate { get; set; } = new DateTime();
        public UInt32 Location { get; set; }
        public UInt32 StartAddress { get; set; }
        public UInt32 Size { get; set; }
        public UInt32 Crc { get; set; }

        public const UInt16 ExpectedSize = 16;

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            if (bytes.Length >= ExpectedSize)
            {
                int idx = 0;
                Name = Helper.GetString(bytes, 32, ref idx);
                InstallDate = Helper.GetTime(bytes, ref idx);
                Location = Helper<UInt32>.GetVariable(bytes, ref idx);
                StartAddress = Helper<UInt32>.GetVariable(bytes, ref idx);
                Size = Helper<UInt32>.GetVariable(bytes, ref idx);
                Crc = Helper<UInt32>.GetVariable(bytes, ref idx);
            }
        }

        /// <summary>
        /// Returns the message as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(Helper.GetBytes(Name, 32));
            bytes.AddRange(Helper.GetBytes(InstallDate));
            bytes.AddRange(Helper<UInt32>.GetBytes(Location));
            bytes.AddRange(Helper<UInt32>.GetBytes(StartAddress));
            bytes.AddRange(Helper<UInt32>.GetBytes(Size));
            bytes.AddRange(Helper<UInt32>.GetBytes(Crc));

            return bytes.ToArray();
        }
    }
    public class SvlSdh
    {
        public static EventHandler<(int, int)> BinaryDownloadProgressEvent;
        public static int GetBinaryNum()
        {
            byte[] recvBuf;
            int binaryNum = 0;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = (UInt16)SysmonMessageId.SVL_SDH_SYSMON_MSG_BINARY_NUM_REQ;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 0;

            SysmonFunctions.SemaphoreWait();

            if (GCP.TransmitMessage(0, messageHeader, new byte[] { }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 2000) == true)
                {
                    binaryNum = ((recvBuf[1] << 8) + recvBuf[0]);
                }
            }
            Thread.Sleep(10);

            SysmonFunctions.SemaphoreRelease();

            return binaryNum;
        }

        public static SdhBinaryDescriptorMessage GetBinaryInfo(int index)
        {
            byte[] recvBuf;
            SdhBinaryDescriptorMessage binaryInfo = new SdhBinaryDescriptorMessage();
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = (UInt16)SysmonMessageId.SVL_SDH_SYSMON_MSG_BINARY_INFO_REQ;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 2;

            SysmonFunctions.SemaphoreWait();

            if (GCP.TransmitMessage(0, messageHeader, new byte[] { (byte)(index), (byte)((int)index >> 8), }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 2000) == true)
                {
                    binaryInfo.GetFromBytes(recvBuf);
                }
            }
            Thread.Sleep(10);

            SysmonFunctions.SemaphoreRelease();

            return binaryInfo;
        }

        public static bool SendInstallRequest(int index)
        {
            byte[] recvBuf;
            bool retval = false;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = (UInt16)SysmonMessageId.SVL_SDH_SYSMON_MSG_SOFTWARE_INSTALL_REQ;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 2;

            SysmonFunctions.SemaphoreWait();

            if (GCP.TransmitMessage(0, messageHeader, new byte[] { (byte)(index), (byte)((int)index >> 8), }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 2000) == true)
                {
                    int idx = 0;
                    int rxIdx = Helper<UInt16>.GetVariable(recvBuf, ref idx);

                    if (rxIdx == index)
                        retval = true;
                }
            }
            Thread.Sleep(10);

            SysmonFunctions.SemaphoreRelease();

            return retval;
        }

        public static bool SendEraseRequest(int index)
        {
            byte[] recvBuf;
            bool retval = false;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = (UInt16)SysmonMessageId.SVL_SDH_SYSMON_MSG_BINARY_ERASE_REQ;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 3;

            SysmonFunctions.SemaphoreWait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { (byte)(index), (byte)((int)index >> 8), 73 }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 5000) == true)
                {
                    int idx = 0;
                    int rxIdx = Helper<UInt16>.GetVariable(recvBuf, ref idx);

                    if (rxIdx == index)
                        retval = true;
                }
            }
            Thread.Sleep(10);
            SysmonFunctions.SemaphoreRelease();

            return retval;
        }

        public static bool SendBinary(List<byte> bytes)
        {
            byte[] recvBuf;
            bool res = false;
            int chunkSize = 1024;

            ChunkDescriptor chunkDesc = new ChunkDescriptor();
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            int chunks = bytes.Count / chunkSize + (bytes.Count % chunkSize == 0 ? 0 : 1);

            for (int chunkCounter = 0; chunkCounter < chunks; chunkCounter++)
            {
                chunkDesc.ChunkIndex = (UInt16)chunkCounter;

                messageHeader.MessageId = (UInt16)SysmonMessageId.SVL_SDH_SYSMON_MSG_BINARY_CHUNK_REQ;
                messageHeader.ProtocolVersion = 1;
                messageHeader.PayloadSize = (UInt16)(chunkDesc.GetBytes().Length + chunkSize);

                List<byte> payload = new List<byte>();
                payload.AddRange(chunkDesc.GetBytes());

                if (bytes.Skip(chunkCounter * chunkSize).ToArray().Length >= chunkSize)
                {
                    payload.AddRange(bytes.Skip(chunkCounter * chunkSize).Take(chunkSize));
                }
                else
                {
                    payload.AddRange(bytes.Skip(chunkCounter * chunkSize).Take(bytes.Skip(chunkCounter * chunkSize).ToArray().Length));
                }
                SysmonFunctions.SemaphoreWait();

                Thread.Sleep(50);

                if (GCP.TransmitMessage(0, messageHeader, payload.ToArray(), 0xffff) == true)
                {
                    if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 5000) == true)
                    {
                        chunkDesc.GetFromBytes(recvBuf);

                        if (chunkDesc.Result != 1)
                        {
                            res = false;
                            SysmonFunctions.SemaphoreRelease();
                            break;
                        }
                        else
                        {
                            res = true;
                            BinaryDownloadProgressEvent?.Invoke(null, (chunkCounter + 1, chunks));
                        }
                    }
                    else
                    {
                        res = false;
                        SysmonFunctions.SemaphoreRelease();
                        break;
                    }
                }
                else
                {
                    res = false;
                    SysmonFunctions.SemaphoreRelease();
                    break;
                }

                SysmonFunctions.SemaphoreRelease();
            }

            Thread.Sleep(1000);

            return res;
        }

        public static SdhBinaryDownloadRequestResult SendBinaryDownloadRequest(SdhBinaryDescriptorMessage binaryDescriptor)
        {
            byte[] recvBuf;
            SdhBinaryDownloadRequestResult result = SdhBinaryDownloadRequestResult.COMM_ERR;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = (UInt16)SysmonMessageId.SVL_SDH_SYSMON_MSG_DOWNLOAD_REQ;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = (UInt16)binaryDescriptor.GetBytes().Length;

            SysmonFunctions.SemaphoreWait();

            Thread.Sleep(10);

            if (GCP.TransmitMessage(0, messageHeader, binaryDescriptor.GetBytes(), 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 2000) == true)
                {
                    result = (SdhBinaryDownloadRequestResult)recvBuf[0];
                }
            }

            SysmonFunctions.SemaphoreRelease();

            return result;
        }
    }
}
