using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace TesteMultaNovo.Controllers
{
    [ApiController, Route("[controller]")]
    public class MultasController : ControllerBase
    {
        private readonly string connectionString = "Data Source=.;Initial Catalog=TesteMulta;Integrated Security=True;";

        [HttpGet]
        public ActionResult ObterMultas()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta à tabela Multa
                    string selectQuery = "SELECT id, placa, valor FROM Multa";
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<object> multas = new List<object>();
                            while (reader.Read())
                            {
                                var multa = new
                                {
                                    Id = reader.GetInt32(0),
                                    Placa = reader.GetString(1),
                                    Valor = reader.GetDecimal(2)
                                };
                                multas.Add(multa);
                            }

                            if (multas.Count() < 1) return NotFound("Nao foram encontrados nenhuma multa na base de dados");  

                            return Ok(multas);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult InserirMulta(string placa, int? manualId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Criação do objeto multa com os atributos
                    int id = manualId ?? 1; // Implemente a lógica para obter o próximo ID
                    double valor = 100.50; // Exemplo de valor

                    // Inserção na tabela Multa
                    string insertQuery = "INSERT INTO Multa (id, placa, valor) VALUES (@id, @placa, @valor)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@placa", placa);
                        command.Parameters.AddWithValue("@valor", valor);
                        command.ExecuteNonQuery();
                    }

                    return Ok($"Multa inserida com sucesso! ID: {id}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
