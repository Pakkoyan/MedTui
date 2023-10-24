using Attachment.Model;
using LiteDB;

namespace Attachment.Model;

public class AttachmentRequest
{
    public ObjectId Id { get; set; }
    public enum AttachmentStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }
    public readonly DateTime RegistrationTime;
    public readonly DateTime? ProcessingTime;
    public Patient Patient { get; set; }
    public AttachmentStatus Status { get; set; }
    public MedicalOrganisation Organisation { get; set; }
    
    public AttachmentRequest(Patient patient, AttachmentStatus status, MedicalOrganisation organisation)
    {
        RegistrationTime = DateTime.UtcNow;
        Patient = patient;
        Status = status;
        Organisation = organisation;
    }

    public AttachmentRequest()
    {
          
    }
}