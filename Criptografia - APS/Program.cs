using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace criptografias
{
    public class Criptografia
    {
        public static string GerarHash(string texto)
        {
            if (texto != string.Empty) { 
                char[] texto_char = texto.ToCharArray();
                char[] texto_char_ao_contrario = texto.ToCharArray();
                Array.Reverse(texto_char_ao_contrario, 0, texto_char_ao_contrario.Length);
                int tamanho_da_hash = 64; //tamanho da hash a ser criada
                char[] texto_tamanho_fixo_array = new char[tamanho_da_hash]; //cria um novo array que vai deixar o texto em um tamanho fixo
                int total_char = 1, total_char_contrario=1;
                for (int x = 0; x < tamanho_da_hash; x++) 
                {
                    int y = x % (texto_char.Length); //y serve para copiar os characteres se o texto for menor do que o tamanho da hash
                    int modificador_x = x % 500; // modificador x é relacionado ao tamanho da hash (ele é usado para fazer contas com o charactere)
                    int modificador_y = y % 500; // modificador x é relacionado ao tamanho do testo (ele é usado para fazer contas com o charactere)
                    int modificador_x_negativo = 500 % texto_char.Length * y;
                    int modificador_y_negativo = 500 % texto_char.Length;

                    int numero_char = (int)(texto_char[y]), numero_anterior; // numero_char converte o charactere em número
                    total_char += numero_char;
                    int numero_char_ao_contrario = (int)(texto_char_ao_contrario[y]); // numero_char converte o charactere em número
                    total_char_contrario += numero_char_ao_contrario;
                    int modificador_x_inicial = 1;
                    if (x < 5) { modificador_x_inicial = 324 * numero_char_ao_contrario; }
                    if (y > 0) { numero_anterior = (int)(texto_char[y - 1]); } else { numero_anterior = 1; } //numero_anterior é o charactere anterior também em número
                    int numero_novo_char = (int)((total_char_contrario * total_char * modificador_x_inicial * modificador_x_negativo * modificador_y_negativo * numero_char * texto_char.Length * numero_char_ao_contrario + modificador_x * modificador_y) % 500); //a maioria das operações de criptografia são feitas nesse passo
                    while (numero_novo_char <= 33 || numero_novo_char == 127) { numero_novo_char = ((numero_novo_char + x + y +3) % 500); } //se o charactere for um charactere como backspace, enter, etc, o programa faz outras operações até o mesmo não poder ser utilizado
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
            char[] texto_descriptografado_char = texto_descriptografado.ToCharArray(); //pega o texto em string e transforma em um array de caracteres
            int tamanho_texto = texto_descriptografado.Length; //pega o tamanho do texto
            char[] senha_char = senha.ToCharArray(); //pega a senha em string e transforma em um array de caracteres
            int tamanho_senha = senha.Length; //pega o tamanho da senha
            string texto_criptografado = string.Empty; //especificando a variável para que o FOR abaixo insira os caracteres
            for (int x = 0; x < tamanho_texto; x++)
            {
                int y = x % tamanho_senha; //y serve para contar os caracteres da senha e quando acabar, usar os caracteres da senha, ela volta a ser usada.
                int numero_char_texto = (int)(texto_descriptografado_char[x]); //transforma o charactere em número
                int numero_char_senha = (int)(senha_char[y]); //transforma o charactere da senha em número
                int novo_numero = numero_char_texto + numero_char_senha; //soma os dois characteres
                texto_criptografado += novo_numero + "-"; //insere o numero e um "-" no texto criptografado
            }
            return texto_criptografado;
        }
        public static string Descriptografar(string texto_criptografado, string senha) //descriptografa o texto a se a senha for igual
        {
            char[] senha_char = senha.ToCharArray();
            string[] separador = new string[] { "-" };
            string[] texto_criptografado_array = texto_criptografado.Split(separador, StringSplitOptions.None);
            int x = 0;
            string texto_descriptografado = string.Empty;
            foreach (string caractere in texto_criptografado_array)
            {
                int caractere_numero;
                bool convertido = Int32.TryParse(caractere, out caractere_numero);
                if (convertido == true)
                {
                    int y = x % senha_char.Length;
                    int novo_charactere_numero = caractere_numero - (int)senha_char[y];
                    char novo_charactere = (char)novo_charactere_numero;
                    texto_descriptografado += novo_charactere;
                    x++;
                }
            }

            //texto_descriptografado = texto_criptografado;
            return texto_descriptografado;
        }
    }
    public class principal
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
            if (File.Exists(local_arquivo))
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

                        string mensagem_descriptografada = Criptografia.Descriptografar(texto_criptografado, senha_criptografada);

                        Console.Write(mensagem_descriptografada + "\n");

                    }
                    linha++;
                }
            }
            return linha;
        }
        static void Main(string[] args) // Este main existe apenas para, se o professor quiser apenas um executável, é possível gera-lo facilmente.
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
                    Thread.Sleep(500);
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
