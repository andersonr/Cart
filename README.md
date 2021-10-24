# Cart

Criar uma api que possa ser utilizada na criação de um carrinho de compras de um e-commerce.

### Pontos de atenção:
    -> Performance
    -> Carga
    -> Tempo de resposta
    -> Tratamentos de erros adequados
    -> Persistencia de carrinhos(usuários logados ou não)  
    -> Documentar como o projeto é iniciado, listando as soluções dos problemas
    -> Montar uma imagem no docker e docker-compose com o ambiente de desenvolvimento
    -> Projeto de testes e documentação de como rodar

### Funcionalidades
- [x] Adicionar um item ao carrinho
- Remover um item do carrinho
- Atualizar quantidade de um item do carrinho
- Limpar o carrinho
- Adicionar cupom de desconto ao carrinho
    - Apenas determinado item
    - No carrinho todo
- Gerar totais e subtotais
    - Subtotais: Preço unitário, Preço unitário com desconto do cupom e total daquele item(*qtdade)
    - Total do carrinho com e sem o desconto
- [x] Salvar carrinhos 
    - Salvar abaixo do usuário logado, salvando vários carrinhos diferentes, permitindo que usuário logado consiga seleciona-los novamente
    - [x] Salvar o carrinho construido, sem a necessidade de login, através de controle de cookies
- Retornar um JSON com todos os itens do carrinho


### Melhorias
1. Link para a página do produto daquele item
2. Existem produtos que possuem customizações. Ex.: 
    - Roupas é possível escolher a cor, tamanho
    - Pizzas é possível escolher os sabores
    - Eletronicos é possível escolher a voltagem 110V ou 220V
    
    Seria legal permitir essa customizações direto no carrinho

### TechStack
- .net 5 - Facilitar a entrega - Se der tempo, trocar pra versão 6 e usar MiniServices
- EntityFrameWork Core
- SqLite

### Imagens do Docker
Tentar entregar em apenas 1 imagem, para facilitar. 
Então, se faz necessário, além do Docker, também a utilização do Docker Compose, aonde serão criados 2 containers do DocSite

### Git
Descrição dos commits segue o padrão 'Conventional Commits' para permitir a geração de um changelog automaticamente.

### Falta implementar
    Funcionalidades requeridas
    Testes unitários
    Testes de performance e carga
    Documentar API

### Estrutura fisica das tabelas imaginadas
![Tabelas](https://github.com/andersonr/Cart/blob/main/Diagrama%20visual.png)
