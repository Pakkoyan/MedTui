using System.Collections.Immutable;
using Attachment.Model;

namespace Controller.Interfaces;

public interface IPatientRepository
{
    void AddPatient(Patient patient);
    void RemovePatient(string IIN);
    Patient FindPatientByIIN(string IIN);
    
    // Недочет, потому что в реальном проекте будет много людей с одинаковыми именем и фамилией 
    Patient FindPatientByName(string Surnameб, string Name); 
    ImmutableList<Patient> GetAllPatients();
    
}