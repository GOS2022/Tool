//*************************************************************************************************
//
//                            #####             #####             #####
//                          #########         #########         #########
//                         ##                ##       ##       ##
//                        ##                ##         ##        #####
//                        ##     #####      ##         ##           #####
//                         ##       ##       ##       ##                ##
//                          #########         #########         #########
//                            #####             #####             #####
//
//                                      (c) Ahmed Gazar, 2023
//
//*************************************************************************************************
//! @file       GCP.cs
//! @author     Ahmed Gazar
//! @date       2023-09-22
//! @version    1.0
//!
//! @brief      GOS General Communication Protocol handler.
//! @details    Implements the GCP frame and message layers.
//*************************************************************************************************
// History
// ------------------------------------------------------------------------------------------------
// Version    Date          Author          Description
// ------------------------------------------------------------------------------------------------
// 1.0        2023-09-22    Ahmed Gazar     Initial version created
//*************************************************************************************************
//
// Copyright (c) 2023 Ahmed Gazar
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//*************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GOSTool
{
    /// <summary>
    /// GCP message header class.
    /// </summary>
    public class GcpMessageHeader
    {
        public UInt16 ProtocolVersion { get; set; }
        public UInt16 MessageId { get; set; }
        public UInt16 PayloadSize { get; set; }
        public UInt32 PayloadCrc { get; set; }

        public const UInt16 ExpectedSize = 10;

        /// <summary>
        /// Fills out the class members from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            if (bytes.Length == 10)
            {
                ProtocolVersion = (UInt16)((bytes[1] << 8) + bytes[0]);
                MessageId = (UInt16)((bytes[3] << 8) + bytes[2]);
                PayloadSize = (UInt16)((bytes[5] << 8) + bytes[4]);
                PayloadCrc = (UInt16)((bytes[9] << 24) + (bytes[8] << 16) + (bytes[7] << 8) + bytes[6]);
            }
        }

        /// <summary>
        /// Returns the header bytes as an array based on the
        /// class member values.
        /// </summary>
        /// <returns></returns>
        public byte[] GetHeaderBytes()
        {
            return new byte[]
            {
                (byte)(ProtocolVersion), (byte)(ProtocolVersion >> 8),
                (byte)(MessageId), (byte)(MessageId >> 8),
                (byte)(PayloadSize), (byte)(PayloadSize >> 8),
                (byte)(PayloadCrc), (byte)(PayloadCrc >> 8), (byte)(PayloadCrc >> 16), (byte)(PayloadCrc >> 24),
            };
        }
    }

    /// <summary>
    /// GCP frame header class.
    /// </summary>
    class GcpHeaderFrame
    {
        public byte ProtocolMajor { get; set; }
        public byte ProtocolMinor { get; set; }
        public byte AckType { get; set; }
        public UInt16 MessageId { get; set; }
        public UInt16 DataSize { get; set; }        
        public UInt32 DataCrc { get; set; }
        public UInt32 HeaderCrc { get; set; }

        public const UInt16 ExpectedFrameSize = 48;

        public const byte ExpectedProtocolVersionMajor = 2;
        public const byte ExpectedProtocolVersionMinor = 0;

        public const UInt16 ExpectedSize = 16;

        /// <summary>
        /// Fills out the class members from the given byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            ProtocolMajor = bytes[0];
            ProtocolMinor = bytes[1];
            AckType = bytes[2];
            var dummy = bytes[3];
            MessageId = (UInt16)((bytes[5] << 8) + bytes[4]);
            DataSize = (UInt16)((bytes[7] << 8) + bytes[6]);
            DataCrc = (UInt32)((bytes[11] << 24) + (bytes[10] << 16) + (bytes[9] << 8) + bytes[8]);
            HeaderCrc = (UInt32)((bytes[15] << 24) + (bytes[14] << 16) + (bytes[13] << 8) + bytes[12]);
        }

        /// <summary>
        /// Returns the header bytes as an array based on the
        /// class member values.
        /// </summary>
        /// <returns></returns>
        public byte[] GetHeaderBytes()
        {
            List<byte> headerBytes = new List<byte>();
            byte[] headerBytesWithoutHeaderCrc = new byte[]
            {
                (byte)(ProtocolMajor), (byte)(ProtocolMinor),
                (byte)(AckType), (byte)(0),
                (byte)(MessageId), (byte)(MessageId >> 8),
                (byte)(DataSize), (byte)(DataSize >> 8),
                (byte)(DataCrc), (byte)(DataCrc >> 8), (byte)(DataCrc >> 16), (byte)(DataCrc >> 24),
            };
            HeaderCrc = Crc.GetCrc32(headerBytesWithoutHeaderCrc);
            headerBytes.AddRange(headerBytesWithoutHeaderCrc);
            headerBytes.AddRange(new byte[] { (byte)(HeaderCrc), (byte)(HeaderCrc >> 8), (byte)(HeaderCrc >> 16), (byte)(HeaderCrc >> 24) });
            return headerBytes.ToArray();
        }
    }

    /// <summary>
    /// GCP static class.
    /// </summary>
    public static class GCP
    {
        /// <summary>
        /// Virtually unused.
        /// </summary>
        //private static UInt16 sessionId;

        /// <summary>
        /// Transmits the given message defined by the header and payload
        /// on the given channel.
        /// </summary>
        /// <param name="channelNumber"></param>
        /// <param name="messageHeader"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static bool TransmitMessage(int channelNumber, GcpMessageHeader messageHeader, byte[] payload)
        {
            // TODO
            Uart.ClearRxBuffer();
            messageHeader.PayloadCrc = Crc.GetCrc32(payload);

            GcpHeaderFrame frameHeader = new GcpHeaderFrame();
            frameHeader.AckType = 0;
            frameHeader.ProtocolMajor = GcpHeaderFrame.ExpectedProtocolVersionMajor;
            frameHeader.ProtocolMinor = GcpHeaderFrame.ExpectedProtocolVersionMinor;
            frameHeader.DataSize = messageHeader.PayloadSize;
            frameHeader.MessageId = messageHeader.MessageId;
            frameHeader.DataCrc = messageHeader.PayloadCrc;
            frameHeader.HeaderCrc = Crc.GetCrc32(frameHeader.GetHeaderBytes().Take(GcpHeaderFrame.ExpectedSize - 4).ToArray());

            GcpHeaderFrame responseHeader = new GcpHeaderFrame();

            if (messageHeader != null && ((payload != null) || (payload == null && messageHeader.PayloadSize == 0u)))
            {
                if (Uart.Send(frameHeader.GetHeaderBytes(), GcpHeaderFrame.ExpectedSize) &&
                    Uart.Send(payload, frameHeader.DataSize) &&
                    Uart.Receive(out byte[] rxBuffer, GcpHeaderFrame.ExpectedSize))
                {
                    responseHeader.GetFromBytes(rxBuffer);

                    if (ValidateHeader(responseHeader, out byte ack) &&
                        responseHeader.AckType == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        private static bool ValidateHeader (GcpHeaderFrame headerFrame, out byte ack)
        {
            if (Crc.GetCrc32(headerFrame.GetHeaderBytes().Take(GcpHeaderFrame.ExpectedSize - 4).ToArray()) == headerFrame.HeaderCrc)
            {
                if (headerFrame.ProtocolMajor == GcpHeaderFrame.ExpectedProtocolVersionMajor &&
                    headerFrame.ProtocolMinor == GcpHeaderFrame.ExpectedProtocolVersionMinor)
                {
                    ack = 0;
                    return true;
                }
                else
                {
                    ack = 5;
                    return false;
                }
            }
            else
            {
                ack = 2;
                return false;
            }
        }

        private static bool ValidateData (GcpHeaderFrame headerFrame, byte[] data, out byte ack)
        {
            if(Crc.GetCrc32(data) ==  headerFrame.DataCrc)
            {
                ack = 0;
                return true;
            }
            else
            {
                ack = 2;
                return false;
            }
        }

        /// <summary>
        /// Receives a message (header + payload) on the given channel.
        /// </summary>
        /// <param name="channelNumber"></param>
        /// <param name="messageHeader"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static bool ReceiveMessage(int channelNumber, out GcpMessageHeader messageHeader, out byte[] payload, int timeout = 250)
        {
            GcpHeaderFrame rxHeader = new GcpHeaderFrame();
            messageHeader = new GcpMessageHeader();
            payload = null;
            GcpHeaderFrame responseHeader = new GcpHeaderFrame();
            responseHeader.DataSize = 0;
            responseHeader.DataCrc = 0;
            responseHeader.ProtocolMajor = GcpHeaderFrame.ExpectedProtocolVersionMajor;
            responseHeader.ProtocolMinor = GcpHeaderFrame.ExpectedProtocolVersionMinor;

            if (Uart.Receive(out byte[] receivedBytes, GcpHeaderFrame.ExpectedSize))
            {
                rxHeader.GetFromBytes(receivedBytes);
                if (ValidateHeader(rxHeader, out byte ack) &&
                    ((rxHeader.DataSize == 0) || (rxHeader.DataSize > 0 && Uart.Receive(out payload, rxHeader.DataSize, timeout))) &&
                    ValidateData(rxHeader, payload, out ack))
                {
                    messageHeader.MessageId = rxHeader.MessageId;
                    messageHeader.PayloadSize = rxHeader.DataSize;
                    messageHeader.PayloadCrc = rxHeader.DataCrc;

                    responseHeader.AckType = 1;
                    responseHeader.HeaderCrc = Crc.GetCrc32(responseHeader.GetHeaderBytes().Take(GcpHeaderFrame.ExpectedSize - 4).ToArray());

                    if (Uart.Send(responseHeader.GetHeaderBytes(), GcpHeaderFrame.ExpectedSize))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    /*if (ack != 2)
                    {
                        // If not CRC error, exit loop.
                        //return false;
                    }
                    else
                    {
                        // In case of CRC error, we will try to receive again.
                        // TODO: temporary
                        //return false;
                    }*/

                    responseHeader.AckType = ack;
                    responseHeader.HeaderCrc = Crc.GetCrc32(responseHeader.GetHeaderBytes().Take(GcpHeaderFrame.ExpectedSize - 4).ToArray());

                    if (Uart.Send(responseHeader.GetHeaderBytes(), GcpHeaderFrame.ExpectedSize))
                    { return false; }
                }
            }

            return false;
        }
    }
}
