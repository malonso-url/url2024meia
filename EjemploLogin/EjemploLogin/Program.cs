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
            string path = "C:\\Ejemplos\\SistemaArchivos\\users.txt";
            string[] lineas = { };
            bool usuarioEncontrado = false;
            string nombre = "";

            string path_enhanced = "C:\\Ejemplos\\SistemaArchivos\\users_enhanced.txt";
            string registro = GetRecord(path_enhanced, 2);


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
                if (campos[1].Equals(username))
                {

                    if (campos[2].ToLower().Equals( GetMD5Hash(password).ToLower() ))
                    {
                        usuarioEncontrado = true;
                        nombre = campos[0];
                    }
                    
                } 
            }

            if (usuarioEncontrado)
            {
                Console.WriteLine("Bienvenido " + nombre);
            }
            else 
            {
                Console.WriteLine("Usuario o Contraseña incorrecta");
            }

            Console.ReadKey();
        }

    }
}
