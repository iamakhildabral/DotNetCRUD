using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        { 
            clientInfo.name = Request.Form["name"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.email = Request.Form["email"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.name.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.email.Length == 0
                || clientInfo.address.Length == 0)
            {
                errorMessage = "All Fields are requried :";
                return;
            }

            // saving data inot database
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                using(SqlConnection connection = new SqlConnection(connectionString)) 
                { 
                    connection.Open();
                    String sql = "INSERT INTO clients" +
                    "(name,email,phone,address)" +
                    "VALUES(@name,@email,@phone,@address);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();
                    }

                    successMessage = "New Client added Successfully";
                }
                
                
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "\nProblem in creating new Client";
                return;
            }

            clientInfo.name = ""; clientInfo.phone = "";clientInfo.address = "";
            clientInfo.email = "";
            if(successMessage == "New Client added Successfully")
            {
                Thread.Sleep(3000);
                Response.Redirect("/Clients/Index");
            }




        }
    }
}
