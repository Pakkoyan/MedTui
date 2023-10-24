using Attachment.Model;
using Controller.Repository;
using LiteDB;

namespace FrontEnd.Model;


// ФронтЕнд для нашего сайта по этому будет отличатся, в нашем случае в консольном приложение
// Можно и нужно использовать статический класс т.к. поток всего один, и мы либо работаем 
// Прямо сейчас с обьектом, либо мы завершили программу 

public static class MedSite
{
    private static User _user;
    private static string _limiter = new string('-', 20);
    private static UserRepository _userRepo = new UserRepository("userbd");
    private static PatientRepository _patientRepo = new PatientRepository("patientbd");
    private static RequestRepository _requestRepo = new RequestRepository("requestdb");
    
    private static string WriteMenuItem(int move, int position, string text)
    {
        string symbol = move == position ? "->" : "";
        return $"{symbol} {text} ";
    }
    
    private static void MoveBottomMenuSelection(ref int selection, int menuItemsCount)
    {
        if (selection >= menuItemsCount)
        {
            selection = 1;
        }
        else
        {
            selection++;
        }

    }

    private static void MoveTopMenuSelection(ref int selection, int menuItemsCount)
    {
        if (selection <= 1)
        {
            selection = menuItemsCount;
        }
        else
        {
            selection--;
        }
    }
    public static void Start()
    {
        if (WelcomMenu() == false) throw new Exception("Пока Пока!!!");

        while (true)
        {
            if (_user._privilege == "admin")
            {
                if (AdminMenu() == false) if (WelcomMenu() == false) throw new Exception("Пока Пока!!!"); //Костыли, но это что бы быстрее закончить блин!!! я бы мог написать долгую рекурсию но это так впадлу
            }
            
            if (_user._privilege == "user")
            {
                if (UserMenu() == false) if (WelcomMenu() == false) throw new Exception("Пока Пока!!!"); //Костыли, но это что бы быстрее закончить блин!!! я бы мог написать долгую рекурсию но это так впадлу
            }
            
        }       
        
    }

    private static User Registration()
    {
        Console.WriteLine($"{_limiter} Введите логин {_limiter}");
        string login = Console.ReadLine();
        Console.Clear();
        
        Console.WriteLine($"{_limiter}Введите пароль {_limiter}");
        string password = Console.ReadLine();
        Console.Clear();
        
        Console.WriteLine("А теперь загадка: ");
        Console.WriteLine("Весит груша, нельзя скушать ? ");
        string passPhrase = Console.ReadLine();
        Console.Clear();
        
        return new User(login, password, passPhrase);
    }

    private static KeyValuePair<string, string> Login()
    {
        Console.WriteLine($"{_limiter} Введите логин {_limiter}");
        string login = Console.ReadLine();
        Console.Clear();
        
        Console.WriteLine($"{_limiter}Введите пароль {_limiter}");
        string password = Console.ReadLine();
        Console.Clear();

        return new KeyValuePair<string, string>(login, password);
    }
    
    private static bool WelcomMenu()
    {
        Console.WriteLine($"{_limiter} Управление происходит за счет стрелочек вниз/ввверх {_limiter}");
        Console.WriteLine("1. Нажмите стрелочку ");
        Console.WriteLine("2. Дойдите до пункта ");
        Console.WriteLine("3. Нажмите Enter ");
        //Thread.Sleep(3000);
        Console.Clear();

        
        ConsoleKeyInfo key = new ConsoleKeyInfo();
        int move = 1;
        while (key.Key != ConsoleKey.Enter)
        {
            Console.WriteLine($"{_limiter} Вас приветсвуте сайт с медицинской пропиской {_limiter}");
            Console.WriteLine(WriteMenuItem(move, 1,"Регистрация "));
            Console.WriteLine(WriteMenuItem(move, 2,"Вход "));
            Console.WriteLine(WriteMenuItem(move, 3,"Выход "));
            Console.WriteLine($"{_limiter}{_limiter}");
            
            key = Console.ReadKey();
            if (key.Key == ConsoleKey.DownArrow)
            {
                MoveBottomMenuSelection(ref move, menuItemsCount: 3);
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                MoveTopMenuSelection(ref move, menuItemsCount: 3);
            }

            Console.Clear();
        }

        switch (move)
        {
            case 1:
                _user = _userRepo.AddUser(Registration()); //Хз как это реализовать, но я нарушаю SOLID делая так что внутри
                break;
            case 2:
                var kostil = Login();
                var tmpUser = _userRepo.FindUserByLogin(kostil.Key); // очень не безопасно но фискить впадлу
                if (tmpUser.Password == kostil.Value)
                {
                    _user = tmpUser;
                } 
                break;
            case 3:
                Console.WriteLine("Досвидание!!! ");
                return false;
        }
        
        Console.WriteLine("Вход прошел успешно! ");
        return true;
    }

    private static bool AdminMenu()
    {
        ConsoleKeyInfo key = new ConsoleKeyInfo();
        int move = 1;
        while (key.Key != ConsoleKey.Enter)
        {
            Console.WriteLine($"{_limiter} Admin menu {_limiter}");
            Console.WriteLine(WriteMenuItem(move, 1,"Поиск по ФИО "));
            Console.WriteLine(WriteMenuItem(move, 2,"Поиск по ИИН "));
            Console.WriteLine(WriteMenuItem(move, 3,"Прикрепить пациента "));
            Console.WriteLine(WriteMenuItem(move, 4,"Просмотр входящих прикреплений "));
            Console.WriteLine(WriteMenuItem(move, 5,"Вывод всех прикреплений "));
            Console.WriteLine(WriteMenuItem(move, 6,"Выход "));
            Console.WriteLine($"{_limiter}{_limiter}");
            
            key = Console.ReadKey();
            if (key.Key == ConsoleKey.DownArrow)
            {
                MoveBottomMenuSelection(ref move, menuItemsCount: 6);
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                MoveTopMenuSelection(ref move, menuItemsCount: 6);
            }

            Console.Clear();
        }

        switch (move)
        {
            case 1:
                FindPatientByName();
                break;
            
            case 2:
                FindPatientByIIN();
                break;
            
            case 3:
                AttachPatient();
                break;
            
            case 4:
                AllPendingRequest();
                break;
            
            case 5:
                ReadAllRequest();
                break;
            
            case 6:
                return false;
        }
        
        return true;
    }

    private static void FindPatientByName()
    {
        Console.WriteLine("Имя");
        var name = Console.ReadLine();
        Console.WriteLine("Фамилия");
        var surname = Console.ReadLine();
        Console.WriteLine(_patientRepo.FindPatientByName(name,surname));
    }

    private static void FindPatientByIIN()
    {
        Console.WriteLine("ИИН");
        var iin = Console.ReadLine();
        Console.WriteLine(_patientRepo.FindPatientByIIN(iin));
    }

    private static void AttachPatient()
    {
        Console.WriteLine("Создание пациента");
        Console.WriteLine("Имя ");
        var name = Console.ReadLine();
        Console.WriteLine("Фамилия ");
        var surname = Console.ReadLine();
        Console.WriteLine("ИИН ");
        var iin = Console.ReadLine();
        Console.WriteLine("Название организации ");
        var nameOrg = Console.ReadLine();
        
        var patient = new Patient(name,surname,iin);
        _patientRepo.AddPatient(patient);
        
        var atachhment = new AttachmentRequest(patient, AttachmentRequest.AttachmentStatus.Pending, new MedicalOrganisation(nameOrg));
        _requestRepo.AddRequest(atachhment);
    }

    private static void AllPendingRequest()
    {
        var i = 0;
        foreach (var request in _requestRepo.GetAllRequest().Where(r => r.Status == AttachmentRequest.AttachmentStatus.Pending))
        {
            i++;
            Console.WriteLine(_limiter + _limiter);
            Console.WriteLine($"Заявка - {i}");
            Console.WriteLine($"Время поступление заявки {request.RegistrationTime}");
            Console.WriteLine($"Пациент - {request.Patient.Surname} {request.Patient.Name}");
            Console.WriteLine($"ИИН пациента - {request.Patient.IIN}");
            Console.WriteLine("Статус заявки - на расмотрение");
            Console.WriteLine(_limiter + _limiter);
            Console.WriteLine();
            Console.WriteLine("Жить или не жить данной заявки ?");
            Console.WriteLine("1 - Жить");
            Console.WriteLine("2 - Не жить");
            var answer = Console.ReadLine();
            if (answer == "1") _requestRepo.FindRequestAndReWrite(request.Id, AttachmentRequest.AttachmentStatus.Approved);
            if (answer == "2") _requestRepo.FindRequestAndReWrite(request.Id, AttachmentRequest.AttachmentStatus.Rejected);
        }
        
    }
    
    private static void ReadAllRequest()
    {
        var i = 0;
        foreach (var request in _requestRepo.GetAllRequest())
        {
            i++;
            Console.WriteLine(_limiter + _limiter);
            Console.WriteLine($"Заявка - {i}");
            Console.WriteLine($"Время поступление заявки {request.RegistrationTime}");
            Console.WriteLine($"Пациент - {request.Patient.Surname} {request.Patient.Name}");
            Console.WriteLine($"ИИН пациента - {request.Patient.IIN}");
            if (request.Status == AttachmentRequest.AttachmentStatus.Approved)
            {
                Console.WriteLine("Статус заявки - действующий ");
            }
            
            if (request.Status == AttachmentRequest.AttachmentStatus.Pending)
            {
                Console.WriteLine("Статус заявки - на расмотрение ");
            }
            
            if (request.Status == AttachmentRequest.AttachmentStatus.Rejected)
            {
                Console.WriteLine("Статус заявки - отклоненно ");
            }
            Console.WriteLine(_limiter + _limiter);
        }
        
    }

    private static bool UserMenu()
    {
        ConsoleKeyInfo key = new ConsoleKeyInfo();
        int move = 1;
        while (key.Key != ConsoleKey.Enter)
        {
            Console.WriteLine($"{_limiter} User menu {_limiter}");
            Console.WriteLine(WriteMenuItem(move, 1,"Поиск по ФИО "));
            Console.WriteLine(WriteMenuItem(move, 2,"Поиск по ИИН "));
            Console.WriteLine(WriteMenuItem(move, 3,"Прикрепить пациента "));
            Console.WriteLine(WriteMenuItem(move, 4,"Выход "));
            Console.WriteLine($"{_limiter}{_limiter}");
            
            key = Console.ReadKey();
            if (key.Key == ConsoleKey.DownArrow)
            {
                MoveBottomMenuSelection(ref move, menuItemsCount: 4);
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                MoveTopMenuSelection(ref move, menuItemsCount: 4);
            }

            Console.Clear();
        }

        switch (move)
        {
            case 1:
                FindPatientByName();
                break;
            
            case 2:
                FindPatientByIIN();
                break;
            
            case 3:
                AttachPatient();
                break;
            
            case 4:
                return false;
        }

        return true;
    } 
    
}