using DAL.Concrete;
using DTO;
using MongoDB.Bson;
using System.Threading.Tasks.Dataflow;

namespace Services
{

    public class UserService
    {
        UserDal dal1 = new UserDal();
        UserDalNeo dal2 = new UserDalNeo();
        public bool Add(Users user)
        {
            var res1 = dal1.Add(user);

            byte[] bytes = user.Id.ToByteArray();
            int idNeo = BitConverter.ToInt32(bytes, 0);

            var res2 = dal2.CreateUserAsync(idNeo);

            if (res1 && (res2 != null))
                return true;
            return false;
        }
        public bool Delete(Users user)
        {
            var res1 = dal1.Delete(user.Email);

            byte[] bytes = user.Id.ToByteArray();
            int idNeo = BitConverter.ToInt32(bytes, 0);

            var res2 = dal2.DeleteUserAsync(idNeo);

            if (res1)
                return true;
            return false;
        }
    }
}
