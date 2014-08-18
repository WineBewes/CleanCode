using System;

namespace CleanCode.Business
{
    public interface IUserNameGetter
    {
        string GetUserName();
    }

    public class UserNameGetter : IUserNameGetter
    {
        public string GetUserName()
        {
            throw new Exception("Username not found !");
        }
    }
}
