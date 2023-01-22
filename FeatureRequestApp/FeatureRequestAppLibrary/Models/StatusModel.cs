using System;
namespace FeatureRequestAppLibrary.Models
{
	public class StatusModel
	{
        //identifier
        [BsonId]

        //object Id unique identifier used when inserting an object into our db
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }

        public string StatusName { get; set; }

        public string StatusDescription { get; set; }  
    }
}

