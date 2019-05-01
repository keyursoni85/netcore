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
    public class StartedRegistration : BaseModel
    {
        public StartedRegistration(string challenge, string appId)
        {
            if (string.IsNullOrWhiteSpace(challenge) || string.IsNullOrWhiteSpace(appId))
                throw new ArgumentException("Invalid argument(s) were being passed.");

            Version = U2F.U2FVersion;
            Challenge = challenge;
            AppId = appId;
        }

        public string Version { get; private set; }
        public string Challenge { get; private set; }
        public string AppId { get; private set; }

        public override int GetHashCode()
        {
            int hash = Version.Sum(c => c + 31);
            hash += Challenge.Sum(c => c + 31);
            hash += AppId.Sum(c => c + 31);

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StartedRegistration))
                return false;
            if (this == obj)
                return true;
            if (GetType() != obj.GetType())
                return false;
            StartedRegistration other = (StartedRegistration)obj;
            if (AppId == null)
            {
                if (other.AppId != null)
                    return false;
            }
            else if (!AppId.Equals(other.AppId))
                return false;
            if (Challenge == null)
            {
                if (other.Challenge != null)
                    return false;
            }
            else if (!Challenge.Equals(other.Challenge))
                return false;
            if (Version == null)
            {
                if (other.Version != null)
                    return false;
            }
            else if (!Version.Equals(other.Version))
                return false;
            return true;
        }
    }
}
