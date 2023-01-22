using System;
namespace FeatureRequestAppLibrary.DataAccess
{
    public class MongoUserData : IUserData
    {
        private readonly IMongoCollection<UserModel> _users;

        public MongoUserData(IDbConnection db)
        {
            _users = db.UserCollection;
        }

        public async Task<List<UserModel>> GetUsersAsync()
        {
            //Returns all users with in a collection
            var results = await _users.FindAsync(_ => true);

            //returns results in a list format
            return results.ToList();
        }

        public async Task<UserModel> GetUser(string id)
        {
            //Match ID to ID passed in as an argument
            var results = await _users.FindAsync(u => u.Id == id);

            //returns results in a list format
            return results.FirstOrDefault();
        }

        public async Task<UserModel> GetUserFromAuthentication(string objectId)
        {
            //Returns all users with in a collection
            var results = await _users.FindAsync(u => u.ObjectIdentifier == objectId);

            //returns results in a list format
            return results.FirstOrDefault();
        }

        public Task CreateUser(UserModel user)
        {
            return _users.InsertOneAsync(user);
        }

        public Task UpdateUser(UserModel user)
        {
            var filter = Builders<UserModel>.Filter.Eq("Id", user.Id);
            return _users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
        }
    }
}

