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
using Application.Framework.Utils;
using Newtonsoft.Json.Linq;

namespace Application.Framework
{
    public class ClientData
    {
        private const string TypeParam = "typ";
        private const string ChallengeParam = "challenge";
        private const string OriginParam = "origin";
        public string Type { get; private set; }
        public string Challenge { get; private set; }
        public string Origin { get; private set; }
        public string RawClientData { get; private set; }

        public ClientData(string clientData)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(clientData))
                {
                    RawClientData = clientData.Base64StringToByteArray().GetString();
                    JObject element = JObject.Parse(RawClientData);
                    if (element != null)
                    {
                        JToken theType, theChallenge, theOrgin;
                        if (element.TryGetValue(TypeParam, out theType))
                        {
                            Type = theType.ToString();
                        }

                        if (element.TryGetValue(ChallengeParam, out theChallenge))
                        {
                            Challenge = theChallenge.ToString();
                        }

                        if (element.TryGetValue(OriginParam, out theOrgin))
                        {
                            Origin = theOrgin.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CheckContent(string type, string challenge, HashSet<string> facets)
        {
            if (facets != null)
            {
                VerifyOrigin(Origin, CanonicalizeOrigins(facets));
            }
        }
        public string AsJson()
        {
            return RawClientData;
        }
        private void VerifyOrigin(string origin, HashSet<string> allowedOrigins)
        {
            if (!allowedOrigins.Contains(CanonicalizeOrigin(origin)))
            {
                throw new UriFormatException(origin + " is not a recognized home origin for this backend");
            }
        }
        private HashSet<string> CanonicalizeOrigins(HashSet<string> origins)
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string orgin in origins)
            {
                result.Add(CanonicalizeOrigin(orgin));
            }
            return result;
        }
        private string CanonicalizeOrigin(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Invalid argument(s) were being passed.");
            try
            {
                Uri uri = new Uri(url);
                if (string.IsNullOrWhiteSpace(uri.Authority))
                    return url;

                return uri.Scheme + "://" + uri.Authority;
            }
            catch (UriFormatException e)
            {
                throw new UriFormatException("specified bad origin", e);
            }
        }
    }
}
