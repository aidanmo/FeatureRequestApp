using System;
namespace FeatureRequestAppLibrary.DataAccess
{
    public class MongoFeatureRequestData : IFeatureRequestData
    {
        private readonly IDbConnection _db;
        private readonly IUserData _userData;
        private readonly IMemoryCache _cache;
        private readonly IMongoCollection<FeatureRequestModel> _featureRequests;
        private const string CacheName = "StatusData";

        public MongoFeatureRequestData(IDbConnection db, IUserData userData, IMemoryCache cache)
        {
            _db = db;
            _userData = userData;
            _cache = cache;
            _featureRequests = db.FeatureRequestCollection;
        }

        public async Task<List<FeatureRequestModel>> GetAllFeatureRequests()
        {
            var output = _cache.Get<List<FeatureRequestModel>>(CacheName);
            if (output is null)
            {
                var results = await _featureRequests.FindAsync(s => s.Archived == false);
                output = results.ToList();

                _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
            }
            return output;
        }

        public async Task<List<FeatureRequestModel>> GetAllApprovedFeatureRequests()
        {
            var output = await GetAllFeatureRequests();
            return output.Where(x => x.ApprovedForRelease).ToList();
        }

        public async Task<FeatureRequestModel> GetFeatureRequest(string id)
        {
            var results = await _featureRequests.FindAsync(s => s.Id == id);
            return results.FirstOrDefault();
        }

        public async Task<List<FeatureRequestModel>> GetAllFeatureRequestsForApproval()
        {
            var output = await GetAllFeatureRequests();
            //Checking that the feature hasn't already been rejected and hasn't been approved either.
            return output.Where(x => x.ApprovedForRelease == false
            && x.Rejected == false).ToList();
        }

        public async Task UpdateFeatureRequest(FeatureRequestModel feature)
        {
            await _featureRequests.ReplaceOneAsync(s => s.Id == feature.Id, feature);
            _cache.Remove(CacheName);
        }

        public async Task UpvoteFeature(string featureId, string userId)
        {
            var client = _db.Client;

            using var session = await client.StartSessionAsync();

            session.StartTransaction();

            //TO-DO1: go over caching structure so you fully understand logic at work.
            try
            {
                var db = client.GetDatabase(_db.DbName);
                var featuresInTransaction = db.GetCollection<FeatureRequestModel>(_db.FeatureRequestCollectionName);
                var feature = (await featuresInTransaction.FindAsync(s => s.Id == featureId)).First();

                bool isUpvote = feature.UserVotes.Add(userId);
                if (isUpvote == false)
                {
                    feature.UserVotes.Remove(userId);
                }

                await featuresInTransaction.ReplaceOneAsync(s => s.Id == featureId, feature);

                var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
                var user = await _userData.GetUser(feature.Author.Id);

                if (isUpvote)
                {
                    user.VotedOnFeatures.Add(new BasicFeatureRequestModel(feature));
                }
                else
                {
                    var featureToRemove = user.VotedOnFeatures.Where(s => s.Id == featureId).First();
                    user.VotedOnFeatures.Remove(new BasicFeatureRequestModel(feature));
                }
                await usersInTransaction.ReplaceOneAsync(u => u.Id == userId, user);

                await session.CommitTransactionAsync();

                _cache.Remove(CacheName);
            }
            catch (Exception ex)
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }

        public async Task CreateFeatureRequest(FeatureRequestModel feature)
        {
            var client = _db.Client;

            using var session = await client.StartSessionAsync();

            session.StartTransaction();

            try
            {
                var db = client.GetDatabase(_db.DbName);
                var featuresInTransaction = db.GetCollection<FeatureRequestModel>(_db.FeatureRequestCollectionName);
                await featuresInTransaction.InsertOneAsync(feature);

                var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
                var user = await _userData.GetUser(feature.Author.Id);
                user.AuthoredFeatures.Add(new BasicFeatureRequestModel(feature));

                await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);

                await session.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }




    }
}

