using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace TesteMultaNovo.Controllers
{
    [ApiController, Route("[controller]")]
    public class MultasController : ControllerBase
    {
        private readonly string connectionString = "Data Source=.;Initial Catalog=TesteMulta;Integrated Security=True;";

        // Método HTTP POST para inserir uma multa
        [HttpPost]
        public IActionResult InserirMulta([FromBody] Multa multa)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Multa (placa, valor) VALUES (@placa, @valor)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@placa", multa.Placa);
                        command.Parameters.AddWithValue("@valor", multa.Valor);
                        command.ExecuteNonQuery();
                    }
                }
                return Ok("Multa inserida com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Método HTTP GET para obter todas as multas
        [HttpGet]
        public IActionResult ObterMultas()
        {
            try
            {
                List<Multa> multas = new List<Multa>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT placa, valor FROM Multa";
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                multas.Add(new Multa
                                {
                                    Placa = reader["placa"].ToString(),
                                    Valor = Convert.ToDecimal(reader["valor"])
                                });
                            }
                        }
                    }
                }
                return Ok(multas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class Multa
    {
        public string Placa { get; set; }
        public decimal Valor { get; set; }
    }
}
