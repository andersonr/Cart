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
- Adicionar um item ao carrinho
- Remover um item do carrinho
- Atualizar quantidade de um item do carrinho
- Limpar o carrinho
- Adicionar cupom de desconto ao carrinho
    - Apenas determinado item
    - No carrinho todo
- Gerar totais e subtotais
    - Subtotais: Preço unitário, Preço unitário com desconto do cupom e total daquele item(*qtdade)
    - Total do carrinho com e sem o desconto
- Salvar carrinhos 
    - Salvar abaixo do usuário logado, salvando vários carrinhos diferentes, permitindo que usuário logado consiga seleciona-los novamente
    - Salvar o carrinho construido, sem a necessidade de login
- Retornar um JSON com todos os itens do carrinho
- Link para a página do produto daquele item
- Frete
    - Trazer tudo gratuito
- Existem produtos que possuem customizações. Ex.: 
    - Roupas é possível escolher a cor, tamanho
    - Pizzas é possível escolher os sabores
    - Eletronicos é possível escolher a voltagem 110V ou 220V

    Seria legal permitir essa customizações direto no carrinho


### TechStack
- .net 5 - Facilitar a entrega - Se der tempo, trocar pra versão 6 e usar MiniServices
- Redis
- EntityFrameWork Core
- SqLite

### Imagens do Docker
Por enquanto, utilizarei mais de 1 imagem. Um para rodar a aplicação e um pra rodar o Redis pra armazenar os dados em cache.
Então, se faz necessário, além do Docker, também a utilização do Docker Compose, aonde serão criados 2 containers do DocSite




