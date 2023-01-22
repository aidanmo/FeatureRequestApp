using System;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace FeatureRequestAppLibrary.DataAccess
{
    public class DbConnection : IDbConnection
    {
        private readonly IConfiguration _config;
        private readonly IMongoDatabase _db;
        private string _connectionID = "MongoDB";
        //Private set makes this string read only while allowing us to still use get set
        public string DbName { get; private set; }

        //Strings used to store the name of each collection in our DB

        public string CategoryCollectionName { get; private set; } = "categories";

        public string StatusCollectionName { get; private set; } = "statuses";

        public string UserCollectionName { get; private set; } = "users";

        public string FeatureRequestCollectionName { get; private set; } = "featurerequest";

        public MongoClient Client { get; private set; }

        //Used as collections to individual tables each

        public IMongoCollection<CategoryModel> CategoryCollection { get; private set; }

        public IMongoCollection<StatusModel> StatusCollection { get; private set; }

        public IMongoCollection<UserModel> UserCollection { get; private set; }

        public IMongoCollection<FeatureRequestModel> FeatureRequestCollection { get; private set; }


        public DbConnection(IConfiguration config)
        {
            //Create connection to DB
            _config = config;
            Client = new MongoClient(_config.GetConnectionString(_connectionID));
            DbName = _config["DatabaseName"];
            _db = Client.GetDatabase(DbName);

            //Creating connection to all four collections
            CategoryCollection = _db.GetCollection<CategoryModel>(CategoryCollectionName);
            StatusCollection = _db.GetCollection<StatusModel>(StatusCollectionName);
            UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
            FeatureRequestCollection = _db.GetCollection<FeatureRequestModel>(FeatureRequestCollectionName);
        }
    }
}

