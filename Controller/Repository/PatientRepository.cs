using System.Collections.Immutable;
using Attachment.Model;
using Controller.Interfaces;
using LiteDB;

namespace Controller.Repository;

public class PatientRepository : IPatientRepository
{
    private readonly string _connectionString;

    public PatientRepository(string connectionString)
    {
        _connectionString = connectionString;
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<Patient>("patients");
            collection.EnsureIndex(x => x.IIN, unique: true);
            collection.EnsureIndex(x => x.Surname);
            collection.EnsureIndex(x => x.Name);
        }
    }

    public void AddPatient(Patient patient)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<Patient>("patients");
            collection.Insert(patient);
        }
    }

    public void RemovePatient(string IIN)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<Patient>("patients");
            collection.DeleteMany(patient => patient.IIN == IIN);
        }
    }

    public Patient FindPatientByIIN(string IIN)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<Patient>("patients");
            return collection.FindOne(patient => patient.IIN == IIN);
        }
    }

    public Patient FindPatientByName(string surname, string name)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<Patient>("patients");
            return collection.FindOne(patient => patient.Name == name && patient.Surname == surname);
        }
    }

    public ImmutableList<Patient> GetAllPatients()
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<Patient>("patients");
            return ImmutableList.CreateRange(collection.FindAll());
        }
    }
}