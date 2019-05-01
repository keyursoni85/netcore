/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/

namespace Application.Framework
{
    public class CustomPasswordValidator
    {
        public static int CheckPasswordConfiguration(string password, int minLenght, int maxLenght, bool isAllowSpecialChar, bool isAllowUpperCase)
        {
            int IsValid = -1;
            if (password.Length >= minLenght && password.Length <= maxLenght)
            {
                if (isAllowSpecialChar == true && isAllowUpperCase == true)
                {
                    string passwordRegex = @"((?=.*\d)(?=.*[A-Z])(?=.*\W).{1,})";
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(passwordRegex);
                    if (regex.IsMatch(password))
                    {
                        IsValid = 1;
                    }
                    else
                    {
                        IsValid = -2;
                    }
                }
                else if (isAllowSpecialChar == true && isAllowUpperCase == false)
                {
                    string passwordRegex = @"((?=.*\d)(?=.*[a-z])(?=.*\W).{1,})";
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(passwordRegex);
                    if (regex.IsMatch(password))
                    {
                        IsValid = 1;
                    }
                    else
                    {
                        IsValid = -3;
                    }
                }
                else if (isAllowSpecialChar == false && isAllowUpperCase == true)
                {
                    string passwordRegex = @"((?=.*\d)(?=.*[A-Z])(?=.*).{1,})";
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(passwordRegex);
                    if (regex.IsMatch(password))
                    {
                        IsValid = 1;
                    }
                    else
                    {
                        IsValid = -4;
                    }
                }
                else
                {
                    string passwordRegex = @"[a-zA-Z0-9]*";
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(passwordRegex);
                    if (regex.IsMatch(password))
                    {
                        IsValid = 1;
                    }
                    else
                    {
                        IsValid = -5;
                    }
                }
            }
            return IsValid;
        }
    }
}
