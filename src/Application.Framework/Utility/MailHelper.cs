/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using System;
using System.Net.Mail;
using System.Text;
using Application.Framework.Utils;

namespace Application.Framework.Utility
{
    public class MailHelper
    {
        #region Send Mail

        public static string SendEmail(string sTo, string sFrom, string sCc, string sBcc, string sSubject, string sBody, string Smtphost, string Smtpport, string Issmtpsslenable, string Smtpusername, string Smtppassword)
        {
            string message = string.Empty;
            try
            {
                string i = Smtphost;
                SmtpClient SmtpMail = new SmtpClient(Smtphost);
                if (Smtpport != string.Empty)
                {
                    SmtpMail.Port = Utils.Utils.ToInt(Smtpport);
                }
                if (Issmtpsslenable == "1" || Issmtpsslenable.ToLower() == "true" || Issmtpsslenable == "yes")
                {
                    SmtpMail.EnableSsl = true;
                }
                SmtpMail.UseDefaultCredentials = false;
                SmtpMail.Credentials = new System.Net.NetworkCredential(Smtpusername, Smtppassword);

                MailMessage email = new MailMessage();

                string[] strArrEmails = sTo.ToSafeString().Split(',');
                foreach (string strEmail in strArrEmails)
                {
                    if (!string.IsNullOrEmpty(strEmail))
                        email.To.Add(GetValidEmailID(strEmail.Trim()));
                }

                email.From = new MailAddress(sFrom);

                if (!String.IsNullOrEmpty(sCc))
                {
                    strArrEmails = sCc.ToSafeString().Split(',');
                    foreach (string strEmail in strArrEmails)
                    {
                        if (!string.IsNullOrEmpty(strEmail))
                            email.CC.Add(GetValidEmailID(strEmail.Trim()));
                    }
                }

                if (!String.IsNullOrEmpty(sBcc))
                    email.Bcc.Add(sBcc);

                if (!String.IsNullOrEmpty(sSubject))
                    email.Subject = sSubject;

                email.Body = sBody;
                email.IsBodyHtml = true;

                SmtpMail.Send(email);
                message = "success";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }

        public static string GetValidEmailID(string strEmailID)
        {
            return strEmailID.Replace(";", ",").Replace(" ", "");
        }


        public static string SendUserLockoutEmail(string identifier, string path, DateTime date, string sUrl, string sName, string sEmail, string sLogo, string sTo, string sFrom, string sCc, string sBcc, string sSubject, string Smtphost, string Smtpport, string Issmtpsslenable, string Smtpusername, string Smtppassword, int maxLoginAttempts)
        {
            string message = string.Empty;
            try
            {
                StringBuilder sbMailBody = new StringBuilder();
                sbMailBody.Append(Utils.Utils.GetDataFromFile(path));
                sbMailBody.Replace("[USERNAME]", identifier.ToSafeString());
                sbMailBody.Replace("[DATETIME]", date.ToString("F"));
                sbMailBody.Replace("[CURRENTYEAR]", date.Year.ToString());
                sbMailBody.Replace("[SITEURL]", sUrl);
                sbMailBody.Replace("[SITENAME]", sName);
                sbMailBody.Replace("[SUPPORTEMAIL]", sEmail);
                sbMailBody.Replace("[LOGO]", sEmail);
                sbMailBody.Replace("[Times]", maxLoginAttempts.ToSafeString());

                string i = Smtphost;
                SmtpClient SmtpMail = new SmtpClient(Smtphost);
                if (Smtpport != string.Empty)
                {
                    SmtpMail.Port = Utils.Utils.ToInt(Smtpport);
                }
                if (Issmtpsslenable == "1" || Issmtpsslenable.ToLower() == "true" || Issmtpsslenable == "yes")
                {
                    SmtpMail.EnableSsl = true;
                }
                SmtpMail.UseDefaultCredentials = false;
                SmtpMail.Credentials = new System.Net.NetworkCredential(Smtpusername, Smtppassword);

                MailMessage email = new MailMessage();

                string[] strArrEmails = sTo.ToSafeString().Split(',');
                foreach (string strEmail in strArrEmails)
                {
                    if (!string.IsNullOrEmpty(strEmail))
                        email.To.Add(GetValidEmailID(strEmail.Trim()));
                }

                email.From = new MailAddress(sFrom);

                if (!String.IsNullOrEmpty(sCc))
                {
                    strArrEmails = sCc.ToSafeString().Split(',');
                    foreach (string strEmail in strArrEmails)
                    {
                        if (!string.IsNullOrEmpty(strEmail))
                            email.CC.Add(GetValidEmailID(strEmail.Trim()));
                    }
                }

                if (!String.IsNullOrEmpty(sBcc))
                    email.Bcc.Add(sBcc);

                if (!String.IsNullOrEmpty(sSubject))
                    email.Subject = sSubject;

                email.Body = sbMailBody.ToSafeString();
                email.IsBodyHtml = true;

                SmtpMail.Send(email);
                message = "success";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }

        #endregion
    }
}
