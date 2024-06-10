using System;
using System.Collections.Generic;
using System.Text;

namespace DNH.Storage.MongoDB.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class EntityCollectionGroup : System.Attribute
    {
        public string CollectionName;

        public EntityCollectionGroup(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
