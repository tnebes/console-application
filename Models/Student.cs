using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_app.Models;

public class Student : Entity
{
    string FirstName;
    string LastName;
    List<long> groups;
}
