using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace EjemploLogin
{

    internal class Program
    {
        static int RECORD_SIZE = 84;

        static string GetRecord(string path, int posicion) {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fs)) { 
                fs.Seek(posicion * RECORD_SIZE, SeekOrigin.Begin);

                byte[] line = reader.ReadBytes(RECORD_SIZE);
                string record = Encoding.ASCII.GetString(line).Trim();

                return record;
            }
        }

        static string GetMD5Hash(string input) 
        {
            
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++) {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        static void Main(string[] args)
        {
            string path = "C:\\Ejemplos\\SistemaArchivos\\users_index.txt";
            string[] lineas = { };
            bool usuarioEncontrado = false;


            string path_enhanced = "C:\\Ejemplos\\SistemaArchivos\\users_enhanced.txt";
            

            if (File.Exists(path))
            {
                lineas = File.ReadAllLines(path);
            }

            Console.WriteLine("Ingrese su correo y contraseña");
            Console.Write("Usuario: ");
            String username = Console.ReadLine();
            Console.Write("Contraseña: ");
            String password = Console.ReadLine();


            for (int i = 0; i < lineas.Length; i++)
            {
                string[] campos = lineas[i].Split('|');
                if (campos[0].Trim().Equals(username))
                {
                    int index_enhanced_file = Convert.ToInt32(campos[1]);
                    string registro = GetRecord(path_enhanced, index_enhanced_file);
                    string nombre = registro.Split('|')[0].Trim();
                    string password_guardado = registro.Split('|')[2].Trim();

                    if (password_guardado.ToLower().Equals( GetMD5Hash(password).ToLower() )) //maalonsog@correo.url.edu.gt  password123
                    {
                        usuarioEncontrado = true;
                        Console.WriteLine("Bienvenido " + nombre);
                    }
                    
                } 
            }

            if (!usuarioEncontrado) { 
                Console.WriteLine("Usuario o Contraseña incorrecta");
            }

            Console.ReadKey();
        }

    }
}
