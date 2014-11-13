using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public static string GerarHash(string texto)
        {
            if (texto != string.Empty) { 
                char[] texto_char = texto.ToCharArray();

                int tamanho_da_hash = 64; //tamanho da hash a ser criada
                char[] texto_tamanho_fixo_array = new char[tamanho_da_hash]; //cria um novo array que vai deixar o texto em um tamanho fixo
                for (int x = 0; x < tamanho_da_hash; x++) 
                {
                    int y = x % (texto_char.Length); //y serve para copiar os characteres se o texto for menor do que o tamanho da hash
                    int modificador_x = x % 255; // modificador x é relacionado ao tamanho da hash (ele é usado para fazer contas com o charactere)
                    int modificador_y = y % 255; // modificador x é relacionado ao tamanho do testo (ele é usado para fazer contas com o charactere)
                    
                    int numero_char = (int)(texto_char[y]), numero_anterior; // numero_char converte o charactere em número
                    if (y > 0) { numero_anterior = (int)(texto_char[y - 1]); } else { numero_anterior = 1; } //numero_anterior é o charactere anterior também em número
                    int numero_novo_char = (int)(((numero_anterior + 32) + modificador_x * numero_char * (modificador_x * 0.3 + modificador_y)) % 255); //a maioria das operações de criptografia são feitas nesse passo
                    while (numero_novo_char < 33 || numero_novo_char == 127 || numero_novo_char == 0) { numero_novo_char += ((numero_anterior+numero_novo_char + numero_novo_char * (x + 1)) % 255); } //se o charactere for um charactere como backspace, enter, etc, o programa faz outras operações até o mesmo não poder ser utilizado
                    char novo_char = (char)(numero_novo_char); //transforma o número de volta em caractere
                    texto_tamanho_fixo_array[x] = novo_char; //insere o caractere no array
                }
                string texto_tamanho_fixo = string.Join("", texto_tamanho_fixo_array); //junta os arrays na string
                return texto_tamanho_fixo; //retorna o texto hasheado
            }
            return "Texto sem senha!Texto sem senha!Texto sem senha!Texto sem senha!"; //retorna esse testo, caso a string seja vazia
        }

        public static string Criptografar(string text, string cryptoKey)
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
        public static string Descriptografar(string text, string cryptoKey)
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
            string senha_criptografada = Criptografia.GerarHash(senha_descriptografada);

            string mensagem_criptografada = Criptografia.Criptografar(texto, senha_criptografada);
            StreamWriter writer = new StreamWriter(local_arquivo, true);
            using (writer)
            {
                writer.WriteLine(nome);
                writer.WriteLine(segundos);
                writer.WriteLine(mensagem_criptografada);
                writer.WriteLine("\n");
            }
        }
        public static int LerMensagem(string nome, string senha, string local_arquivo, int linha)
        {
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

                    string senha_criptografada = Criptografia.GerarHash(senha_descriptografada);

                    string mensagem_descriptografada = Criptografia.Descriptografar(texto_criptografado, senha_descriptografada);

                    //Console.Write(DecryptMessage(texto_criptografado, senha_criptografada_diminuida) + "\n");
                    Console.Write(mensagem_descriptografada + "\n");

                }
                linha++;
            }
            return linha;
        }
        static void Main(string[] args)
        {

            Console.Write("Digite seu nome:");
            string nome, senha, local_arquivo;
            int modo,linha=0;
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
                    linha = LerMensagem(nome, senha, local_arquivo,linha);
                    Thread.Sleep(300);
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
