using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace Criptografia___APS
{
    public class Criptografia
    {
        /// <summary>     
        /// Vetor de bytes utilizados para a criptografia (Chave Externa)     
        /// </summary>     
        private static byte[] bIV = 
    { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18,
        0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };

        /// <summary>     
        /// Representação de valor em base 64 (Chave Interna)    
        /// O Valor representa a transformação para base64 de     
        /// um conjunto de 32 caracteres (8 * 32 = 256bits)    
        /// A chave é: "Criptografias com Rijndael / AES"     
        /// </summary>     

        /// <summary>     
        /// Metodo de criptografia de valor     
        /// </summary>     
        /// <param name="text">valor a ser criptografado</param>     
        /// <returns>valor criptografado</returns>
        public static string Encrypt(string text, string cryptoKey)
        {
            try
            {
                // Se a string não está vazia, executa a criptografia
                if (!string.IsNullOrEmpty(text))
                {
                    // Cria instancias de vetores de bytes com as chaves                
                    byte[] bKey = Convert.FromBase64String(cryptoKey);
                    byte[] bText = new UTF8Encoding().GetBytes(text);

                    // Instancia a classe de criptografia Rijndael
                    Rijndael rijndael = new RijndaelManaged();

                    // Define o tamanho da chave "256 = 8 * 32"                
                    // Lembre-se: chaves possíves:                
                    // 128 (16 caracteres), 192 (24 caracteres) e 256 (32 caracteres)                
                    rijndael.KeySize = 256;

                    // Cria o espaço de memória para guardar o valor criptografado:                
                    MemoryStream mStream = new MemoryStream();
                    // Instancia o encriptador                 
                    CryptoStream encryptor = new CryptoStream(
                        mStream,
                        rijndael.CreateEncryptor(bKey, bIV),
                        CryptoStreamMode.Write);

                    // Faz a escrita dos dados criptografados no espaço de memória
                    encryptor.Write(bText, 0, bText.Length);
                    // Despeja toda a memória.                
                    encryptor.FlushFinalBlock();
                    // Pega o vetor de bytes da memória e gera a string criptografada                
                    return Convert.ToBase64String(mStream.ToArray());
                }
                else
                {
                    // Se a string for vazia retorna nulo                
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Se algum erro ocorrer, dispara a exceção            
                throw new ApplicationException("Erro ao criptografar", ex);
            }
        }

        /// <summary>     
        /// Pega um valor previamente criptografado e retorna o valor inicial 
        /// </summary>     
        /// <param name="text">texto criptografado</param>     
        /// <returns>valor descriptografado</returns>     
        public static string Decrypt(string text, string cryptoKey)
        {
            try
            {
                // Se a string não está vazia, executa a criptografia           
                if (!string.IsNullOrEmpty(text))
                {
                    // Cria instancias de vetores de bytes com as chaves                
                    byte[] bKey = Convert.FromBase64String(cryptoKey);
                    byte[] bText = Convert.FromBase64String(text);

                    // Instancia a classe de criptografia Rijndael                
                    Rijndael rijndael = new RijndaelManaged();

                    // Define o tamanho da chave "256 = 8 * 32"                
                    // Lembre-se: chaves possíves:                
                    // 128 (16 caracteres), 192 (24 caracteres) e 256 (32 caracteres)                
                    rijndael.KeySize = 256;

                    // Cria o espaço de memória para guardar o valor DEScriptografado:               
                    MemoryStream mStream = new MemoryStream();

                    // Instancia o Decriptador                 
                    CryptoStream decryptor = new CryptoStream(
                        mStream,
                        rijndael.CreateDecryptor(bKey, bIV),
                        CryptoStreamMode.Write);

                    // Faz a escrita dos dados criptografados no espaço de memória   
                    decryptor.Write(bText, 0, bText.Length);
                    // Despeja toda a memória.                
                    decryptor.FlushFinalBlock();
                    // Instancia a classe de codificação para que a string venha de forma correta         
                    UTF8Encoding utf8 = new UTF8Encoding();
                    // Com o vetor de bytes da memória, gera a string descritografada em UTF8       
                    return utf8.GetString(mStream.ToArray());
                }
                else
                {
                    // Se a string for vazia retorna nulo                
                    return null;
                }
            }
            catch
            {
                // Se algum erro ocorrer, dispara a exceção            
                return "Erro ao descriptografar";
            }
        }
    }
    class Program
    {
        public static void EscreverMensagem(string nome, string senha, string local_arquivo)
        {
            string texto;
            Console.Write("Digite sua mensagem: ");
            texto = Console.ReadLine();
            string hora = DateTime.Now.ToString("ddMMyyhmmss");
            string segundos = DateTime.Now.ToString("ss");
            string senha_descriptografada = senha + hora;

            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty, senha_sha256 = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(senha_descriptografada), 0, Encoding.ASCII.GetByteCount(senha_descriptografada));
            foreach (byte bit in crypto) { senha_sha256 += bit.ToString("x2"); }
            string senha_32bytes_md5 = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(senha_sha256.Substring(0, 32)));
            string mensagem_criptografada = Criptografia.Encrypt(texto, senha_32bytes_md5);
            StreamWriter writer = new StreamWriter(local_arquivo, true);
            using (writer)
            {
                writer.WriteLine(nome);
                writer.WriteLine(segundos);
                writer.WriteLine(mensagem_criptografada);
                writer.WriteLine("\n");
            }
        }
        public static void LerMensagem(string nome, string senha, string local_arquivo)
        {
            int linha = 0;
            Console.WriteLine("Lendo Mensagens...");
            string[] texto_arquivo = File.ReadAllLines(local_arquivo);
            bool check_nome = false;
            string horario_mensagem = "";
            while (linha < texto_arquivo.Length)
            {
                int segundos = 0;
                string texto_criptografado;
                if (linha % 5 == 0 && texto_arquivo[linha] != "")
                {
                    if (texto_arquivo[linha] != nome)
                    {
                        check_nome = true;
                        Console.Write(texto_arquivo[linha] + " disse: ");
                    }
                    else { check_nome = false; }
                }
                if (linha % 5 == 1 && texto_arquivo[linha] != "" && check_nome == true)
                {
                    int.TryParse(texto_arquivo[linha], out segundos);
                    DateTime novo;
                    DateTime agora = DateTime.Now;
                    if (agora.Second < segundos)
                    {
                        novo = new DateTime(agora.Year, agora.Month, agora.Day, agora.Hour, agora.Minute - 1, segundos);
                    }
                    else
                    {
                        novo = new DateTime(agora.Year, agora.Month, agora.Day, agora.Hour, agora.Minute, segundos);
                    }
                    horario_mensagem = novo.ToString("ddMMyyhmmss");
                }
                if (linha % 5 == 2 && texto_arquivo[linha] != "" && check_nome == true)
                {
                    texto_criptografado = texto_arquivo[linha];


                    string senha_descriptografada = senha + horario_mensagem;

                    SHA256Managed crypt = new SHA256Managed();
                    string hash = String.Empty, senha_sha256 = String.Empty;
                    byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(senha_descriptografada), 0, Encoding.ASCII.GetByteCount(senha_descriptografada));
                    foreach (byte bit in crypto) { senha_sha256 += bit.ToString("x2"); }
                    string senha_32bytes_md5 = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(senha_sha256.Substring(0, 32)));

                    string mensagem_descriptografada = Criptografia.Decrypt(texto_criptografado, senha_32bytes_md5);

                    //Console.Write(DecryptMessage(texto_criptografado, senha_criptografada_diminuida) + "\n");
                    Console.Write(mensagem_descriptografada + "\n");
                    Console.ReadKey();
                }
                linha++;
            }
        }
        static void Main(string[] args)
        {

            Console.Write("Digite seu nome:");
            string nome, senha, local_arquivo;
            int modo;
            nome = Console.ReadLine();
            Console.Write("Digite uma senha: ");
            senha = Console.ReadLine();
            Console.Write("Digite 1 para modo de escrita ou 2 para modo leitura: ");
            modo = int.Parse(Console.ReadLine());
            Console.Write("Digite o local onde o arquivo será salvo: ");
            local_arquivo = Console.ReadLine();
            while (true)
            {
                if (modo == 1)
                {
                    EscreverMensagem(nome, senha, local_arquivo);
                }
                else if (modo == 2)
                {
                    LerMensagem(nome, senha, local_arquivo);
                }
                else
                {
                    Console.Write("Digite 1 para modo de escrita ou 2 para modo leitura: ");
                    modo = int.Parse(Console.ReadLine());
                }
            }
        }
    }
}
