using System;
namespace FeatureRequestAppLibrary.Models
{
	public class UserModel
	{
        //identifier
        [BsonId]

        //object Id unique identifier used when inserting an object into our db
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }

        public string ObjectIdentifier { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string EmailAddress { get; set; }

        public List<BasicFeatureRequestModel> AuthoredFeatures{ get; set; } = new();

        public List<BasicFeatureRequestModel> VotedOnFeatures { get; set; } = new();
    }
}

