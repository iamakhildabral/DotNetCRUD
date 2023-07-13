using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClients = new List<ClientInfo>();
        public void OnGet()
        {
            try{
                //creating database connection string
            
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //setting sql query
                    String sql = "SELECT * FROM clients";
                    
                    // executing sql queries
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = "" + reader.GetInt32(0); // for string conversion "" is added
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                                clientInfo.created_at = reader.GetDateTime(5).ToString();
                                // since we r showing everything in string on webpage

                                listClients.Add(clientInfo);
                                Console.WriteLine(listClients);

                            }
                        }
                    }
                }


            }catch(Exception ex)
            {
                Console.WriteLine("Exception : "+ex.ToString());
            }
        }
    }

    public class ClientInfo
    {
        public string id;
        public string name;
        public string email;
        public string phone;
        public string address;
        public string created_at;
    }
}
