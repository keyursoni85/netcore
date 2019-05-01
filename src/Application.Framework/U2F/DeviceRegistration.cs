/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Application.Framework
{
    public class DeviceRegistration : BaseModel
    {
        public const uint InitialCounterValue = 0;

        public DeviceRegistration(byte[] keyHandle, byte[] publicKey, byte[] attestationCert, uint counter, bool isCompromised = false)
        {
            KeyHandle = keyHandle;
            PublicKey = publicKey;
            Counter = counter;
            IsCompromised = isCompromised;

            try
            {
                AttestationCert = attestationCert;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsCompromised { get; private set; }

        public byte[] KeyHandle { get; private set; }

        public byte[] PublicKey { get; private set; }

        public byte[] AttestationCert { get; private set; }

        public uint Counter { get; private set; }

        public X509Certificate GetAttestationCertificate()
        {
            return new X509Certificate(AttestationCert);
        }

        public uint CheckAndUpdateCounter(uint clientCounter)
        {
            if (clientCounter <= Counter)
            {
                IsCompromised = true;
            }
            Counter = clientCounter;
            return Counter;
        }

        public override int GetHashCode()
        {
            int hash = PublicKey.Sum(b => b + 31);
            hash += AttestationCert.Sum(b => b + 31);
            hash += KeyHandle.Sum(b => b + 31);
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DeviceRegistration))
                return false;

            DeviceRegistration other = (DeviceRegistration)obj;

            return KeyHandle.SequenceEqual(other.KeyHandle)
                   && PublicKey.SequenceEqual(other.PublicKey)
                   && AttestationCert.SequenceEqual(other.AttestationCert)
                   && (IsCompromised == other.IsCompromised);
        }
    }
}
