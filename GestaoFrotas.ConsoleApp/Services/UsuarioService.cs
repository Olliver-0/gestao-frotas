using GestaoFrotas.ConsoleApp.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GestaoFrotas.ConsoleApp.Services
{
    public class UsuarioService
    {
        private static List<Usuario> _usuarios = new List<Usuario>();
        private static int _proximoId = 1;

        public UsuarioService()
        {
            if (_usuarios.Count == 0)
            {
                // Usuário Administrador padrão para login em Program.cs
                _usuarios.Add(new Usuario { id = _proximoId++, nome = "Admin Master", login = "admin@frota.com", senha = "123", perfil = "Administrador" });
                _usuarios.Add(new Usuario { id = _proximoId++, nome = "Coordenador Alfa", login = "coord@frota.com", senha = "123", perfil = "Coordenador" });
            }
        }

        public Usuario ConsultarPorLogin(string login)
        {
            return _usuarios.FirstOrDefault(u => u.login.Equals(login, StringComparison.OrdinalIgnoreCase));
        }

        public Usuario ConsultarPorId(int id)
        {
            return _usuarios.FirstOrDefault(u => u.id == id);
        }

        public List<Usuario> ConsultarTodos()
        {
            return _usuarios.OrderBy(u => u.id).ToList();
        }
        public string CadastrarUsuario(string nome, string login, string senha, string perfil)
        {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(senha))
                return "Erro: Todos os campos são obrigatórios.";

            if (_usuarios.Any(u => u.login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                return "Erro: Já existe um usuário com esse login.";

            var novoUsuario = new Usuario
            {
                id = _proximoId++,
                nome = nome,
                login = login,
                senha = senha,
                perfil = perfil
            };

            _usuarios.Add(novoUsuario);
            return $"Sucesso: Usuário '{nome}' cadastrado com sucesso!";
        }

        public string EditarUsuario(int id, string nome, string login, string perfil)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.id == id);
            if (usuario == null)
                return "Erro: Usuário não encontrado.";

            usuario.nome = nome;
            usuario.login = login;
            usuario.perfil = perfil;

            return $"Sucesso: Usuário '{usuario.nome}' atualizado!";
        }

        public string ExcluirUsuario(int id)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.id == id);
            if (usuario == null)
                return "Erro: Usuário não encontrado.";

            _usuarios.Remove(usuario);
            return $"Sucesso: Usuário '{usuario.nome}' excluído!";
        }
    }
}

