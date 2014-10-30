var crypto = require('crypto');
var texto="Este texto é criptografado usando meu novo método de criptografia. A vantagem desse tipo de criptografia é que, para cada segundo, uma chave totalmente diferente é usada.";
var senha = "senhasupersecreta"; //Senha que deve ser igual entre os dois computadores
var data_agora = new Date(); // só pra fazer uma média mesmo
var hora = Math.floor(data_agora.getTime()/1000);//pega um numero inteiro de todos os segundos. (Assim pode gerar a data completa)
var hash_descriptografada = senha+hora; //soma a senha com a hora
var hash = crypto.createHash('sha256').update(hash_descriptografada).digest('hex'); //gera a hash. Isso irá criptografar a mensagem --> incompatibilidade com o PHP - ainda comprime em base64
var time = data_agora.getSeconds(); //gera os segundos. Também será enviado para o outro computador
var criptografador = crypto.createCipher('aes-256-cbc',hash);
var msg = criptografador.update(texto, 'utf8', 'base64');

console.log("hora: "+hora);
console.log("Texto: "+texto);
console.log("msg: "+msg);
console.log("time: "+time);

console.log();
console.log();
console.log();
console.log();

console.log("Variável \$time recebida: "+time);
console.log("Variável \$msg recebida: "+msg);

var senha = "senhasupersecreta"; //Senha que deve ser igual entre os dois computadores
var now = new Date();
var data_agora = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), time, 0);
var data_segundos = Math.floor(data_agora.getTime()/1000); //regenera a data em segundos recebida pela variavel time
var hash_descriptografada = senha+data_segundos;
var hash = crypto.createHash('sha256').update(hash_descriptografada).digest('hex'); //gera a hash. Isso irá criptografar a mensagem --> incompatibilidade com o PHP - ainda comprime em base64
var descriptografador = crypto.createDecipher('aes-256-cbc',hash);
var texto_descriptografado = descriptografador.update(msg, 'base64', 'utf8');
console.log("Menagem descriptografada: "+texto_descriptografado);
