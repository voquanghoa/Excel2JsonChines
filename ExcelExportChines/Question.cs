using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExcelExportChines
{
    [DataContract]
    public class Question
    {
        [DataMember(Name = "questionTitle")]
        public string Title { get; set; }

        [DataMember(Name = "anwers")]
        public List<string> Answers { get; set; }

        [DataMember(Name = "correctAnswer")]
        public int Correct { get; set; }
    }
}
