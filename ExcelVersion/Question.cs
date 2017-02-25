using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExcelVersion
{
    [DataContract]
    public class Question
    {
        [DataMember(Name = "question")]
        public string Title { get; set; }

        [DataMember(Name = "anwers")]
        public List<string> Answers { get; set; }

        [DataMember(Name = "correct")]
        public int Correct { get; set; }
    }
}
