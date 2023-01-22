 using System;
namespace FeatureRequestAppLibrary.Models
{
	public class FeatureRequestModel
	{
        //identifier
        [BsonId]

        //object Id unique identifier used when inserting an object into our db
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }

        public string FeatureRequest { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public CategoryModel Category { get; set; }

        public BasicUserModel Author { get; set; }

        //Using hashset to prevent duplicate votes from a user, becuase hash
        //sets are a list of unique  items.
        public HashSet<string> UserVotes { get; set; }

        public StatusModel FeatureStatus { get; set; }

        public string OwnerNotes { get; set; }

        public bool ApprovedForRelease { get; set; }

        public bool Archived { get; set; } = false;

        public bool Rejected { get; set; } = false;

    }
}

