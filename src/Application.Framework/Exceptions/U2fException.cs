/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using System;

namespace Application.Framework.Exceptions
{
    public class U2fException : Exception
    {
        public U2fException(string message)
        {
            Console.WriteLine("U2f exception:{0}", message);
        }

        public U2fException(string message, InvalidKeySpecException invalidKeyException): base(message, invalidKeyException?.InnerException)
        {
            Console.WriteLine("Error verifying signature:{0} invalid key exception:{1}", message, invalidKeyException);
        }

        public U2fException(string message, Exception invalidKeyException): base(message, invalidKeyException?.InnerException)
        {
            Console.WriteLine("Could not parse:{0} invalid key exception:{1}", message, invalidKeyException);
        }
    }
}
