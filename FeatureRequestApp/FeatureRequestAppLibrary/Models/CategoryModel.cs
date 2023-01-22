using System;
namespace FeatureRequestAppLibrary.Models
{
	public class CategoryModel
	{
        //identifier
        [BsonId]

        //object Id unique identifier used when inserting an object into our db
        [BsonRepresentation(BsonType.ObjectId)]

		public string Id { get; set; }
        
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }
    }
}

