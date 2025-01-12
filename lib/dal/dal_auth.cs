﻿using System;
using lib.dal;
using lib.dto;
using lib.str;

namespace lib.lib.dal
{
    public class dal_auth
    {
        public dto_usuario dal_login(dto_usuario adt, dal_conexao acn = null)
        {
            dal_conexao dalcon = acn ?? new dal_conexao();
            dalcon.OpenConn(acn == null);

            string vsql = string.Empty;

            try
            {
                vsql =
                    "SELECT " +
                    "   ID, Nome, Email, Senha, Telefone, " +
                    "   Instagram, Linkedin, Github, img_perfil " +
                    "FROM usuario " +
                    "WHERE 1=1 ";

                if ((adt?.Email?.Length ?? 0) > 0)
                    vsql += "AND Email = ?Email ";

                if ((adt?.Senha?.Length ?? 0) > 0)
                    vsql += "AND Senha = ?Senha ";

                dalcon.CreateCommand(vsql);

                if ((adt?.Email?.Length ?? 0) > 0)
                    dalcon.AddParams("?Email", adt?.Email);

                if ((adt?.Senha?.Length ?? 0) > 0)
                    dalcon.AddParams("?Senha", adt?.Senha);

                dalcon.ExecReader();

                dto_usuario usr = null;

                if (dalcon.Reader.Read())
                {
                    usr = new dto_usuario()
                    {
                        ID = str_lib.CampoInt64(dalcon.Reader, "ID"),
                        Nome = str_lib.CampoString(dalcon.Reader, "Nome"),
                        Email = str_lib.CampoString(dalcon.Reader, "Email"),
                        Senha = str_lib.CampoString(dalcon.Reader, "Senha"),
                        Telefone = str_lib.CampoString(dalcon.Reader, "Telefone"),
                        Instagram = str_lib.CampoString(dalcon.Reader, "Instagram"),
                        Linkedin = str_lib.CampoString(dalcon.Reader, "Linkedin"),
                        Github = str_lib.CampoString(dalcon.Reader, "Github"),
                        dt_cadastro = str_lib.CampoDateTime(dalcon.Reader, "dt_cadastro"),
                        img_perfil = str_lib.CampoBlob(dalcon.Reader, "img_perfil")
                    };
                }

                dalcon.CloseReader();
                dalcon.CloseConn(true, fecharConexao: acn != null);

                return usr;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public dto_usuario dal_max_cod(dal_conexao acn = null)
        {
            dal_conexao dalcon = acn ?? new dal_conexao();
            dalcon.OpenConn(acn == null);

            string vsql = string.Empty;

            try
            {
                vsql = "SELECT MAX(ID) AS ID FROM usuario";


                dalcon.CreateCommand(vsql);
                dalcon.ExecReader();

                dto_usuario rst = new dto_usuario();
                if (dalcon.Reader.Read())
                {
                    rst.ID = str_lib.CampoInt64(dalcon.Reader, "ID") + 1;
                }

                dalcon.CloseReader();
                dalcon.CloseConn(acn == null);

                return rst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void dal_atualizar(dto_usuario adt, dal_conexao acn = null)
        {
            dal_conexao dalcon = acn ?? new dal_conexao();
            dalcon.OpenConn(acn == null);

            string vsql = string.Empty;

            try
            {
                vsql =
                     "UPDATE USUARIO SET" +
                     "   Nome = ?Nome, Email = ?Email, Senha = ?Senha, Telefone = ?Telefone, " +
                     "   Instagram = ?Instagram, Linkedin = ?Linkedin, Github = ?Github, img_perfil = ?img_perfil" +
                     " WHERE Email = ?Email ";

                if ((adt?.ID ?? 0) > 0)
                    vsql += " AND ID = ?ID";

                dalcon.CreateCommand(vsql);
                dalcon.AddParams("?Email", adt?.Email);

                if ((adt?.ID ?? 0) > 0)
                    dalcon.AddParams("?ID", adt.ID);

                dalcon.AddParams("?Nome", adt?.Nome);
                dalcon.AddParams("?Senha", adt?.Senha);
                dalcon.AddParams("?Telefone", adt?.Telefone);
                dalcon.AddParams("?Instagram", adt?.Instagram);
                dalcon.AddParams("?Linkedin", adt?.Linkedin);
                dalcon.AddParams("?Github", adt?.Github);
                dalcon.AddParams("?img_perfil", ((adt?.img_perfil?.Length ?? 0) > 0) ? adt?.img_perfil : null);

                dalcon.ExecSQL();
                dalcon.CloseConn(acn == null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void dal_cadastro(dto_usuario adt, dal_conexao acn = null)
        {
            dal_conexao dalcon = acn ?? new dal_conexao();
            dalcon.OpenConn(acn == null);

            string vsql = string.Empty;

            try
            {
                vsql =
                     "INSERT INTO USUARIO (" +
                     "   ID, Nome, Email, Senha, Telefone, " +
                     "   Instagram, Linkedin, Github, dt_cadastro, img_perfil" +
                     ") VALUES (" +
                     "   ?ID, ?Nome, ?Email, ?Senha, ?Telefone, " +
                     "   ?Instagram, ?Linkedin, ?Github, ?dt_cadastro, ?img_perfil" +
                     ")";

                dalcon.CreateCommand(vsql);

                dalcon.AddParams("?ID", adt.ID);
                dalcon.AddParams("?Nome", adt?.Nome);
                dalcon.AddParams("?Email", adt?.Email);
                dalcon.AddParams("?Senha", adt?.Senha);
                dalcon.AddParams("?Telefone", adt?.Telefone);
                dalcon.AddParams("?Instagram", adt?.Instagram);
                dalcon.AddParams("?Linkedin", adt?.Linkedin);
                dalcon.AddParams("?Github", adt?.Github);
                dalcon.AddParams("?dt_cadastro", adt?.dt_cadastro);
                dalcon.AddParams("?img_perfil", (adt?.img_perfil.Length > 0) ? adt?.img_perfil : null);

                dalcon.ExecSQL();
                dalcon.CloseConn(acn == null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
