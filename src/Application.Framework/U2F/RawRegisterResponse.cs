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
using Application.Framework.Utils;
using Org.BouncyCastle.X509;

namespace Application.Framework
{
    public class RawRegisterResponse
    {
        private const byte RegistrationReservedByteValue = 0x05;
        private const byte RegistrationSignedReservedByteValue = 0x00;

        // The (uncompressed) x,y-representation of a curve point on the P-256 NIST elliptic curve.
        private readonly byte[] _userPublicKey;

        // A handle that allows the U2F token to identify the generated key pair.
        private readonly byte[] _keyHandle;

        private readonly X509Certificate _attestationCertificate;

        // A ECDSA signature (on P-256) 
        private readonly byte[] _signature;

        public RawRegisterResponse(byte[] userPublicKey, byte[] keyHandle,
                                   X509Certificate attestationCertificate, byte[] signature)
        {
            _userPublicKey = userPublicKey;
            _keyHandle = keyHandle;
            _attestationCertificate = attestationCertificate;
            _signature = signature;
        }

        public static RawRegisterResponse FromBase64(string rawDataBase64)
        {
            if (string.IsNullOrWhiteSpace(rawDataBase64))
            {
                return null;
            }
            else
            {
                byte[] bytes = rawDataBase64.Base64StringToByteArray();

                Stream stream = new MemoryStream(bytes);
                BinaryReader binaryReader = new BinaryReader(stream);

                try
                {
                    byte reservedByte = binaryReader.ReadByte();
                    if (reservedByte != RegistrationReservedByteValue)
                    {
                        return null;
                    }

                    byte[] publicKey = binaryReader.ReadBytes(65);
                    byte[] keyHandle = binaryReader.ReadBytes(binaryReader.ReadByte());
                    X509CertificateParser x509CertificateParser = new X509CertificateParser();
                    X509Certificate attestationCertificate = x509CertificateParser.ReadCertificate(stream);
                    int size = (int)(binaryReader.BaseStream.Length - binaryReader.BaseStream.Position);


                    byte[] signature = binaryReader.ReadBytes(size);

                    RawRegisterResponse rawRegisterResponse = new RawRegisterResponse(
                        publicKey,
                        keyHandle,
                        attestationCertificate,
                        signature);

                    return rawRegisterResponse;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    stream.Dispose();
                    binaryReader.Dispose();
                }
            }
        }

        public void CheckSignature(string appId, string clientData)
        {
            if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(clientData))
                throw new ArgumentException("Invalid argument(s) were being passed.");

            byte[] signedBytes = PackBytesToSign(
                U2F.Crypto.Hash(appId),
                U2F.Crypto.Hash(clientData),
                _keyHandle,
                _userPublicKey);

            U2F.Crypto.CheckSignature(_attestationCertificate, signedBytes, _signature);
        }

        public DeviceRegistration CreateDevice()
        {
            return new DeviceRegistration(
                _keyHandle,
                _userPublicKey,
                _attestationCertificate.GetEncoded(),
                DeviceRegistration.InitialCounterValue);
        }

        public byte[] PackBytesToSign(byte[] appIdHash,
            byte[] clientDataHash, byte[] keyHandle, byte[] userPublicKey)
        {
            List<byte> someBytes = new List<byte>();
            someBytes.Add(RegistrationSignedReservedByteValue);
            someBytes.AddRange(appIdHash);
            someBytes.AddRange(clientDataHash);
            someBytes.AddRange(keyHandle);
            someBytes.AddRange(userPublicKey);

            return someBytes.ToArray();
        }

        public override int GetHashCode()
        {
            int hash = 23 + _userPublicKey.Sum(b => b + 31);
            hash += _keyHandle.Sum(b => b + 31);
            hash += _signature.Sum(b => b + 31);

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RawRegisterResponse))
                return false;
            if (this == obj)
                return true;
            if (GetType() != obj.GetType())
                return false;
            RawRegisterResponse other = (RawRegisterResponse)obj;
            if (!_attestationCertificate.Equals(other._attestationCertificate))
                return false;
            if (!_keyHandle.SequenceEqual(other._keyHandle))
                return false;
            if (!_signature.SequenceEqual(other._signature))
                return false;
            return _userPublicKey.SequenceEqual(other._userPublicKey);
        }
    }
}
