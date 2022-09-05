namespace token_authentication.Models
{

    public static class FakeDatabase
    {
        public static List<User> users { get; set; }

        static FakeDatabase()
        {
            users = new List<User>();
            users.Add(new User { UserName = "Test1", Password = "Test1" });
            users.Add(new User { UserName = "Test2", Password = "Test2" });
        }

        public static User? Login(LoginModel user)
        {
            return users.Where(x => x.UserName == user.UserName && x.Password == user.Password).FirstOrDefault();
        }

        public static User? Login(string username)
        {
            return users.Where(x => x.UserName == username).FirstOrDefault();
        }

        public static void SaveUser(User user)
        {
            var oldUser = users.Where(x => x.UserName == user.UserName && x.Password == user.Password).SingleOrDefault();
            users.Remove(oldUser);
            users.Add(user);
        }

        public static List<User> GetUsers()
        {
            return users;
        }
    }
}
