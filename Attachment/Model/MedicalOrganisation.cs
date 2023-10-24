using LiteDB;

namespace Attachment.Model;

public class MedicalOrganisation
{
    public string Name { get; set; }
    private static int _count = 0;
    private readonly ObjectId _id;
    
    public MedicalOrganisation(string name)
    {
        Name = name;
    }
    
}