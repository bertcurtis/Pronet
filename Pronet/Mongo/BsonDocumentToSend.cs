using MongoDB.Bson;
using MongoDB.Driver;
using Pronet.Driver;
using NUnit.Framework;

namespace Pronet.Mongo
{
    public class BsonDocumentsToSend
    {
        private IMongoCollection<BsonDocument> Collection { get; set; }
        private DriverContext DriverContext { get; set; }

        public BsonDocumentsToSend(IMongoCollection<BsonDocument> collection, DriverContext driver)
        {
            DriverContext = driver;
            Collection = collection;
        }
        public void NewDocument(string document)            
        {
            var preCount = Collection.Count(new BsonDocument());
            Collection.InsertOne(document.ToBsonDocument());
            var postCount = Collection.Count(new BsonDocument());
            Verify.That(DriverContext, false, true, () => Assert.GreaterOrEqual(postCount, preCount));
        }
        public void NewDocument(BsonDocument mongoField)
        {
            var preCount = Collection.Count(new BsonDocument());
            Collection.InsertOne(mongoField);
            var postCount = Collection.Count(new BsonDocument());
            Verify.That(DriverContext, false, true, () => Assert.GreaterOrEqual(postCount, preCount));
        }
        public void UpdateDocument(string whereThisField, object isThisValue, string fieldToUpdate, object valueBeingUpdated, MongoUpdateInfo info = MongoUpdateInfo.SettingFieldValue)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(whereThisField.ToString(), isThisValue);

            var update = Builders<BsonDocument>.Update;
            if (info == MongoUpdateInfo.PushingToArray)
            {
                Collection.FindOneAndUpdate(filter, update.Push(fieldToUpdate, valueBeingUpdated));
            }
            else if (info == MongoUpdateInfo.RemovingFromArray)
            {
                Collection.FindOneAndUpdate(filter, update.Pull(fieldToUpdate, valueBeingUpdated));
            }
            else if (info == MongoUpdateInfo.SettingFieldValue)
            {
                Collection.FindOneAndUpdate(filter, update.Set(fieldToUpdate, valueBeingUpdated));
            }
        }       
    }
}
