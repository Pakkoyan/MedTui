using System.Collections.Immutable;
using Attachment.Model;
using Controller.Interfaces;
using LiteDB;

namespace Controller.Repository;

public class RequestRepository : IAttachmentRequestRepository
{
    private readonly string _connectionString;

    public RequestRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public void AddRequest(AttachmentRequest request)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<AttachmentRequest>("requests");
            collection.Insert(request);
        }
    }

    public void RemoveRequest(ObjectId requestId)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<AttachmentRequest>("requests");
            collection.Delete(requestId);
        }
    }

    public AttachmentRequest FindRequest(ObjectId requestId)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<AttachmentRequest>("requests");
            return collection.FindById(requestId);
        }
    }
    
    public void FindRequestAndReWrite(ObjectId requestId, AttachmentRequest.AttachmentStatus newStatus)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<AttachmentRequest>("requests");
            var request = collection.FindById(requestId);
            collection.Delete(requestId);
            request.Status = newStatus;
            collection.Insert(request);
        }
    }

    public ImmutableList<AttachmentRequest> GetAllRequest()
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<AttachmentRequest>("requests");
            return ImmutableList.CreateRange(collection.FindAll());
        }
    }
}