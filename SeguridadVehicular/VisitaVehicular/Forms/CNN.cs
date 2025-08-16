using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace VisitaVehicular.Forms
{

    public class CNN
    {

        //private string v;
        //private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MiConexionSQLServer"].ConnectionString;
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["MiConexionSQLServer"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public void ILogE(string codexE, string msgE, string usuarioE)
        {
            try
            {
                using (SqlConnection conexion = GetConnection())
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand("REGISTRALOGE", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add("@p_Cod", SqlDbType.VarChar, 5).Value = codexE;
                        comando.Parameters.Add("@p_Desc", SqlDbType.VarChar, 100).Value = msgE;
                        comando.Parameters.Add("@p_usu", SqlDbType.VarChar, 15).Value = usuarioE;
                        comando.ExecuteNonQuery();
                    }
                    conexion.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }


        public static string Encrypt(string plainText)
        {
            string key = "nJ_dQGYjn%rN@)2!"; // Clave de 16 caracteres
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.GenerateIV(); // Genera un IV aleatorio

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Escribir el IV en el stream
                    memoryStream.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            string key = "nJ_dQGYjn%rN@)2!"; // Clave de 16 caracteres
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);

                using (MemoryStream memoryStream = new MemoryStream(fullCipher))
                {
                    // Leer el IV del stream
                    byte[] iv = new byte[aes.BlockSize / 8];
                    memoryStream.Read(iv, 0, iv.Length);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }


    }


}