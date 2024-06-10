using DNH.Storage.MongoDB.Attributes;
using DNH.Storage.MongoDB.Models;
using MongoDB.Bson.Serialization.Attributes;
using ProjectModule.Repository.Collections.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectModule.Repository.Collections
{
    [EntityCollectionGroup("DNHDemo")]
    public class ProjectDocument : MongoBaseEntity
    {
        [BsonElement("title")] // Tên của field tương ứng trong collection
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("proprosal")]
        public ProjectProposalElement Proprosal { get; set; }

        // Các trường khác và các attribute tương ứng
    }
}
