
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using Pronet.Driver;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Pronet.Mongo
{
    public class MongoDriver
    {
        private string dbAccount = BaseConfiguration.DBAccountName;
        private string dbPassword = BaseConfiguration.DBAccountPassword;
        private string connectionUri = BaseConfiguration.ConnectionURI;
        private string databaseName = BaseConfiguration.NameOfDatabase;
        private string collectionName = BaseConfiguration.CollectionName;


        public BsonDocumentsToSend Insert { get; set; }
        private DriverContext DriverContext { get; set; }
        private IMongoCollection<BsonDocument> Collection { get; set; }

        public MongoDriver(DriverContext driverContext)
        {
            DriverContext = driverContext;
            EstablishDbConnection();
            Insert = new BsonDocumentsToSend(Collection, DriverContext);
        }

        private void EstablishDbConnection()
        {
            var client = new MongoClient(string.Format(connectionUri, dbAccount, dbPassword));
            var database = client.GetDatabase(databaseName);
            var collection = database.GetCollection<BsonDocument>(collectionName);
            Collection = collection;
            Verify.That(DriverContext, false, true, () => Assert.IsNotNull(Collection));
        }

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="filterArgs">Filter arguments.</param>
        public void DeleteDocument(Dictionary<string, object> filterArgs)
        {
            IList<FilterDefinition<BsonDocument>> filters = new List<FilterDefinition<BsonDocument>>();
            var builder = Builders<BsonDocument>.Filter;

            foreach (var kvp in filterArgs)
            {
                var filter = kvp.Value.GetType() != typeof(int) ? builder.AnyEq(kvp.Key, kvp.Value) : builder.Gt(kvp.Key, kvp.Value);
                filters.Add(filter);
            }
            var filterConcat = Builders<BsonDocument>.Filter.And(filters);
            Collection.DeleteOne(filterConcat);
        }

        /// <summary>
        /// Gets the value from document.
        /// </summary>
        /// <returns>The value from document.</returns>
        /// <param name="fieldOfValueToReturn">Field of value to return.</param>
        /// <param name="isFirstInArray">If set to <c>true</c> is first in array.</param>
        public object GetValueFromDocument(string fieldOfValueToReturn, bool isFirstInArray = true)
        {
            var filter = Builders<BsonDocument>.Filter.Exists(fieldOfValueToReturn);
            return FilterReturn(filter, fieldOfValueToReturn, isFirstInArray);
        }

        /// <summary>
        /// Gets a value from a field from the DB based on a filter argument. Any int will be evaluated as a greater than
        /// </summary>
        /// <returns>The value from document.</returns>
        /// <param name="filterArgs">Filter arguments.</param>
        /// <param name="fieldOfValueToReturn">Field of value to return.</param>
        /// <param name="isFirstInArray">If set to <c>true</c> is first in array.</param>
        public object GetValueFromDocument(Dictionary<string, object> filterArgs, string fieldOfValueToReturn, bool isFirstInArray = true)
        {
            IList<FilterDefinition<BsonDocument>> filters = new List<FilterDefinition<BsonDocument>>();
            var builder = Builders<BsonDocument>.Filter;

            foreach (var kvp in filterArgs)
            {
                var filter = kvp.Value.GetType() != typeof(int) ? builder.AnyEq(kvp.Key, kvp.Value) : builder.Gt(kvp.Key, kvp.Value);
                filters.Add(filter);
            }
            var filterConcat = Builders<BsonDocument>.Filter.And(filters);
            return FilterReturn(filterConcat, fieldOfValueToReturn, isFirstInArray);
        }

        /// <summary>
        /// Gets the count of documents.
        /// </summary>
        /// <returns>The count of documents.</returns>
        /// <param name="fieldToCount">Field to count.</param>
        /// <param name="valueOfField">Value of field.</param>
        public int GetCountOfDocuments(string fieldToCount, object valueOfField)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(fieldToCount, valueOfField);
            var documents = Collection.Find(filter).ToCursor().ToList();
            return documents.Count;
        }

        private object FilterReturn(FilterDefinition<BsonDocument> filter, string field, bool isFirstInArray)
        {
            var rand = new Random();
            var documents = Collection.Find(filter).ToCursor().ToList();
            int count = documents.Count;
            if (count > 0)
            {
                if (isFirstInArray)
                {
                    return documents[0].GetValue(field);
                }
                else
                {
                    return documents[rand.Next(0, count)].GetValue(field);
                }
            }
            else
            {
                throw new Exception("No document in the DB found based on the passed in args.");
            }
        }
    }
}
