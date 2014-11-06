using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.IO;
namespace ConsoleApplication7{
    class Program{
        public static void EscreverMensagem(string nome, string senha, string local_arquivo){
            string texto;
            Console.Write("Digite sua mensagem: ");
            texto = Console.ReadLine();
            string hora = DateTime.Now.ToString("ddMMyyhmmss");
            string segundos = DateTime.Now.ToString("ss");
            string senha_descriptografada = senha + hora;
            var ue = new UnicodeEncoding();
            var byteSourceText = ue.GetBytes(senha_descriptografada);
            var byteHash = new System.Security.Cryptography.SHA256Managed().ComputeHash(byteSourceText);
            string senha_criptografada = Convert.ToBase64String(byteHash);
            string senha_criptografada_diminuida = senha_criptografada.Substring(0, 32);
            int x = 0;
            byte[] teste = new byte[texto.Length];
            char seila;
            while (x < texto.Length){
                seila = Convert.ToChar(texto.Substring(x, 1));
                teste[x] = Convert.ToByte(seila);
                x++;
            }
            string mensagem_criptografada = EncryptMessage(teste, senha_criptografada_diminuida);
            StreamWriter writer = new StreamWriter(local_arquivo, true);
            using (writer){
                writer.WriteLine(nome);
                writer.WriteLine(segundos);
                writer.WriteLine(mensagem_criptografada);
                writer.WriteLine("\n");
            }
        }
        public static void LerMensagem(string nome, string senha, string local_arquivo){
            int linha=0;
            Console.WriteLine("Lendo Mensagens...");
            string[] texto_arquivo = File.ReadAllLines(local_arquivo);
            bool check_nome = false;
            while(linha < texto_arquivo.Length){
                int segundos = 0;
                string texto_criptografado;
                if (linha % 5 == 0 && texto_arquivo[linha] != "")
                {
                    if (texto_arquivo[linha] != nome)
                    {
                        check_nome = true;
                        Console.Write(texto_arquivo[linha]+" disse: ");
                    }else{check_nome = false;}
                }
                if (linha % 5 == 1 && texto_arquivo[linha] != "" &&check_nome==true)
                {
                    int.TryParse(texto_arquivo[linha], out segundos);
                    Console.Write("aos "+segundos+" segundos: ");
                }
                if (linha % 5 == 2 && texto_arquivo[linha] != "" && check_nome == true)
                {
                    texto_criptografado = texto_arquivo[linha];
                    Console.Write(texto_arquivo[linha] + "\n");
                }
                linha++;
            }
        }
        public static string DecryptMessage(string text, string key){
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 256;
            aes.Padding = PaddingMode.Zeros;
            aes.Mode = CipherMode.CBC;
            aes.Key = Encoding.Default.GetBytes(key);
            text = Encoding.Default.GetString(Convert.FromBase64String(text));
            string IV = text;
            IV = IV.Substring(IV.IndexOf("-[--IV-[-") + 9);
            text = text.Replace("-[--IV-[-" + IV, "");
            text = Convert.ToBase64String(Encoding.Default.GetBytes(text));
            aes.IV = Encoding.Default.GetBytes(IV);
            ICryptoTransform AESDecrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] buffer = Convert.FromBase64String(text);
            return Encoding.Default.GetString(AESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
        }
        public static string EncryptMessage(byte[] text, string key){
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 256;
            aes.Padding = PaddingMode.Zeros;
            aes.Mode = CipherMode.CBC;
            aes.Key = Encoding.Default.GetBytes(key);
            aes.GenerateIV();
            string IV = ("-[--IV-[-" + Encoding.Default.GetString(aes.IV));
            ICryptoTransform AESEncrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] buffer = text;
            return
            Convert.ToBase64String(Encoding.Default.GetBytes(Encoding.Default.GetString(AESEncrypt.TransformFinalBlock(buffer, 0, buffer.Length)) + IV));
        }
        static void Main(string[] args){
            Console.Write("Digite seu nome:");
            string nome, senha,local_arquivo;
            int modo;
            nome = Console.ReadLine();
            Console.Write("Digite uma senha: ");
            senha = Console.ReadLine();
            Console.Write("Digite 1 para modo de escrita ou 2 para modo leitura: ");
            modo = int.Parse(Console.ReadLine());
            Console.Write("Digite o local onde o arquivo será salvo: ");
            local_arquivo = Console.ReadLine();
            while (true){
                if (modo == 1){
                    EscreverMensagem(nome, senha,local_arquivo);
                }
                else if (modo == 2){
                    LerMensagem(nome,senha,local_arquivo);
                }
                else{
                    Console.Write("Digite 1 para modo de escrita ou 2 para modo leitura: ");
                    modo = int.Parse(Console.ReadLine());
                }
            }
        }
    }
}
