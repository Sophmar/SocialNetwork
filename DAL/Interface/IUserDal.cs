using DTO;

namespace DAL.Interface
{
    public interface IUserDal
    {
        public string HashPassword(string passwd);
        public bool Add(Users user);
        public bool Delete(string email);
        public bool Update(Users user);
        public List<Users> GetAll();
        public Users GetByEmail(string email);
        public bool CheckPasswd(string email, string passwd);
    }
}
