using System;
using System.Collections.Generic;
using System.Globalization;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;

namespace GestaoFrotas.ConsoleApp.Views
{
    public class OsView
    {
        private static OsService _osService = new OsService();

        public void ViewExibirMenuOS()
        {
            LimparTela();
            while (true)
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg(" GESTÃO DE ORDEM DE SERVIÇO ");
                MostrarMsg("=========================================");
                MostrarMsg("");
                MostrarMsg("Selecione uma opção:");
                MostrarMsg("1 - Cadastrar Nova OS");
                MostrarMsg("2 - Consultar OS");
                MostrarMsg("3 - Editar OS");
                MostrarMsg("4 - Excluir OS");
                MostrarMsg("5 - Listar Todas as OS");
                MostrarMsg("");
                MostrarMsg("0 - Voltar ao Menu Principal");
                MostrarMsg("");
                MostrarMsg("Digite sua opção: ");
                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        ViewCadastrarOs();
                        break;
                    case "2":
                        ViewConsultarOs();
                        break;
                    case "3":
                        ViewEditarOs();
                        break;
                    case "4":
                        excluirOs();
                        break;
                    case "5":
                        ListarOS();
                        break;
                    case "0":
                        return;
                    default:
                        MostrarMsg("=========================================");
                        MostrarMsg("OPÇÃO INVALIDA, TENTE NOVAMENTE");
                        MostrarMsg("=========================================");
                        break;
                }
            }
        }

        private void ViewCadastrarOs()
        {
            string msg;
            OrdemDeServico novaOS = new OrdemDeServico();
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" CADASTRAR NOVA OS ");
            MostrarMsg("=========================================");
            MostrarMsg("Para cancelar deixa a data em branco e pressione enter\n");

            MostrarMsg("Digite a data de abertura (ex: 10/11/2025): ");
            novaOS.dataAbertura = Console.ReadLine();
            if (string.IsNullOrEmpty(novaOS.dataAbertura))
                return;
            MostrarMsg("Digite o tipo de manutenção (1- Corretiva | 2- Preventiva): ");
            novaOS.tipo = Console.ReadLine();
            MostrarMsg("Digite o veículo: ");
            novaOS.veiculoId = Console.ReadLine();
            MostrarMsg("Digite o ID do mecânico: ");
            novaOS.mecanicoId = Console.ReadLine();
            MostrarMsg("Digite a data de fechamento (ex: 10/11/2025): ");
            novaOS.dataFechamento = Console.ReadLine();
            MostrarMsg("Digite o ID das peça usada: "); // uma ou mais peças ? 
            novaOS.pecas = Console.ReadLine();
            

            msg = _osService.CadastrarOS(novaOS);
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(msg);
            MostrarMsg("=========================================");
            Pausar();
        }
        private OrdemDeServico ViewConsultarOs()
        {
            string cod;
            int opAcao;
            OrdemDeServico buscarOS = null;
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" CONSULTAR ORDEM DE SERVIÇO ");
            MostrarMsg("=========================================");

            MostrarMsg("Digite o ID da OS: ");
            cod = Console.ReadLine();

            int erroEntrada;
            buscarOS = _osService.buscarOsID(cod, out erroEntrada);
            if (erroEntrada == 1)
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg("ERRO- NENHUM VALOR DIGITADO!!");
                MostrarMsg("=========================================");
                Pausar();
                return null;
            }
            else if (erroEntrada == 2)
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg("ERRO- O ID TEM QUE SER UM NÚMERO!!");
                MostrarMsg("=========================================");
                Pausar();
                return null;
            }

            if (buscarOS == null)
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg(" ERRO - ORDEM DE SERVIÇO NÃO ENCONTRADA ");
                MostrarMsg("=========================================");
                Pausar();
                return null;
            }
            else
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg($"ORDEM DE SERVIÇO ID: {cod} ");
                MostrarMsg("=========================================");

                MostrarMsg($"Data de abertura: {buscarOS.dataAbertura}");
                MostrarMsg($"Tipo de Os: {buscarOS.tipo}");
                MostrarMsg($"Veiculo: {buscarOS.veiculoId}");  // puxar o nome do veiculo.
                MostrarMsg($"Mecânico: {buscarOS.mecanicoId}"); //puxar o nome do mecanico;
                MostrarMsg($"Data de fechamento: {buscarOS.dataFechamento}");
                MostrarMsg($"Peças utilizadas: {buscarOS.pecas}");

                MostrarMsg("\n1- Editar OS");
                MostrarMsg("2- Excluir OS");
                MostrarMsg("3- Adicionar estoque");
                MostrarMsg("0- Voltar ao menu\n");
                MostrarMsg("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opAcao))
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg("OPÇÃO INVALIDA");
                    MostrarMsg("=========================================");
                    Pausar();
                    return null;
                }
                if (opAcao == 1)
                    ViewEditarOsConsult(buscarOS);
                else if (opAcao == 2)
                    ViewExcluirOsConsult(buscarOS);
                else if (opAcao == 3) { }
                //ViewAdicionarPecaOs(buscarOS);
                else if (opAcao == 0)
                    return null;
                else
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg("OPÇÃO INVALIDA");
                    MostrarMsg("=========================================");
                    Pausar();
                    return null;
                }
                return buscarOS;
            }
        }
        private void ViewEditarOs()
        {
            string novaDataAbert;
            string novoTipoManut;
            string novoVeic;
            string novoIdMotor;
            string novaDataFecham;
            string novaPeca;
            string cod;  
            string op;
            OrdemDeServico buscarOS = null;
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" EDITAR PEÇA ");
            MostrarMsg("=========================================");

            MostrarMsg("Digite o ID da OS: \n");
            cod = Console.ReadLine();

            int erroEntrada;
            buscarOS = _osService.buscarOsID(cod, out erroEntrada);
            if (erroEntrada == 1)
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg("ERRO- NENHUM VALOR DIGITADO!!");
                MostrarMsg("=========================================");
                Pausar();
                return;
            }
            else if (erroEntrada == 2)
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg("ERRO- O ID TEM QUE SER UM NÚMERO!!");
                MostrarMsg("=========================================");
                Pausar();
                return;
            }
            if (buscarOS == null)
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg(" ERRO - PEÇA NÃO ENCONTRADA ");
                MostrarMsg("=========================================");
                Pausar();
                return;
            }
            else
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg($"ORDEM DE SERVIÇO ID: {cod} ");
                MostrarMsg("=========================================");

                MostrarMsg($"Data de abertura: {buscarOS.dataAbertura}");
                MostrarMsg($"Tipo de Os: {buscarOS.tipo}");
                MostrarMsg($"Veiculo: {buscarOS.veiculoId}");  // puxar o nome do veiculo.
                MostrarMsg($"Mecânico: {buscarOS.mecanicoId}"); //puxar o nome do mecanico;
                MostrarMsg($"Data de fechamento: {buscarOS.dataFechamento}");
                MostrarMsg($"Peças utilizadas: {buscarOS.pecas}");
            }
            MostrarMsg("\n=======================================");
            MostrarMsg(" NOVOS DADOS ");
            MostrarMsg("=========================================");

            MostrarMsg("Digite a nova data de abertura (ex: 10/11/2025): ");
            novaDataAbert = Console.ReadLine();
            MostrarMsg("Digite o novo tipo de manutenção (1- Corretiva | 2- Preventiva): ");
            novoTipoManut = Console.ReadLine();
            MostrarMsg("Digite o novo veículo: ");
            novoVeic = Console.ReadLine();
            MostrarMsg("Digite o novo ID do mecânico: ");
            novoIdMotor= Console.ReadLine();
            MostrarMsg("Digite a nova data de fechamento (ex: 10/11/2025): ");
            novaDataFecham = Console.ReadLine();
            MostrarMsg("Digite o novo ID das peça usada: "); // uma ou mais peças ? 
            novaPeca = Console.ReadLine();

            MostrarMsg("Confirmar alterações? (S/N)");
            op = Console.ReadLine();
            string msg = _osService.EditarOs(buscarOS, novaDataAbert, novoTipoManut, novoVeic, novoIdMotor, novaDataFecham, novaPeca, op);
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(msg);
            MostrarMsg("=========================================");
            Pausar();
            return;
        }

        public void ViewEditarOsConsult(OrdemDeServico ordemDeServico)
        {
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" EDITAR ORDEM DE SERVIÇO ");
            MostrarMsg("=========================================");

            MostrarMsg($"Data de abertura: {ordemDeServico.dataAbertura}");
            MostrarMsg($"Tipo de Os: {ordemDeServico.tipo}");
            MostrarMsg($"Veiculo: {ordemDeServico.veiculoId}");  // puxar o nome do veiculo.
            MostrarMsg($"Mecânico: {ordemDeServico.mecanicoId}"); //puxar o nome do mecanico;
            MostrarMsg($"Data de fechamento: {ordemDeServico.dataFechamento}");
            MostrarMsg($"Peças utilizadas: {ordemDeServico.pecas}");

            MostrarMsg("=========================================");
            MostrarMsg(" NOVOS DADOS: ");
            MostrarMsg("=========================================\n");

            MostrarMsg("Digite a nova data de abertura (ex: 10/11/2025): ");
            string novaDataAbert = Console.ReadLine();
            MostrarMsg("Digite o novo tipo de manutenção (1- Corretiva | 2- Preventiva): ");
            string novoTipoManut = Console.ReadLine();
            MostrarMsg("Digite o novo veículo: ");
            string novoVeic = Console.ReadLine();
            MostrarMsg("Digite o novo ID do mecânico: ");
            string novoIdMotor= Console.ReadLine();
            MostrarMsg("Digite a nova data de fechamento (ex: 10/11/2025): ");
            string novaDataFecham = Console.ReadLine();
            MostrarMsg("Digite o novo ID das peça usada: "); // uma ou mais peças ? 
            string novaPeca = Console.ReadLine();
            MostrarMsg("\nConfirmar alterações? (S/N): ");
            string op = Console.ReadLine();
            string msg = _osService.EditarOs(ordemDeServico, novaDataAbert, novoTipoManut, novoVeic, novoIdMotor, novaDataFecham, novaPeca, op);
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(msg);
            MostrarMsg("=========================================");
            Pausar();
        }

        public void ViewExcluirOsConsult(OrdemDeServico ordemDeServico)
        {
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" EXCLUIR ORDEM DE SERVIÇO ");
            MostrarMsg("=========================================");

            MostrarMsg($"Data de abertura: {ordemDeServico.dataAbertura}");
            MostrarMsg($"Tipo de Os: {ordemDeServico.tipo}");
            MostrarMsg($"Veiculo: {ordemDeServico.veiculoId}");  // puxar o nome do veiculo.
            MostrarMsg($"Mecânico: {ordemDeServico.mecanicoId}"); //puxar o nome do mecanico;
            MostrarMsg($"Data de fechamento: {ordemDeServico.dataFechamento}");
            MostrarMsg($"Peças utilizadas: {ordemDeServico.pecas}");

            MostrarMsg("\nConfirmar a exclusão? (S/N): ");
            string op = Console.ReadLine();
            string msg = _osService.ExcluirOs(ordemDeServico, op);
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(msg);
            MostrarMsg("=========================================");
            Pausar();
        }

        /*public void ViewAdicionarPecaOs(OrdemDeServico ordemDeServico)
        {
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" ADICIONAR PEÇA NA OS ");
            MostrarMsg("=========================================");

            MostrarMsg($"ID: {peca.id}");
            MostrarMsg($"Nome atual: {peca.nome}");
            MostrarMsg($"Quantidade atual: {peca.quantidadeEstoque}");
            MostrarMsg("=========================================\n");
            MostrarMsg("Adicionar quantidade: ");
            string quant = Console.ReadLine();
            MostrarMsg("\nDeseja confirmar a inclusão ? (S/N)");
            string resp = Console.ReadLine();

            string msg = _pecaService.AdicionarEstoque(peca, quant, resp);
            MostrarMsg(msg);
            Pausar();
            if (msg == "\nQUANTIDADE ACRESCENTADA COM SUCESSO!!")
            {
                LimparTela();
                MostrarMsg("\n===== ESTOQUE ATUALIZADO =====");
                MostrarMsg($"ID: {peca.id}");
                MostrarMsg($"Nome: {peca.nome}");
                MostrarMsg($"Quantidade nova: {peca.quantidadeEstoque}");
                Pausar();
            }
        }*/

        public void ListarOS()
        {
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" LISTAR ORDENS DE SERVIÇOS ");
            MostrarMsg("=========================================");
            List<OrdemDeServico> ordens = _osService.ListarOS();

            if (ordens == null)
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg(" NENHUMA PEÇA CADASTRADA! ");
                MostrarMsg("=========================================");
                Pausar();
                return;
            }
            else
            {
                MostrarMsg($"{"ID",-10} {"Data abertura",-17} {"Tipo",-15} {"Veiculo",-13} {"Mecanico",-12} {"Data fechamento",-21} {"Peças",-22}");
                MostrarMsg(new string('-', 105));

                foreach (var dadosOs in ordens)
                {
                    Console.WriteLine(
                                        $"{dadosOs.id,-10} " +
                                        $"{dadosOs.dataAbertura,-17:dd/MM/yyyy} " +
                                        $"{dadosOs.tipo,-15} " +
                                        $"{dadosOs.veiculoId,-13} " +
                                        $"{dadosOs.mecanicoId,-12} " +
                                        $"{dadosOs.dataFechamento,-21:dd/MM/yyyy} " +
                                        $"{dadosOs.pecas,-21}"
                                    );

                }

                Pausar();
            }
        }
        public void excluirOs()
        {
            string cod;
            string op;
            string msg;
            OrdemDeServico buscarOs = null;
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" EXCLUIR ORDEM DE SERVIÇO ");
            MostrarMsg("=========================================");
            MostrarMsg("Para sair deixe o campo em branco e pressione ENTER.");

            MostrarMsg("\nDigite o ID da OS: ");
            cod = Console.ReadLine();
            if (string.IsNullOrEmpty(cod))
                return;
            int erroEntrada;
            buscarOs = _osService.buscarOsID(cod, out erroEntrada);
            if (erroEntrada == 1)
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg("ERRO- NENHUM VALOR DIGITADO!!");
                MostrarMsg("=========================================");
                return;
            }
            else if (erroEntrada == 2)
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg("ERRO- O ID TEM QUE SER UM NÚMERO!!");
                MostrarMsg("=========================================");
                Pausar();
                return;
            }
            else if (buscarOs == null)
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg(" ERRO - ODEM DE SERVIÇO NÃO ENCONTRADA ");
                MostrarMsg("=========================================");
                Pausar();
                return;
            }
            else
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg($"ORDEM DE SERVIÇO ID: {cod} ");
                MostrarMsg("=========================================");
                MostrarMsg($"Data de abertura: {buscarOs.dataAbertura}");
                MostrarMsg($"Tipo de Os: {buscarOs.tipo}");
                MostrarMsg($"Veiculo: {buscarOs.veiculoId}");  // puxar o nome do veiculo.
                MostrarMsg($"Mecânico: {buscarOs.mecanicoId}"); //puxar o nome do mecanico;
                MostrarMsg($"Data de fechamento: {buscarOs.dataFechamento}");
                MostrarMsg($"Peças utilizadas: {buscarOs.pecas}");

                MostrarMsg("\nDeseja realmente excluir? (S/N)");
                op = Console.ReadLine();
                msg = _osService.ExcluirOs(buscarOs, op);
                LimparTela();
                MostrarMsg(msg);
                Pausar();
                return;
            }
        }
        
        public void MostrarMsg(string mensagem)
        {
            Console.WriteLine(mensagem);
        }
        //---------------------------------------------------------------------------------------------------
        public void Pausar(string msg = "Pressione qualquer tecla para continuar...")
        {
            Console.WriteLine();
            Console.Write(msg);
            Console.ReadKey(true);
        }
//---------------------------------------------------------------------------------------------------
       public void LimparTela()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write("\x1b[3J");
        }

    }
}