using System.Collections.Immutable;
using Attachment.Model;
using LiteDB;

namespace Controller.Interfaces;

public interface IAttachmentRequestRepository
{
    void AddRequest(AttachmentRequest request);
    void RemoveRequest(ObjectId requestId);
    AttachmentRequest FindRequest(ObjectId requestId);
    ImmutableList<AttachmentRequest> GetAllRequest();
}