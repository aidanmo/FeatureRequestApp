using MongoDB.Driver;

namespace FeatureRequestAppLibrary.DataAccess;
public interface IDbConnection
{
    string DbName { get; }

    string CategoryCollectionName { get; }

    string StatusCollectionName { get; }

    string UserCollectionName { get; }

    string FeatureRequestCollectionName { get; }

    MongoClient Client { get; }

    IMongoCollection<CategoryModel> CategoryCollection { get; }

    IMongoCollection<StatusModel> StatusCollection { get; }

    IMongoCollection<UserModel> UserCollection { get; }

    IMongoCollection<FeatureRequestModel> FeatureRequestCollection { get; }
}