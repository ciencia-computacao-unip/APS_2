Programa em PHP para criação da hash e criptografia da senha:
<?php
	$senha = "senhasupersecreta"; //Senha que deve ser igual entre os dois computadores
	$hora = time(); //pega um numero inteiro de todos os segundos. (Assim pode gerar a data completa)
	$hash_descriptografada = $senha.$hora; //soma a senha com a hora
	$hash = hash('sha256', $hash_descriptografada); //gera a hash. Isso irá criptografar a mensagem
	$time = date('s',$hora); //gera os segundos. Será enviado para o outro computador
	$msg = openssl_encrypt($texto, 'AES-256-CBC', $hash); //criptografa o texto com a $hash. Também será enviado para o outro computador
?>

Descriptografar a mensagem (Lembre-se que deve receber as variáveis $msg e $time):
<?php
	$senha = "senhasupersecreta"; //Senha que deve ser igual entre os dois computadores
	$data_antiga = new DateTime(date());
	if($data_agora->format('s')<$time){
		$data_antiga->setTime($data_antiga->format('H'),$data_antiga->format('i')-1,$time);
	}else{
		$data_antiga->setTime($data_antiga->format('H'),$data_antiga->format('i'),$time);
	}
	echo "<br/>".$data_antiga->format('U');
	$hash_antiga_descriptografada = $senha.$data_antiga->format('U');
	$hash_antiga = hash('sha256', $hash_antiga_descriptografada); //gera a hash. Isso irá criptografar a mensagem
	$mensagem_descriptografada = openssl_decrypt($msg, 'AES-256-CBC', $hash_antiga);
?>

Obs.: o tempo de criptografia e descriptografia (somados) não chegou a 100 milisegundos.

