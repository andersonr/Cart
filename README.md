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
- [x] Remover um item do carrinho
- [x] Atualizar quantidade de um item do carrinho
- [x] Limpar o carrinho
- [x] Adicionar cupom de desconto ao carrinho       
- [x] Gerar totais e subtotais
        - Subtotais: Preço unitário, Preço unitário com desconto do cupom e total daquele item(*qtdade)
        - Total do carrinho com e sem o desconto - Já fica armazenado na base, mas disponibilizei um endpoint para trazer a totalização também
- [x] Salvar carrinhos 
    - [x] Disponibiliza um campo para atrelar o carrinho a um usuário "logado", porém, não foi feito o controle de autenticação
    - [x] Salvar o carrinho construido, sem a necessidade de login, através de controle de cookies
- [x] Retornar um JSON com todos os itens do carrinho


### Melhorias
1. Link para a página do produto daquele item
2. Existem produtos que possuem customizações. Ex.: 
    - Roupas é possível escolher a cor, tamanho
    - Pizzas é possível escolher os sabores
    - Eletronicos é possível escolher a voltagem 110V ou 220V
    
    Seria legal permitir essa customizações direto nos itens do carrinhos
3. Desenvolver rotinas para armazenar uma lista de carrinhos "favoritos" do usuário

### TechStack
- .net 5 
- EntityFrameWork Core - ORM básico
- SqLite - Fazer a persistência 
- Flunt - Auxiliar validação dos dados recebidos na api
- Swagger - Para documentar api

### Imagens do Docker
Tentar entregar em apenas 1 imagem, para facilitar. 
Então, se faz necessário, além do Docker, também a utilização do Docker Compose, aonde serão criados 2 containers do DocSite

### Git
Descrição dos commits segue o padrão 'Conventional Commits' para permitir a geração de um changelog automaticamente.

### Falta fazer    
    Testes unitários
    Testes de performance e carga
    Refatorar 
        1. Idioma das váriaveis no código
        2. Remover classes e comentários não usados
    Melhorar essa documentação
    Compartilhar com os perfis necessários. 

### Estrutura fisica das tabelas imaginadas
![Tabelas](https://github.com/andersonr/Cart/blob/main/Diagrama%20visual.png)
