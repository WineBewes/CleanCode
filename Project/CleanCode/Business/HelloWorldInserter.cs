namespace CleanCode.Business
{
    public class HelloWorldInserter
    {
        private readonly IUserNameGetter userNameGetter;

        public HelloWorldInserter(IUserNameGetter userNameGetter)
        {
            this.userNameGetter = userNameGetter;
        }

        public string InsertHelloWorld()
        {
            var userName = userNameGetter.GetUserName();

            return string.Format("Hello World from {0} !", userName);
        }
    }
}
