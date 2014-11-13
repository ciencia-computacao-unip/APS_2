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
        public static string Criptografar(string texto_descriptografado, string senha)
        {
            char[] texto_descriptografado_char = texto_descriptografado.ToCharArray();
            int tamanho_texto = texto_descriptografado.Length;
            char[] senha_char = senha.ToCharArray();
            int tamanho_senha = senha.Length;
            string texto_criptografado = string.Empty;
            for (int x = 0; x < tamanho_texto; x++)
            {
                int y = x % tamanho_senha;
                int numero_char_texto = (int)(texto_descriptografado_char[x]);
                int numero_char_senha = (int)(senha_char[y]);
                int novo_numero = numero_char_texto + numero_char_senha;
                texto_criptografado += novo_numero + "-";
            }
            return texto_criptografado;
        }
        public static string Descriptografar(string texto_criptografado, string senha)
        {
            string texto_descriptografado = texto_criptografado;
            return texto_descriptografado;
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
