Comunicação criptografada de descriptografada por uma hash única e por um número inteiro até 60.
Os 2 computadores devem ter datas sincronizadas (hora/min/seg) => Caso não estejam sincronizados (diferença de mais de 30 segundos) a criptografia não vai funcionar
Mesmo com os horários sincronizados, se a mensagem demorar mais de 30 segundos para ser recebida, será descriptografada de forma errada. (Aparecerão numeros esquisitos...)

Forma de envio:
Mensagem descriptografada ──────────────────────┬───Gera mensagem criptografada───envia em variável $msg
senha para sincronia      ────────┐             │
Horário                   ───┬────┴─Gera hash───┘
Segundos                     └──pega os segundos a partir do horario───────────envia em variavel $time

Forma de recebimento:
Mensagem criptografada                  ─$msg──────────────────────────────────────────────────────────┬Descriptografa a mensagem
senha para sincronia(mesma do envio)    ───────────────────────────────────┐                           │
Horário                                 ───────┬Procura horario de envio*──┴Gera hash(igual a de envio)┘
Segundo                                 ──$time┘

* Ao procurar o horário, o sistema vai ver qual o tempo é mais próximo. Ex:
Horario de envio foi 11:02:34
Ao enviar a mensagem, o sistema grava o segundo em que a mensagem foi enviada e mostra para o outro computador a variável $time=34
ao receber a mensagem (que deve ter um delay maximo de 30 segundos), o sistema recebe a a variável e e procura qual o tempo ele recebeu: se ele ecebeu às 11:02:38, ele vai procurar o horário mais próximo com 34 segundos.
Com isso, é possível criptografar a mensagem de forma que, para cada mensagem, é necessário que o invasor tenha que fazer um bruteforce. Tornando quase impossível descriptografar todas as mensagens.

Para que um invasor consiga descriptografar, ele deve ter a senha para sincronia ou então tentar brute force para cada mensagem.


Processo de criação da hash:
Primeiro é necessário ter a senha para sincronia. Essa senha pode ser, futuramente, dividida em outras substrings que podem ser outros valores. é só concatena-los juntos...
Coloque a senha de sincronia em um variável string $hash_descriptografada
Concatenar a $hash com o o horário (definir a forma de exibição.) no PHP o script para obter o tempo seria o seguinte: time(); (Tomar cuidado com mudança de fuso horários. Ver o funcionamento no C#) 
Com isso, temos uma string de $hash_descriptografada como a seguinte: "senhasupersecreta1408733847"
Agora precisamos encriptar isso na hash sha256. No PHP é só fazer o seguinte: hash('sha256', $hash_descriptografada);
Pronto, essa hash foi gerada e após isso é necessário gerar a hash da variável $msg

Programa +- em PHP para criação da hash:
<?php
	$senha = "senhasupersecreta"; //Senha que deve ser igual entre os dois computadores
	$hora = time(); //pega um numero inteiro de todos os segundos. (Assim pode gerar a data completa)
	$hash_descriptografada = $senha.$hora; //soma a senha com a hora
	$hash = hash('sha256', $hash_descriptografada); //gera a hash. Isso irá criptografar a mensagem
	$time = date('s',$hora); //gera os segundos. Também será enviado para o outro computador
?>