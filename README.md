# TheFlashLoan
Contrato e buscador de oportunidades para flashloan na Aave.

##Passos
Entrar no Remix
http://remix.ethereum.org/

Clicar no botão "GitHub" abaixo de "Import from" e entrar com:
https://github.com/jacksonsavitraz/theflashloan/blob/master/Contract/Flashloan.sol

Com o arquivo "Flashloan.sol" aberto, na aba "Compiler" marcar:
[X] Auto compile
[X] Enable optimization

Na aba "Deploy & Run", escolher:
Environment: Injected Web3

Após conectar sua carteira, clique no botão "Deploy" para gerar o contrato.
Depois de criado, o mesmo vai aparecer na lista "Deployed contracts". Se não aparecer, basta colar o endereço no campo "At address".
No contrato, escolha o método "flashloan" clicando na seta ao lado para expandir os campos "amount" e "arbitrage".
O campo "amount" é o total emprestado da moeda inicial via flashloan, já acrescito a quantidade de zeros decimais.
O campo "arbitrage" é o vetor de tokens que representam a oportunidade de arbitragem.

Compilar o projeto "Arbitrage.sln" e aguardar uma nova oportunidade.
Quando acontecer, basta copiar os parâmetros no Remix e clicar em "Transact".