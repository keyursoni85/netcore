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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Framework.Utils
{
    public static class Utils
    {
        #region Global Messages
        // global messages
        public const string NORIGHTSMESSAGE_ACTION = "You do not have sufficient rights to do that action.";
        public const string NORIGHTSMESSAGE_ACTION2 = "You do not have access to perform that action. Please contact Administrator.";
        public const string NORIGHTSMESSAGE_PAGE = "You do not have sufficient rights to access that page.";
        public const string HTML_TAG_PATTERN = "<.*?>";
        public const string ORDERSESSIONKEY = "casemgmtordkeypaypal";
        public const string UNICODE_PATTERN = @"[^\u0000-\u007F]";
        public static IEnumerable<string> ValidDocumentImagesList = new List<string> { ".jpg", ".png", ".jpeg", ".bmp", ".doc", ".docx", ".pdf", ".xls", ".xlsx", "" };
        #endregion

        #region Global Methods

        public static string StripHTML(string inputString)
        {
            string safeString = Regex.Replace(inputString, UNICODE_PATTERN, string.Empty);
            return Regex.Replace(safeString, HTML_TAG_PATTERN, string.Empty);
        }

        public static string StripHTMLHTAGS(string inputString)
        {
            string cleanString = Regex.Replace(inputString, @"(\</?H1(.*?)/?\>)", string.Empty, RegexOptions.IgnoreCase);
            return Regex.Replace(cleanString, "<P>", "<div style=\"height: 5px;\">&nbsp;</div><P>", RegexOptions.IgnoreCase);
        }

        public static bool ContainsAny(this string stringToCheck, params string[] parameters)
        {
            return parameters.Any(parameter => stringToCheck.Contains(parameter));
        }

        public static void PopulateModel(object sourceObj, object targetObj)
        {
            var sourceProperties = TypeDescriptor.GetProperties(sourceObj).Cast<PropertyDescriptor>()
                .Where(p => p.Attributes.OfType<ColumnAttribute>().Count() > 0);

            var targetProperties = TypeDescriptor.GetProperties(targetObj).Cast<PropertyDescriptor>()
                .Where(p => p.Attributes.OfType<ColumnAttribute>().Count() > 0);


            for (int i = 0; i < sourceProperties.Count(); i++)
            {
                targetProperties.ElementAt(i).SetValue(targetObj,
                    sourceProperties.ElementAt(i).GetValue(sourceObj));
            }
        }

        public static void PopulateModelWithNoNull(object sourceObj, object targetObj)
        {
            var sourceProperties = TypeDescriptor.GetProperties(sourceObj).Cast<PropertyDescriptor>()
                .Where(p => p.Attributes.OfType<ColumnAttribute>().Count() > 0);

            var targetProperties = TypeDescriptor.GetProperties(targetObj).Cast<PropertyDescriptor>()
                .Where(p => p.Attributes.OfType<ColumnAttribute>().Count() > 0);


            for (int i = 0; i < sourceProperties.Count(); i++)
            {
                if (sourceProperties.ElementAt(i).GetValue(sourceObj) != null)
                {
                    targetProperties.ElementAt(i).SetValue(targetObj,
                        sourceProperties.ElementAt(i).GetValue(sourceObj));
                }
            }
        }

        public static string GetHijriFromGregorian(DateTime greDate)
        {
            CultureInfo arSA = CultureInfo.CreateSpecificCulture("ar-SA");
            return greDate.ToString("dd/MM/yyyy", arSA);
        }

        public static string GetHijriFromGregorian(string greDate)
        {
            //CultureInfo arSA = CultureInfo.CreateSpecificCulture("ar-SA");
            //return greDate.ToString("dd/MM/yyyy", arSA);

            CultureInfo arCul = new CultureInfo("ar-SA");
            CultureInfo enCul = new CultureInfo("en-US");
            DateTime tempDate = DateTime.ParseExact(greDate, allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
            return tempDate.ToString("dd/MM/yyyy", arCul.DateTimeFormat);
        }

        public static string GetGregorianFromHijri(DateTime hijriDate)
        {
            CultureInfo enUS = CultureInfo.CreateSpecificCulture("en-US");
            return hijriDate.ToString("dd/MM/yyyy", enUS);
        }

        private static string[] allFormats = { "yyyy/MM/dd", "yyyy/M/d", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "yyyy-MM-dd", "yyyy-M-d", "dd-MM-yyyy", "d-M-yyyy", "dd-M-yyyy", "d-MM-yyyy", "yyyy MM dd", "yyyy M d", "dd MM yyyy", "d M yyyy", "dd M yyyy", "d MM yyyy" };
        public static string GetGregorianFromHijri(string hijri)
        {
            CultureInfo arCul = new CultureInfo("ar-SA");
            CultureInfo enCul = new CultureInfo("en-US");
            DateTime tempDate = DateTime.ParseExact(hijri, allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
            return tempDate.ToString("dd/MM/yyyy", enCul.DateTimeFormat);
        }

        public static DateTime ConvertDDMMYYToDate(this string dateString)
        {
            DateTime dt;
            if (DateTime.TryParseExact(dateString,
                                        "d/M/yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                out dt))
            {
                //valid date
            }
            else
            {
                //invalid date
            }

            return dt;
        }

        public static DateTime ConvertDDMMYYHHMMToDate(this string dateString, string timeString)
        {
            DateTime dt;
            if (DateTime.TryParseExact(string.Format("{0} {1}", dateString, timeString),
                                        "d/M/yyyy H:mm",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                out dt))
            {
                //valid date
            }
            else
            {
                //invalid date
            }

            return dt;
        }

        public static string ToSafePageName(this string str)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c.ToString(), string.Empty);
            }

            // Remove Some more extra characters.
            str = str.ToLower().Trim().Replace("&", " ").Replace("/", " ").Replace("\\", " ").Replace("-", " ");

            // Replace multiple spaces to single space in string key.
            str = Regex.Replace(str, @"\s+", " ");

            // Returns string after replacing space with -
            return str.Replace(" ", "-").Replace(".", "").Replace("'", "").Replace("+", "");
        }

        public static string ToSafePageNameWithURL(this string str)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c.ToString(), string.Empty);
            }

            // Remove Some more extra characters.
            str = str.ToLower().Trim().Replace("&", " ").Replace("/", " ").Replace("\\", " ").Replace("-", " ");

            // Replace multiple spaces to single space in string key.
            str = Regex.Replace(str, @"\s+", " ");

            // Returns string after replacing space with -
            return Regex.Replace(str, "[^0-9a-zA-Z~_ -]+", string.Empty).Replace(" ", "-");
        }

        public static string Truncate(object pstrData, int intCount)
        {
            string result = string.Empty;

            try
            {
                if (!(pstrData == null))
                {
                    if (pstrData.ToString().Length > intCount)
                    {
                        //Truncate String
                        result = pstrData.ToString().Substring(0, intCount) + "... ..";
                    }
                    else
                    {
                        result = pstrData.ToString(); //Data already smaller.
                    }
                }
            }
            catch
            {
                result = pstrData.ToString();
            }


            return result;
        }

        public static void DeleteFile(string strPath)
        {
            FileInfo fi = new FileInfo(strPath);
            try
            {
                if (fi.Exists)
                    fi.Delete();
            }
            catch { }
        }

        public static string GetIconNameFromExtension_64(this string extension)
        {
            string result = "file.png";

            switch (extension.ToLower())
            {
                case ".jpg":
                case ".png":
                case ".bmp":
                case ".jpeg":
                case ".gif":
                    result = "image.png";
                    break;
                case ".doc":
                case ".docx":
                    result = "word.png";
                    break;
                case ".xls":
                case ".xlsx":
                    result = "excel.png";
                    break;
                case ".ppt":
                case ".pptx":
                case ".pps":
                case ".ppsx":
                    result = "powerpoint.png";
                    break;
                case ".pdf":
                    result = "pdf.png";
                    break;
                case ".tiff":
                case ".tif":
                    result = "tiff.png";
                    break;
            }

            return result;
        }

        public static int ToInt(string str)
        {
            int result = 0;
            if (str != null && str.Length > 0)
            {
                try
                {
                    int.TryParse(str, out result);
                }
                catch { }
            }

            return result;
        }

        public static DateTime ToDate(object data)
        {
            if (data == null)
                return new DateTime(1900, 1, 1);
            else
                return ToDate(data.ToString());
        }

        public static int ToInt(object str)
        {
            int result = 0;
            if (str != null)
            {
                try
                {
                    int.TryParse(str.ToString(), out result);
                }
                catch { }
            }

            return result;
        }

        public static bool ToBool(object data)
        {
            if (data == null)
                return false;
            else
                return ToBool(data.ToString());
        }

        public static bool ToBool(string data)
        {
            bool result = false;
            if (data != null)
            {
                string tdata = data.Trim().ToLower();
                if (tdata == "true" || tdata == "yes" || tdata == "1")
                    result = true;
                else
                    result = false;
            }
            return result;
        }

        public static string ReadFile(string filePath)
        {
            StreamReader fp;
            string fileContent = string.Empty;
            try
            {
                fp = File.OpenText(filePath);
                fileContent = fp.ReadToEnd();
                fp.Close();
            }
            catch { }
            finally { }

            return fileContent;
        }

        public static string Get8Digits()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return String.Format("{0:D8}", random);
        }

        public static void MoveImages(string sessPath, string dirPath, int fileLimitToMove = 25, bool isUniqueName = false)
        {
            try
            {
                string fileName = string.Empty;
                string newFilePath = string.Empty;
                string uniqueFileName = string.Empty;

                DirectoryInfo sessionDirInfo = new DirectoryInfo(sessPath);
                if (sessionDirInfo.Exists)
                {
                    DirectoryInfo desDirInfo = new DirectoryInfo(dirPath);
                    if (!desDirInfo.Exists)
                        desDirInfo.Create();

                    int fileLimitIndex = 0;
                    foreach (var file in sessionDirInfo.GetFiles())
                    {
                        if (fileLimitIndex >= fileLimitToMove)
                        {
                            break;
                        }
                        fileName = Path.GetFileName(file.Name);
                        if (File.Exists(Path.Combine(dirPath + "\\", fileName)))
                        {
                            File.Delete(Path.Combine(dirPath + "\\", fileName));
                        }

                        if (isUniqueName)
                            uniqueFileName = System.Guid.NewGuid().ToString() + Path.GetExtension(fileName);

                        newFilePath = Path.Combine(dirPath + "\\", (isUniqueName ? uniqueFileName : fileName));
                        System.IO.File.Move(sessPath + "\\" + fileName, newFilePath);

                        fileLimitIndex = fileLimitIndex + 1;
                    }
                    Directory.Delete(sessPath, true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string CreateTempTableSQLString(DataTable dTbl)
        {
            StringBuilder stb = new StringBuilder();
            stb.Append(" CREATE TABLE #" + dTbl.TableName + " (");
            foreach (DataColumn dcol in dTbl.Columns)
            {
                switch (dcol.DataType.Name.ToLower())
                {
                    case "string":
                        stb.Append(dcol.ColumnName + " varchar(500), ");
                        break;
                    case "decimal":
                    case "double":
                    case "float":
                        stb.Append(dcol.ColumnName + " money, ");
                        break;
                    case "int32":
                        stb.Append(dcol.ColumnName + " int, ");
                        break;
                    case "bool":
                    case "boolean":
                        stb.Append(dcol.ColumnName + " bit, ");
                        break;
                }
            }

            stb.Remove(stb.ToString().Length - 2, 2);
            stb.Append(");");

            return stb.ToString();
        }

        public static string MakeRelativeCustomUrlSafeForApp(string customUrl, string appPath)
        {
            string result = customUrl;
            string testAppPath = appPath.TrimEnd('/');

            if (appPath != "/")
            {
                if (testAppPath.Length > 0)
                {
                    result = testAppPath + customUrl;
                }
            }

            return result;
        }

        public static string BuildURL(object idOrKey, object pagePart, string appPath)
        {
            return MakeRelativeCustomUrlSafeForApp("/" + pagePart + "/" + idOrKey + ".aspx", appPath);
        }

        public static string BuildURL(object idOrKey, object subIdOrKey, object pagePart, string appPath)
        {
            return MakeRelativeCustomUrlSafeForApp("/" + pagePart + "/" + subIdOrKey + "/" + idOrKey + ".aspx", appPath);
        }

        public static string BuildURL(object idOrKey, object subIdOrKey, object subSection, object pagePart, string appPath)
        {
            return MakeRelativeCustomUrlSafeForApp("/" + pagePart + "/" + subIdOrKey + "/" + subSection + "/" + idOrKey + ".aspx", appPath);
        }

        public static string BuildURL(object idOrKey, object subIdOrKey, object subSection, object pagePart, string appPath, object searchPart = null)
        {
            return MakeRelativeCustomUrlSafeForApp("/" + pagePart + "/" + subIdOrKey + "/" + subSection + "/" + idOrKey + ".aspx" + (searchPart != null ? "?" + searchPart.ToString() : ""), appPath);
        }

        public static string ToSafeString(this object objString)
        {
            return (objString == null || objString == DBNull.Value) ? string.Empty : objString.ToString().Trim();
        }

        public static string ToLimitString(this string objString, int limit)
        {
            string resultString = objString;
            if (!string.IsNullOrEmpty(objString))
            {
                string safeString = Regex.Replace(objString, UNICODE_PATTERN, string.Empty);
                if (safeString.Length > limit)
                {
                    resultString = safeString.Substring(0, limit) + "...";
                }
            }
            return resultString;
        }

        public static decimal ToSafeDecimal(this decimal? objValue)
        {
            return (objValue == null || !objValue.HasValue) ? 0.0M : objValue.Value;
        }

        public static double ToSafeDouble(this double? objValue, int decimalPlaces = 2)
        {
            return (objValue == null || !objValue.HasValue) ? Math.Round(0.00, decimalPlaces) : Math.Round(objValue.Value, decimalPlaces);
        }

        public static int ToSafeInt(this object objString)
        {
            return (objString == null || objString == DBNull.Value) ? 0 : Utils.ToInt(objString.ToString());
        }

        public static int ToSafeInt(this int? objValue)
        {
            return (objValue == null || !objValue.HasValue) ? 0 : objValue.Value;
        }

        public static int ToSafeInt(this object objString, int defValue)
        {
            return (objString == null || objString == DBNull.Value) ? defValue : Utils.ToInt(objString.ToString());
        }

        public static bool ToSafeBool(this object objString, bool defValue)
        {
            return (objString == null || objString == DBNull.Value) ? defValue : Utils.ToBool(objString.ToString());
        }

        public static bool ToSafeBool(this bool? objString, bool defValue)
        {
            return (objString == null || !objString.HasValue) ? defValue : Utils.ToBool(objString.ToString());
        }

        public static string GetArrayPart(this string[] objArray, int arrayIndex)
        {
            return (objArray != null && objArray.Length > arrayIndex) ? objArray[arrayIndex] : string.Empty;
        }

        public static string GetPropertyValue(this object o, string propName)
        {
            try
            {
                Type t = o.GetType();
                PropertyInfo p = t.GetProperty(propName);
                object v = p.GetValue(o, null);

                return v.ToSafeString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static object GetPropertyValueAsObject(this object o, string propName)
        {
            try
            {
                Type t = o.GetType();
                PropertyInfo p = t.GetProperty(propName);
                object v = p.GetValue(o, null);

                return v;
            }
            catch
            {
                return null;
            }
        }

        public static string GetDataFromFile(string FileName)
        {
            StringBuilder stbHTML = new StringBuilder();
            if (File.Exists(FileName))
            {
                StreamReader objStreamReader = new StreamReader(FileName);
                stbHTML.Append(objStreamReader.ReadToEnd());
                objStreamReader.Dispose();
            }

            return stbHTML.ToString();
        }

        public static string EncodeString(string s)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(s);
            return Convert.ToBase64String(b, 0, b.Length);
        }

        public static string DecodeString(string s)
        {
            byte[] b = Convert.FromBase64String(s);
            return System.Text.Encoding.Default.GetString(b);
        }

        public static string ByteArrayToBase64String(this byte[] input)
        {
            string result = Convert.ToBase64String(input);
            result = result.TrimEnd('=');
            result = result.Replace('+', '-');
            result = result.Replace('/', '_');

            return result;
        }

        /// <summary>
        /// Formats string to proper base64 string and returns it as a byte array.
        /// </summary>
        /// <param name="input">The input.</param>
        public static byte[] Base64StringToByteArray(this string input)
        {
            input = input.Replace('-', '+');
            input = input.Replace('_', '/');

            int mod4 = input.Length % 4;
            if (mod4 > 0)
            {
                input += new string('=', 4 - mod4);
            }

            return Convert.FromBase64String(input);
        }

        public static byte[] GetBytes(this string stringToConvert)
        {
            return Encoding.UTF8.GetBytes(stringToConvert);
        }

        public static string GetString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string BytesToStringConverted(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public static string ToHexString(this byte[] hex)
        {
            if (hex == null) return null;
            if (hex.Length == 0) return string.Empty;

            var s = new StringBuilder();
            foreach (byte b in hex)
            {
                s.Append(b.ToString("x2"));
            }
            return s.ToString();
        }
        #endregion
    }
}
