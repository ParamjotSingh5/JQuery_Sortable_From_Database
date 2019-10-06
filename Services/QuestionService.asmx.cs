using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace jQuery_Sortable_From_Database
{
    /// <summary>
    /// Summary description for QuestionService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class QuestionService : System.Web.Services.WebService
    {

        [WebMethod]
        public void GetQuestionData()
        {
            string Cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            QuestionPageData questionPageData = new QuestionPageData()
            {
                QuestionOptions = new List<Options>(),
                AnswerOptions = new List<Options>()
            };

            using (SqlConnection con = new SqlConnection(Cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("GetQuestionDataById", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter paramQuestionId = new SqlParameter();
                paramQuestionId.ParameterName = "@questionid";
                paramQuestionId.Value = 1;
                da.SelectCommand.Parameters.Add(paramQuestionId);

                DataSet dataSet = new DataSet();
                da.Fill(dataSet);

                questionPageData.QuestionId = Convert.ToInt32(dataSet.Tables[0].Rows[0]["Id"]);
                questionPageData.QuestionText = dataSet.Tables[0].Rows[0]["QuestionText"].ToString();

                foreach(DataRow dataRow in dataSet.Tables[1].Rows)
                {
                    Options questionOptions = new Options();
                    questionOptions.Id = Convert.ToInt32( dataRow["Id"]);
                    questionOptions.OptionText = dataRow["OptionText"].ToString();
                    questionOptions.QuestionId = Convert.ToInt32(dataRow["QuestionId"]);
                    questionPageData.QuestionOptions.Add(questionOptions);      
                }

                foreach(DataRow dataRow in dataSet.Tables[2].Rows)
                {
                    Options answerOptions = new Options();
                    answerOptions.Id = Convert.ToInt32(dataRow["Id"]);
                    answerOptions.OptionText = dataRow["OptionText"].ToString();
                    answerOptions.QuestionId = Convert.ToInt32(dataRow["QuestionId"]);
                    questionPageData.AnswerOptions.Add(answerOptions);
                }

            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(questionPageData));
        }

        [WebMethod]
        public void GetAnswerData()
        {
            string Cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            Answer answer = new Answer();

            using (SqlConnection con = new SqlConnection(Cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("GetAnswerByQuestionId", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter paramQuestionId = new SqlParameter();
                paramQuestionId.ParameterName = "@questionid";
                paramQuestionId.Value = 1;
                da.SelectCommand.Parameters.Add(paramQuestionId);

                DataSet dataSet = new DataSet();
                da.Fill(dataSet);
                
                answer.Id = Convert.ToInt32(dataSet.Tables[0].Rows[0]["Id"]);
                answer.QuestionId = Convert.ToInt32(dataSet.Tables[0].Rows[0]["QuestionId"]);
                answer.AnswerText = dataSet.Tables[0].Rows[0]["Answer"].ToString();

            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(answer));
        }
    }
}
