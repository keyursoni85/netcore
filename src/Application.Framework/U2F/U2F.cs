/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using System.Collections.Generic;
using Application.Framework.Utils;

namespace Application.Framework
{
    public static class U2F
    {
        private static ICryptoService _crypto = new CryptoService();
        public const string U2FVersion = "U2F_V2";
        private const string AuthenticateTyp = "navigator.id.getAssertion";
        private const string RegisterType = "navigator.id.finishEnrollment";

        public static ICryptoService Crypto
        {
            get { return _crypto; }
            private set { _crypto = value; }
        }

        public static StartedRegistration StartRegistration(string appId)
        {
            byte[] challenge = Crypto.GenerateChallenge();
            string challengeBase64 = challenge.ByteArrayToBase64String();

            return new StartedRegistration(challengeBase64, appId);
        }

        public static DeviceRegistration FinishRegistration(StartedRegistration startedRegistration, RegisterResponse tokenResponse, HashSet<string> facets = null)
        {
            ClientData clientData = tokenResponse.GetClientData();
            clientData.CheckContent(RegisterType, startedRegistration.Challenge, facets);

            RawRegisterResponse rawRegisterResponse = RawRegisterResponse.FromBase64(tokenResponse.RegistrationData);
            rawRegisterResponse.CheckSignature(startedRegistration.AppId, clientData.AsJson());

            return rawRegisterResponse.CreateDevice();
        }

        public static StartedAuthentication StartAuthentication(string appId, DeviceRegistration deviceRegistration)
        {
            byte[] challenge = Crypto.GenerateChallenge();
            return StartAuthentication(appId, deviceRegistration, challenge);
        }

        public static StartedAuthentication StartAuthentication(string appId, DeviceRegistration deviceRegistration, byte[] challenge)
        {
            return new StartedAuthentication(challenge.ByteArrayToBase64String(), appId, deviceRegistration.KeyHandle.ByteArrayToBase64String());
        }

        public static uint FinishAuthentication(StartedAuthentication startedAuthentication, AuthenticateResponse response, DeviceRegistration deviceRegistration, HashSet<string> facets = null)
        {
            ClientData clientData = response.GetClientData();
            clientData.CheckContent(AuthenticateTyp, startedAuthentication.Challenge, facets);

            RawAuthenticateResponse authenticateResponse = RawAuthenticateResponse.FromBase64(response.SignatureData);
            authenticateResponse.CheckSignature(startedAuthentication.AppId, clientData.AsJson(), deviceRegistration.PublicKey);
            authenticateResponse.CheckUserPresence();

            return deviceRegistration.CheckAndUpdateCounter(authenticateResponse.Counter);
        }

        /// <summary>
        /// Generates a base 64 encode string 
        /// </summary>
        /// <returns>base 64 encode string</returns>
        public static string GenerateChallenge()
        {
            byte[] challenge = Crypto.GenerateChallenge();
            string challengeBase64 = challenge.ByteArrayToBase64String();

            return challengeBase64;
        }

    }
}
