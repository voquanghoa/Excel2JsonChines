using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExcelVersion
{
    [DataContract]
    public class TestContent
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "questions")]
        public List<Question> Questions { get; set; }
    }
}
