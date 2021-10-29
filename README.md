# Cart

Criar uma api que possa ser utilizada na criação de um carrinho de compras de um e-commerce.

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

### TechStack
- .net 5 
- EntityFrameWork Core - ORM básico
- SqLite - Fazer a persistência 
- Flunt - Auxiliar validação dos dados recebidos na api
- Swagger - Para documentar api

### Padroes
    -> Base
        - Seguido o padrão do EntityFrameWork Core(Pluralizar, ForeignKey)
        - DataAnnotations para garantir a integridade dos dados do ORM
        - Nomenclatura das entidades(tabelas) no idioma Português     
    -> ViewModel api
        - Mantido propriedades no idioma português, para ser padronizado com as nomenclaturas das entidades do BD
        - Validações com Flunt para auxiliar na validação/integridade dos dados trafegados
    -> Controller api
        - OpenApi/WebAPI
        - Um controller por entidade
        - Rotas versionadas(v1)
        - Idioma inglês
        - Suporte processamento assincrono       

### Imagens do Docker
Adicionado arquivo Dockerfile para dar suporte ao desenvolvimento utilizando a imagem.
Feito utilizando a ferramenta do próprio Visual Studio.

### Git
Descrição dos commits segue o padrão 'Conventional Commits' para permitir a geração de um changelog automaticamente.

### Testes
    - Unitários: Não fiz, mas gostaria de ter feito os testes unitários com XUnit. Tive problemas para subir uma instância falsa do SQLite no momento da execução dos testes e acabei não fazendo os testes a tempo.  
    - De consumo da API: Gostaria de ter utilizado o Jest para fazer testes na api mas acabei não fazendo a tempo.

### Melhorias futuras
1. Fazer testes unitários
2. Fazer testes de consumo
3. Existem produtos que possuem customizações. Ex.: 
    - Roupas é possível escolher a cor, tamanho
    - Pizzas é possível escolher os sabores
    - Eletronicos é possível escolher a voltagem 110V ou 220V
    
    Seria legal permitir essa customizações direto nos itens do carrinhos
4. Desenvolver rotinas para armazenar uma lista de carrinhos "favoritos" do usuário

