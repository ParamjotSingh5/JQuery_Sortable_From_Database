using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jQuery_Sortable_From_Database
{
    public class QuestionPageData
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<Options> QuestionOptions { get; set; }
        public List<Options> AnswerOptions { get; set; }
    }
}