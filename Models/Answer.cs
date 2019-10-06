using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jQuery_Sortable_From_Database
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
    }
        
}