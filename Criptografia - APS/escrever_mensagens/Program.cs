using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using criptografias;

namespace escrever_mensagens
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Escritor de Mensagens";
            Console.Write("Digite seu nome:");
            string nome, senha, local_arquivo;
            nome = Console.ReadLine();
            Console.Write("Digite uma senha: ");
            senha = Console.ReadLine();
            Console.Write("Digite o local onde o arquivo será salvo: ");
            local_arquivo = Console.ReadLine();
            while (true)
            {
                principal.EscreverMensagem(nome, senha, local_arquivo);
            }
        }
    }
}
