using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using criptografias;

namespace ler_mensagens
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Leitor de Mensagens";
            Console.Write("Digite seu nome:");
            string nome, senha, local_arquivo;
            int linha = 0;
            nome = Console.ReadLine();
            Console.Write("Digite uma senha: ");
            senha = Console.ReadLine();
            Console.Write("Digite o local onde o arquivo será salvo: ");
            local_arquivo = Console.ReadLine();
            while (true)
            {
                linha = principal.LerMensagem(nome, senha, local_arquivo, linha);
                Thread.Sleep(500);
            }
        }
    }
}
