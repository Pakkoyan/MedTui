namespace Attachment.Model;

public class Patient
{
    private string _IIN;
    public string Name { get; set; }
    public string Surname { get; set; }
    public string IIN
    {
        get
        {
            return _IIN;
        }

        set
        {
            if (value.Length == 12)
            {
                _IIN = value;
            }
            else
            {
                throw new InvalidIIN("Неправильная длина введеного вам ИИН");
            }
        }

    }

    public Patient(string name, string surname, string iin)
    {
        Name = name;
        Surname = surname;
        
        if (iin.Length == 12)
        {
            _IIN = iin;
        }
        else
        {
            throw new InvalidIIN("Неправильная длина введеного вам ИИН");
        }
    }
}

public class InvalidIIN  : Exception
{
    public InvalidIIN() : base("ИИН введен не верно") { }

    public InvalidIIN(string message) : base(message) { }

    public InvalidIIN(string message, Exception inner) : base(message, inner) { }
}
