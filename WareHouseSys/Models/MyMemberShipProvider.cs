using AifuSecurity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;
using WareHouseSys.DBModels;

namespace WareHouseSys.Models
{
    public class MyMemberShipProvider : MembershipProvider
    {
        public override bool EnablePasswordRetrieval => throw new NotImplementedException();

        public override bool EnablePasswordReset => throw new NotImplementedException();

        public override bool RequiresQuestionAndAnswer => throw new NotImplementedException();

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int MaxInvalidPasswordAttempts => throw new NotImplementedException();

        public override int PasswordAttemptWindow => throw new NotImplementedException();

        public override bool RequiresUniqueEmail => throw new NotImplementedException();

        public override MembershipPasswordFormat PasswordFormat => throw new NotImplementedException();

        public override int MinRequiredPasswordLength => throw new NotImplementedException();

        public override int MinRequiredNonAlphanumericCharacters => throw new NotImplementedException();

        public override string PasswordStrengthRegularExpression => throw new NotImplementedException();

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            status = MembershipCreateStatus.Success;
            return new MembershipUser("MyMemberShipProvider", username, username,
                                null, null, null, true, false, DateTime.Now, DateTime.Now, DateTime.Now,
                                DateTime.Now, DateTime.Now);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            string adminPwd = WebConfigurationManager.AppSettings["adminPwd"];


            //if (ADValidate(username, password)) return true;

            string pwd = password.EnCode();
            if (adminPwd == GenSHA256(password.Trim())) return true;

            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];
            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);
            List<Employee> employees = db.Queryable<Employee>().Where(e => e.KEYNO == username).ToList();
            if (employees.Count == 0) return false;

            //return true;
            return pwd == employees[0].USERPWD;
        }

        private bool ADValidate(string UserId,string Password)
        {
            bool retValue = true;
            DirectoryEntry adsEntry = new DirectoryEntry();
            adsEntry.Path = "LDAP://192.168.3.236:389/CN=Configuration,DC=tmrt,DC=com,DC=tw";

            adsEntry.Username = @"tmrt.com.tw\110140";
            adsEntry.Password = "1qaz!QAZ";

            using (DirectorySearcher adsSearcher = new DirectorySearcher(adsEntry))
            {
                //adsSearcher.Filter = "(&(objectClass=user)(objectCategory=person)(sAMAccountName=110140))";
                adsSearcher.Filter = "(&(sAMAccountName=110140))";

                try
                {
                    SearchResult adsSearchResult = adsSearcher.FindOne();
                    
                }
                catch (Exception ex)
                {
                    retValue = false;
                }
                finally
                {
                    adsEntry.Close();
                }

                return retValue;

            }
        }

        /// <summary>
        /// 密碼進行SHA1加密
        /// </summary>
        /// <param name="encodingData"></param>
        /// <returns></returns>
        public static string GenSHA256(string encodingData)
        {
            return Convert.ToBase64String(new SHA256CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(encodingData)));
        }
    }
}