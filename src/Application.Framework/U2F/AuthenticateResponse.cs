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

namespace Application.Framework
{
    public class AuthenticateResponse : BaseModel
    {
        private readonly ClientData _clientDataRef;
        public AuthenticateResponse(string clientData, string signatureData, string keyHandle)
        {
            if (string.IsNullOrWhiteSpace(clientData)
                || string.IsNullOrWhiteSpace(signatureData)
                || string.IsNullOrWhiteSpace(keyHandle))
                throw new ArgumentException("Invalid argument(s) were being passed.");

            ClientData = clientData;
            SignatureData = signatureData;
            KeyHandle = keyHandle;
            _clientDataRef = new ClientData(ClientData);
        }
        public ClientData GetClientData()
        {
            return _clientDataRef;
        }
        public string GetRequestId()
        {
            return GetClientData().Challenge;
        }
        public string SignatureData { get; private set; }
        public string ClientData { get; private set; }
        public string KeyHandle { get; private set; }
        public override int GetHashCode()
        {
            int hash = ClientData.Sum(c => c + 31);
            hash += SignatureData.Sum(c => c + 31);
            hash += KeyHandle.Sum(c => c + 31);

            return hash;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is AuthenticateResponse))
                return false;
            if (this == obj)
                return true;
            if (GetType() != obj.GetType())
                return false;

            AuthenticateResponse other = (AuthenticateResponse)obj;

            if (ClientData == null)
            {
                if (other.ClientData != null)
                    return false;
            }
            else if (!ClientData.Equals(other.ClientData))
                return false;

            if (KeyHandle == null)
            {
                if (other.KeyHandle != null)
                    return false;
            }
            else if (!KeyHandle.Equals(other.KeyHandle))
                return false;

            if (SignatureData == null)
            {
                if (other.SignatureData != null)
                    return false;
            }
            else if (!SignatureData.Equals(other.SignatureData))
                return false;

            return true;
        }
    }
}
