using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Views;

namespace GestaoFrotas.ConsoleApp.Services
{
    public class AddPecaOsService
    {
        private readonly VeiculoService _veiculoService;
        private readonly MotoristaService _motoristaService;
        private readonly OsService _osService;
        private readonly PecaService _pecaService;

        public AddPecaOsService(
            OsService osService,
            VeiculoService veiculoService,
            MotoristaService motoristaService,
            PecaService pecaService)
        {
            _osService = osService;
            _veiculoService = veiculoService;
            _motoristaService = motoristaService;
            _pecaService = pecaService;
        }

        public string CadastrarPecaOs(int osId, string pecaId, int quantidade, string op)
        {
        string dados = ResumoPecaOs(osId, pecaId, quantidade);

            if (string.IsNullOrEmpty(op) || op == "N" || op == "n")
                return "OPERAÇÃO CANCELADA!!";
            else if (op == "S" || op == "s")
            {
                OrdemDeServico os = _osService.BuscarPorId(osId);
                Peca peca = _pecaService.buscarPecaID(pecaId, out _);
                return $"OK - {dados} CADASTRADA NA OS-{osId}.";
            }
            else
                return "ERRO- OPÇÃO INVÁLIDA!!";
            }



        public string ResumoPecaOs(int osId, string pecaId, int quantidade)
        {

            OrdemDeServico BuscarOs = _osService.BuscarPorId(osId);
            if (BuscarOs == null) return "ERRO - ORDEM DE SERVIÇO NÃO ENCONTRADA!!";
            else
            {
                Peca peca = _pecaService.buscarPecaID(pecaId, out int erroEntrada);
                if (erroEntrada == 1) return "ERRO - NENHUM VALOR DIGITADO!!";
                else if (erroEntrada == 2) return "ERRO - O ID TEM QUE SER UM NÚMERO!!";
                else if (peca == null) return "ERRO - PEÇA NÃO ENCONTRADA!!";
                else
                    return $"{quantidade} quantidades da peça {peca.nome}, ID-{peca.id}";
            }

        }
        public int LerInteiro(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    var s = Console.ReadLine();
                    if (int.TryParse(s, out var v) && v >= 0) return v;
                    Console.WriteLine("Erro: valor inválido. Digite um número.");
                }
            }

        
        
    }

}