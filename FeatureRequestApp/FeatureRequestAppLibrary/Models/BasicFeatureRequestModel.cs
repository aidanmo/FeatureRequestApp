using System;
namespace FeatureRequestAppLibrary.Models
{
	public class BasicFeatureRequestModel
	{
        
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }

		public string Feature { get; set; }


		public BasicFeatureRequestModel()
		{

		}

		public BasicFeatureRequestModel(FeatureRequestModel feature)
		{
			Id = feature.Id;
			Feature = feature.FeatureRequest;
		}
	}


}

