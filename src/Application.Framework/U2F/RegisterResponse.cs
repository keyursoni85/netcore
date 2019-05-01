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
    public class RegisterResponse : BaseModel
    {
        private readonly ClientData _clientDataRef;
        
        public RegisterResponse(string registrationData, string clientData)
        {
            if (string.IsNullOrWhiteSpace(registrationData) || string.IsNullOrWhiteSpace(clientData))
                throw new ArgumentException("Invalid argument(s) were being passed.");

            RegistrationData = registrationData;
            ClientData = clientData;
            _clientDataRef = new ClientData(ClientData);
        }

        public string RegistrationData { get; private set; }

        public string ClientData { get; private set; }

        public ClientData GetClientData()
        {
            return _clientDataRef;
        }

        public string GetRequestId()
        {
            return GetClientData().Challenge;
        }

        public override int GetHashCode()
        {
            int hash = RegistrationData.Sum(c => c + 31);
            hash += ClientData.Sum(c => c + 31);

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RegisterResponse))
                return false;
            if (this == obj)
                return true;
            if (GetType() != obj.GetType())
                return false;
            RegisterResponse other = (RegisterResponse)obj;
            if (ClientData == null)
            {
                if (other.ClientData != null)
                    return false;
            }
            else if (!ClientData.Equals(other.ClientData))
                return false;
            if (RegistrationData == null)
            {
                if (other.RegistrationData != null)
                    return false;
            }
            else if (!RegistrationData.Equals(other.RegistrationData))
                return false;
            return true;
        }
    }
}
