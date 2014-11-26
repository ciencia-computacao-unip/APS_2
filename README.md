Como rodar o programa:

1- Primeiramente, deve-se atentar para a existencia de 2 programas : 'Leitor de Mensagens' e 'Escritor de Mensagens';
2- Abra a solução do projeto;
3- Execute os 2 programas (caso veja pelo código fonte, apenas aperte em 'Start Debug' e os 2 programas serão abertos.);
4- Ao rodar os programas, os 2 perguntarão alguns dados como Nome, Senha e Local do Arquivo;
5- Os 2 programas devem conter Nome diferentes, se você colocar o mesmo nome, o programa não enviará a mensagem para o próximo;
6- Além disso, as senhas devem ser idênticas;
7- No caso do Local do Arquivo, deve ser especificado um mesmo local, por exemplo: "C:/arquivo.txt"
8- Assim que os 2 programas estiverem configurados, começaram a executar suas funções. Para testar, envie uma mensagem a partir do 'Escritor de Mensagens', a mesma será recebida no 'Leitor de Mensagens'.


Como funciona o programa para criptografar:

	1- A cada mensagem do usuário:
		1.1- O programa pega a senha digitada no inicio do programa e concatena com o horário (ddMMyyhmmss);
		1.2- Também são guardados os segundos em que isso aconteceu (sim, apenas os segundos, de 0 a 60);
		1.3- A partir dessa string (senha+ddMMyyhmmss) é gerada a hash da senha, que será a senha usada para criptografar e descriptografar;
		1.4- o texto é criptografado com uma chave simétrica, que será a a hash da senha:
			1.4.1- O texto e a senha (hash) são transformados em array de caracteres, cada um;
			1.4.2- O array da senha é repetido para que fique do mesmo tamanho do array do texto;
			1.4.3- Para cada caractere do texto:
				1.4.3.1- Os caracteres do texto e da senha são convertidos em números;
				1.4.3.2- Os números são somados;
				1.4.3.3- O número é adicionado ao final de uma string, juntamente com o caractere "-".
			1.4.4- O texto criptografado sai na forma de "302-343-24-423-".
		1.5- O programa agora grava os valores encontrados em cada linha no arquivo selecionado:
			1.5.1- Adiciona no arquivo a variável nome e muda de linha;
			1.5.2- Adiciona no arquivo o texto criptografado e muda de linha;
			1.5.3- Adiciona no arquivo os segundos que foram gerados no início da criptografia;
			1.5.4- Adiciona mais 2 linhas no final do arquivo, para que seja possível ler a cada mensagem (nada é deletado).
	
	
	
Como funciona o programa para descriptografar:	
	1- A cada 5 linhas recebidas no arquivo:
		1.1- Encontra o horário em que o usuário mandou a mensagem, a partir dos segundos (assim, se passar mais de 1 minuto do envio da mensagem, a senha se torna errada);
		1.2- O programa pega a senha digitada no inicio do programa e concatena com o horário encontrado (ddMMyyhmmss);
		1.3- A partir dessa string (senha+ddMMyyhmmss) é gerada a hash da senha, que, caso seja recebida antes de 60 segundos após a geração a mensagem, será igual à de criptografia;
		1.4- A string cripografada é descriptografada com uma chave simétrica, que será a a hash da senha:
			1.4.1- O texto(criptografado) e a senha (hash) são transformados em array de inteiros e array de caracteres, respectivamente;
			1.4.2- O array da senha é repetido para que fique do mesmo tamanho do array do texto;
			1.4.3- Para cada número do texto criptogragado:
				1.4.3.1- Os caracteres  da senha são convertidos em números;
				1.4.3.2- Os números são subtraidos;
				1.4.3.3- Os números são convertidos para caracteres;
				1.4.3.4- Os caracteres são concatenados em uma mesma string;
			1.4.4- A função retorna o texto descriptografado.
		1.5- O programa exibe o texto descriptografado juntamente com o nome da pessoa que enviou.
