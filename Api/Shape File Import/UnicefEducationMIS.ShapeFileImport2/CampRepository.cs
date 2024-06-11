using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace UnicefEducationMIS.ShapeFileImport2
{
    public class CampRepository
    {
        private const string ConnectionStringName = "unicefedudb_connection"; 
        private MySqlConnection _connection;
        private string _connectionString;

        public CampRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();
        }

        public void TrancateCampCoordinate()
        {
            Console.WriteLine("\n\n Truncating Previous Data.............");

            const string sql = "truncate camp_coordinate";
            try
            {
                using (var command = new MySqlCommand(sql, _connection))
                {
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Truncated All Successfully\n\n\n");
            }
            catch (Exception e)
            {
                throw e ;
            }

        }

        public void DeleteParticularCampData(int campId)
        {
            string sql = "delete from camp_coordinate where campId = "+campId;
            try
            {
                using (var command = new MySqlCommand(sql, _connection))
                {
                    command.ExecuteNonQuery();
                }
                Console.WriteLine($"Deleted All Previous Data of {campId} Successfully\n\n\n");
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void InsertCampCoordinate(CampCoordinate campCoordinate)
        {
            const string query = "insert into Camp_Coordinate(DateCreated,CreatedBy,CampId, Latitude, Longitude) values(@DateCreated,1,@CampId, @Latitude, @Longitude)";

            ExecuteSql(query, campCoordinate);
        }

        private void ExecuteSql(string sql, CampCoordinate campCoordinate)
        {
            try
            {
                using (var command = new MySqlCommand(sql, _connection))
                {
                    List<MySqlParameter> list = new List<MySqlParameter>();
                    list.Add(new MySqlParameter("@DateCreated", DateTime.Now));
                    list.Add(new MySqlParameter("@CampId", campCoordinate.CampId));
                    list.Add(new MySqlParameter("@Latitude", campCoordinate.Latitude));
                    list.Add(new MySqlParameter("@Longitude", campCoordinate.Longitude));

                    command.Parameters.AddRange(list.ToArray<MySqlParameter>());

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<Dictionary<string, object>> GetCamps()
        {
            const string query = "select * from Camp";

            var result = GetData(_connectionString, query);
            return result;
        }

        private List<Dictionary<string, object>> GetData(string connectionString, string sql)
        {
            var allRow = new List<Dictionary<string, object>>();
            try
            {
                using (var command = new MySqlCommand(sql, _connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();

                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                var fieldName = reader.GetName(i);
                                var fieldValue = reader[i];

                                row[fieldName] = fieldValue;
                            }

                            allRow.Add(row);
                        }

                        reader.Close();
                    }
                    return allRow;
                }
            }catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();

                _connection.Dispose();
            }
        }
    }
}
