using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo= new ClientInfo();
        public String successMessage = "";
        public String errorMessage = "";

        public void OnGet()
        {
            //on GET we will take the id and start showing the desired record to edit
            String id = Request.Query["id"];

            try
            {
                //creating database connection string

                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    String sql = "SELECT * FROM clients WHERE id=@id";
 
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            
                            if (reader.Read())
                            {
                                 
                                clientInfo.id = "" + reader.GetInt32(0); // for string conversion "" is added
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "/n Unable to Edit your client data";
            }
        }
        public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email= Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address= Request.Form["address"];

            if (clientInfo.id.Length == 0 || clientInfo.name.Length==0 ||
                clientInfo.email.Length==0 || clientInfo.phone.Length==0 ||
                clientInfo.address.Length==0)
            {
                errorMessage = "All Fields are mandatory :";
                return;
            }

            // saving data inot database
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "UPDATE clients " +
                    "SET name=@name ,email= @email ,phone= @phone ,address= @address " +
                    "WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@id", clientInfo.id);

                        command.ExecuteNonQuery();
                    }

                    successMessage = "Client Details are updated Successfully";
                }


            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "/nProblem in Updating Client details: ";
                return;
            }

            if(successMessage == "Client Details are updated Successfully")
            {
                Response.Redirect("/Clients/Index");
            }
        }
    }
}
