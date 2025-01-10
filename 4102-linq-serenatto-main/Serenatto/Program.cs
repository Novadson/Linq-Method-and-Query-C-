using SerenattoEnsaio.Dados;
using SerenattoEnsaio.Modelos;
using SerenattoPreGravacao.Dados;
using System.Diagnostics.Metrics;

List<Cliente> clientes = DadosClientes.GetClientes();

List<string> pagamentos = DadosFormaDePagamento.FormasDePagamento.OrderBy(c => c).Where(c => c.Contains('c')).ToList();

List<Produto> produtosLoja = DadosCardapio.GetProdutos().OrderBy(p => p.Nome).ToList();
List<Produto> produtosDelivery = DadosCardapio.CardapioDelivery().OrderBy(p => p.Nome).ToList();





// TRAZER PRODUTOS NÃO REPETIDOS
var QuantidadeItensPedidosPorDias = DadosPedidos.QuantidadeItensPedidosPorDia.SelectMany(item => item).Distinct().OrderBy(p => p).ToList();

var QuantidadeItensPedidosPorDs = (from item in DadosPedidos.QuantidadeItensPedidosPorDia
                                   from item2 in item
                                   select item2)
                                  .Distinct()
                                  .OrderBy(item2 => item2)
                                  .ToList();


var UnirList = QuantidadeItensPedidosPorDias.Union(QuantidadeItensPedidosPorDs).ToList();




var produtosDiferenca = DadosCardapio.GetProdutos()
                                     .Except(DadosCardapio.CardapioDelivery())
                                     .ToList();

Console.WriteLine("ALL PRODUCTS REPORT");

// first way using AddRange


foreach (var item in DadosCardapio.GetProdutos()
                                  .OrderBy(p => p.Nome)
                                  .ThenBy(p =>p.Preco))
{
    Console.WriteLine($"Nome {item.Nome} | Preço {item.Preco}");
}


var numbers = new[] { 1, 2, 3, 4, 5 };
var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();


int counter = 0;

do
{
    Console.WriteLine(counter); // Prints 0 to 4
    counter++;
} while (counter < 5);



//QuantidadeItensPedidosPorDs.ForEach(p => Console.WriteLine(p));

//Console.WriteLine("QUATIDADE DE ITENS DIFERENTE");


//trazer quantidade de pedido que tenha items diferente


/*var QuantidadeItensPedidosPorDias = DadosPedidos.QuantidadeItensPedidosPorDia.SelectMany(dailyOrder => dailyOrder)
                                                                             .Count(p => p.Equals(1));

var QuantidadeItensPedidosPorDiasQuery = from p in DadosPedidos.QuantidadeItensPedidosPorDia
                                         .SelectMany(p => p)
                                         where p == 1
                                         select p;

// SELECTMANY OR SUBSLECT 

var QuantidadeItensPedidosPorDiasQuerySelecyMany = from mainList in DadosPedidos.QuantidadeItensPedidosPorDia
                                                   from item in mainList
                                                   where item == 1
                                                   select item;


Console.WriteLine($"Pedido Linq query: {QuantidadeItensPedidosPorDiasQuery}");



var pagmento = from p in pagamentos.SelectMany(dailyOrder => dailyOrder)
               select p;




Console.WriteLine("ALL QTT OF PRODCUT PONTHLY");

//QuantidadeItensPedidosPorDias.ForEach(lista => Console.Write($"{lista} "));

Console.WriteLine(" ");


//foreach (var item in QuantidadeItensPedidosPorDi)
//{
//    Console.WriteLine(item);
//}

SHOW CLIENTS REPORT
Console.WriteLine("SHOW CLIENTS REPORT");

foreach (var client in clientes)
    Console.WriteLine($"Id:{client.Id} | Nome: {client.Nome} | " +
        $"Telefone: {client.Telefone} |Endereço:{client.Endereco}");

/*SHOW PAYMENT METHOD REPORT
Console.WriteLine("SHOW PAYMENT METHOD REPORT");

foreach (var pagamento in pagamentos)
    Console.WriteLine(string.Join(" ", pagamento));



Console.WriteLine("SHOW PRODUCT REPORT");
*/
/*var produtoList = from p in produtos
                  group p by p.Nome into g
                  where g.Count() > 1
                  select new
                  {
                      Nome = g.Key,
                      TotalPreco = g.Sum(p => p.Preco),
                      Descricoes = string.Join(", ", g.Select(p => p.Descricao)),
                      Quatidade = g.Count()
                  };


var produtoLi = produtos.GroupBy(p => p.Nome)
                        .Where(p => p.Count() > 1)
                        .Select(p => new
                        {
                            Nome = p.Key,
                            TotalPreco = p.Sum(c => c.Preco),
                            Descricoes = string.Join(", ", p.Select(x => x.Descricao).Distinct()),
                            Quantidade = p.Count()
                        });


foreach (var p in produtoLi)
    Console.WriteLine($"Nome:{p.Nome} | Descricao{p.Descricoes} | TotalPreco:{p.TotalPreco}");


var produtoPorNome = produtos.GroupBy(g => g.Nome) // Group by the "Nome" property
                             .Where(p => p.Count() > 1) // Filter groups with more than one item
                             .Select(g => new
                             {
                                 Nome = g.Key // Select the "Nome" property as Key
                             }).ToList();

produtoPorNome.ForEach(p => Console.WriteLine(p.Nome));

Console.WriteLine("----------------------------------------");

Console.WriteLine("PRODUCT BY PRICE REPORT");

var produtosPorPreco = produtos.Select(p => new
{
    NomeProduto = p.Nome,
    PrecoProduto = p.Preco
}).OrderBy(p => p.NomeProduto).ToList();


produtosPorPreco.ForEach(p =>
Console.WriteLine($"Nome: {p.NomeProduto} | Preco: R$ {p.PrecoProduto}")
);

// buy 4 pay 3

var produtoPromocao = produtos.OrderBy(p => p.Nome).Select(p => new
{
    NomeProduto = p.Nome,
    PrecoCombo = p.Preco * 3,
    PrecoUnitario = p.Preco,
}).ToList();


Console.WriteLine("----------------------------------------");
Console.WriteLine("PRODUCT REPORT COMBO PRICE BUY 4 PAY 3");

foreach (var p in produtoPromocao)
    Console.WriteLine($"Nome:{p.NomeProduto} | Preço Unitário:R${p.PrecoUnitario:F2} Preço Combo:R${p.PrecoCombo:F2}");
*/

Console.ReadKey();