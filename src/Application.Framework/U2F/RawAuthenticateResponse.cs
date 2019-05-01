/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application.Framework.Exceptions;
using Application.Framework.Utils;

namespace Application.Framework
{
    public class RawAuthenticateResponse
    {
        private const byte UserPresentFlag = 0x01;

        /// <summary>
        /// Initializes a new instance of the <see cref="RawAuthenticateResponse"/> class.
        /// </summary>
        /// <param name="userPresence">The user presence.</param>
        /// <param name="counter">The counter.</param>
        /// <param name="signature">The signature.</param>
        public RawAuthenticateResponse(byte userPresence, uint counter, byte[] signature)
        {
            UserPresence = userPresence;
            Counter = counter;
            Signature = signature;
        }

        public byte UserPresence { get; private set; }
        public uint Counter { get; private set; }
        public byte[] Signature { get; private set; }

        public static RawAuthenticateResponse FromBase64(string rawDataBase64)
        {
            byte[] bytes = rawDataBase64.Base64StringToByteArray();

            Stream stream = new MemoryStream(bytes);
            BinaryReader binaryReader = new BinaryReader(stream);

            byte userPresence = binaryReader.ReadByte();
            byte[] counterBytes = binaryReader.ReadBytes(4);

            //counter has to be reversed if its little endian encoded
            if (BitConverter.IsLittleEndian)
                Array.Reverse(counterBytes);

            uint counter = BitConverter.ToUInt32(counterBytes, 0);

            long size = binaryReader.BaseStream.Length - binaryReader.BaseStream.Position;
            byte[] signature = binaryReader.ReadBytes((int)size);

            try
            {
                return new RawAuthenticateResponse(
                    userPresence,
                    counter,
                    signature);
            }
            finally
            {
                stream.Dispose();
                binaryReader.Dispose();
            }
        }

        public void CheckSignature(string appId, string clientData, byte[] publicKey)
        {
            byte[] signedBytes = PackBytesToSign(
                U2F.Crypto.Hash(appId),
                UserPresence,
                Counter,
                U2F.Crypto.Hash(clientData));

            U2F.Crypto.CheckSignature(
                U2F.Crypto.DecodePublicKey(publicKey),
                signedBytes,
                Signature);
        }

        /// <summary>
        /// Packs the bytes to sign.
        /// </summary>
        /// <param name="appIdHash">The application identifier hash.</param>
        /// <param name="userPresence">The user presence.</param>
        /// <param name="counter">The counter.</param>
        /// <param name="challengeHash">The challenge hash.</param>
        /// <returns></returns>
        public byte[] PackBytesToSign(byte[] appIdHash, byte userPresence, uint counter, byte[] challengeHash)
        {
            // covert the counter to a byte array in case the int is to big for a single byte
            byte[] counterBytes = BitConverter.GetBytes(counter);

            //counter has to be reversed if its little endian encoded
            if (BitConverter.IsLittleEndian)
                Array.Reverse(counterBytes);

            List<byte> someBytes = new List<byte>();
            someBytes.AddRange(appIdHash);
            someBytes.Add(userPresence);
            someBytes.AddRange(counterBytes);
            someBytes.AddRange(challengeHash);

            return someBytes.ToArray();
        }

        public void CheckUserPresence()
        {
            if (UserPresence != UserPresentFlag)
            {
                throw new U2fException("User presence invalid during authentication");
            }
        }

        public override int GetHashCode()
        {
            return 23 + Signature.Sum(b => b + 31 + (int)Counter + UserPresence);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RawAuthenticateResponse))
                return false;
            if (this == obj)
                return true;
            if (GetType() != obj.GetType())
                return false;
            RawAuthenticateResponse other = (RawAuthenticateResponse)obj;
            if (Counter != other.Counter)
                return false;

            if (!Signature.SequenceEqual(other.Signature))
                return false;
            return UserPresence == other.UserPresence;
        }
    }
}
